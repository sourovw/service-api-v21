using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Model;


namespace EPS_Service_API.API.Repositories
{
    public interface ILoginRepository
    {
        Task<UserProfile> Login_byMobileNo(string MobileNo, string Passwordin);
        Task<RefreshToken> EPS_Service_Check_Refresh_Token(string _UserID);
        Task<int> EPS_Service_Revoke_Refresh_Token(int _TokenID_in);
        Task<RefreshToken_2> EPS_Service_Check_Refresh_Token_byToken(string _Token_in);
    }
}
