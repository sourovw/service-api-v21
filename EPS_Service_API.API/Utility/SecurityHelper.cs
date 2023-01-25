using EPS_Service_API.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

namespace EPS_Service_API.Utility
{
    public class SecurityHelper
    {
        private IConfiguration _config;

        public SecurityHelper(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJSONWebToken(UserInfoModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserName),
                new Claim(ClaimTypes.Name, userInfo.Name),
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(ClaimTypes.Role, userInfo.Role),
                new Claim(ClaimTypes.UserData, userInfo.DeviceID)
            };

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
               issuer: _config["JWT:Issuer"],
               audience: _config["JWT:Issuer"],
               claims,
               expires: DateTime.Now.AddMinutes(20),
               signingCredentials: credentials
               );

            var encodetToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
            return encodetToken;
        }

        public string GenerateHash(string payLoad)
        {
            byte[] data = Encoding.UTF8.GetBytes(payLoad + _config["SecretKey"]);
            return Convert.ToBase64String(data);
        }

        public bool IsValidHash(string senderHash, string payLoad)
        {
            if (senderHash != GenerateHash(payLoad))
                return false;

            return true;
        }
    }
}