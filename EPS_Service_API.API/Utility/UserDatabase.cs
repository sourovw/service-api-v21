using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Linq;

namespace EPS_Service_API.Utility
{
    public class UserDatabase
    {
        private List<UserInfoModel> Users;

        public UserDatabase()
        {
            Users = new List<UserInfoModel>
            {
                new UserInfoModel {UserName="sharif", Password="123456", Name="Sharif Ahmed", Email ="sharif2kb@yahoo.com", Role="Administrator"},
                new UserInfoModel {UserName="shimul", Password="123456", Name="Shariful Islam", Email ="shimul@yahoo.com", Role="Moderator"},
                new UserInfoModel {UserName="anis", Password="123456", Name="Anisur Rahman", Email ="anis@yahoo.com", Role="Operator"},
                new UserInfoModel {UserName="01812399996", Password="23567", Name="Sadid Islam", Email ="sadid@gmail.com", Role="MobileUser"}
            };
        }

        public UserInfoModel Authenticate(UserLoginModel userLogin)
        {
            var currentUser = Users.FirstOrDefault(u => u.UserName == userLogin.UserName && u.Password == userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}