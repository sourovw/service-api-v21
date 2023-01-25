using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPS_Service_API.Repositories
{
    public interface IEmailTemplateService
    {
        Task<EmailTemplateModel> GetById(int id);
        Task<List<EmailTemplateModel>> GetList();
        Task<int> Update(EmailTemplateModel emailTemplate);
    }
}