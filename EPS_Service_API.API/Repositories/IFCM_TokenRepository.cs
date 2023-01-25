using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EPS_Service_API.Model.FCM_TokenModel;

namespace EPS_Service_API.Repositories
{
    public interface IFCM_TokenRepository
    {
        Task<FCM_Token> FCM_DeviceToken_Get(FCM_TokenGet _FCMGet);
        Task<int> FCM_DeviceToken_Set(FCM_Token _FCMSet);
    }
}
