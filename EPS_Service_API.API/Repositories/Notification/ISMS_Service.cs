using System.Threading.Tasks;

namespace EPS_Service_API.API.Repositories.Notification
{
    public interface ISMS_Service
    {
        Task SendSMSNotification(string MobileNumber, string Message);
        Task SendSMSNotification_Test(string MobileNumber, string Message);
    }
}
