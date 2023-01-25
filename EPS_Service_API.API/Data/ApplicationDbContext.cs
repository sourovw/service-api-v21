using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Model;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EPS_Service_API.API.Data
{
     public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
   // public class ApplicationDbContext : DbContext
    {

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //   : base(options)
        //{
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<RefreshToken>()
            //.HasNoKey();

            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        */

    }


    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

    }

   



}
