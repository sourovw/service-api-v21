using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Utility
{
    public class HashCreatorValidator
    {
        private readonly IConfiguration _config;

        public HashCreatorValidator(IConfiguration config)
        {
            this._config = config;
        }


        public string GenerateHash(string payload = "Default Payload", string hashkey = "Default Hash Key")
        {
            //  using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_config["Hash:HashKey"])))
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hashkey)))
            {
                byte[] data = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return Convert.ToBase64String(data);
            }
        }



        public bool IsValidHash(string senderHash, string payLoad = "Default Payload", string hashkey = "Default Hash Key")
        {
            var generatedHash = GenerateHash(payLoad, hashkey);
            return (senderHash == generatedHash);
        }



    }
}
