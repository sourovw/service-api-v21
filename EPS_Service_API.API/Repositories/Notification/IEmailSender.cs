using System.Threading.Tasks;

namespace EPS_Service_API.Repositories
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}