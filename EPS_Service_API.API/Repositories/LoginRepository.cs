using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Repositories;

namespace EPS_Service_API.API.Repositories
{
    public class LoginRepository: ILoginRepository
    {
        private DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;

        public LoginRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<UserProfile> Login_byMobileNo(string MobileNo,string Passwordin)
        {
            return (await _dataAccessHelper.QueryData<UserProfile, dynamic>("Login_byUserID", new { PhoneNumber_in = MobileNo, Password_in = Passwordin })).FirstOrDefault();
        }


        public async Task<RefreshToken> EPS_Service_Check_Refresh_Token(string _UserID)
        {
            return (await _dataAccessHelper.QueryData<RefreshToken, dynamic>("EPS_Service_Check_Refresh_Token", new { UserID_in = _UserID })).FirstOrDefault();
        }


        public async Task<int> EPS_Service_Revoke_Refresh_Token(int _TokenID_in)
        {
            await _dataAccessHelper.ExecuteData("EPS_Service_Revoke_Refresh_Token", new { TokenID_in = _TokenID_in });        
            return 1;
        }

        public async Task<RefreshToken_2> EPS_Service_Check_Refresh_Token_byToken(string _Token_in)
        {
            return (await _dataAccessHelper.QueryData<RefreshToken_2, dynamic>("EPS_Service_Check_Refresh_Token_byToken", new { Token_in = _Token_in })).FirstOrDefault();
        }



    }
}
