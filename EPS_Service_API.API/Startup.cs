using System;
using System.Linq;
using System.Net;
using System.Text;
using EPS_Service_API.API.BankServices;
using EPS_Service_API.API.Data;
using EPS_Service_API.API.Repositories;
using EPS_Service_API.API.Repositories.Notification;
using EPS_Service_API.Model.Notification;
using EPS_Service_API.Repositories;
using EPS_Service_API.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;


namespace EPS_Service_API.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
            });

            services.AddControllers();

            // For Control Version
            services.AddApiVersioning();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EPS_Service_API.API", Version = "v1" });

                // For Control Version
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            // JWT Token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddMvc();

            services.AddSingleton(new DataAccessHelper(Configuration));
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<ICustomerProfileRepository, CustomerProfileRepository>();
            services.AddScoped<ITransactionAccountsOfCustomerRepository, TransactionAccountsOfCustomerRepository>();
            services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();

            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IFCM_TokenRepository, FCM_TokenRepository>();

            // Bank
            services.AddScoped<ICreditService, CreditService>();
            services.AddScoped<IDebitService, DebitService>();

            // Mail
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();

            // Porichoy Log
            services.AddSingleton(new DataHelperSQL_Server(Configuration));
            services.AddScoped<IPorichoyLogTaker, PorichoyLogTaker>();

            services.AddSingleton(new SecurityHelper(Configuration));
            services.AddSingleton(new HashCreatorValidator(Configuration));
            services.AddSingleton(new DeviceValidator());
            services.AddScoped<ISMS_Service, SMS_Service>();
            services.AddScoped<IRechargeOfferRepository, RechargeOfferRepository>();

            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddHttpClient();

            // Included for Auto Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("MySQL"), serverVersion)
            );

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTAuthenticationDemo v1"));
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging(); // Generates entry like this: HTTP "GET" "/" responded 200 in 265.8149 ms
            app.UseRouting();

            // Custom UnAuthorized for JWT

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) // 401
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(new UnauthorizedErr
                    {
                        StatusCode = 401,
                        Message = "Token is not valid"
                    }.ToString());
                }
            });

            // Custom UnAuthorized for JWT
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}