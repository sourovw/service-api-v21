using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static EPS_Service_API.Model.FCM_TokenModel;

namespace EPS_Service_API.Repositories
{
    public class FCM_TokenRepository: IFCM_TokenRepository
    {
        private DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;

        public FCM_TokenRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }


        public async Task<FCM_Token> FCM_DeviceToken_Get(FCM_TokenGet _FCMGet)
        {
            return (await _dataAccessHelper.QueryData<FCM_Token, dynamic>("FCM_DeviceToken_Get", new { In_UserID= _FCMGet.UserID, In_MobileNumber= _FCMGet .MobileNumber})).FirstOrDefault();
        }

        public async Task<int> FCM_DeviceToken_Set(FCM_Token _FCMSet)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("In_UserID", _FCMSet.UserID);
            p.Add("In_DeviceToken", _FCMSet.DeviceToken);

            await _dataAccessHelper.ExecuteData("FCM_DeviceToken_Set", p);
            return p.Get<int>("Id");
        }







    }
}
