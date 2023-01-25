using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Repositories;

namespace EPS_Service_API.Repositories
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;

        public EmailTemplateService(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<EmailTemplateModel> GetById(int id)
        {

            return (await _dataAccessHelper.QueryData<EmailTemplateModel, dynamic>("USP_EmailTemplates_GetById", new { TemplateID = id })).FirstOrDefault();

        }

        public async Task<List<EmailTemplateModel>> GetList()
        {
            return (await _dataAccessHelper.QueryData<EmailTemplateModel, dynamic>("CustomerProfile_GetAll", new { })).ToList();
        }

        public async Task<int> Update(EmailTemplateModel emailTemplate)
        {

            DynamicParameters p = new DynamicParameters();
            p.Add("TemplateID", emailTemplate.Id);
            p.Add("TemplateSub", emailTemplate.Subject);
            p.Add("NewTemplate", emailTemplate.Template);

            return await _dataAccessHelper.ExecuteData("USP_EmailTemplates_Update", p);

        }
    }
}