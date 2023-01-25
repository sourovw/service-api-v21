using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Model
{
    public class FCM_TokenModel
    {
        public class FCM_TokenGet
        {
            public string MobileNumber { get; set; }
            public int UserID { get; set; }
        }
        public class FCM_Token
        {
            public int UserID { get; set; }
            public string DeviceToken { get; set; }
        }

        public class objFCM_Token
        {
            public int StatusCode { get; set; }
            public string ErrorDescription { get; set; }
            public string APIVersion { get; set; }
            public object FCM_Token { set; get; }
        }

        public class objFCM_TokenSet
        {
            public int StatusCode { get; set; }
            public string ErrorDescription { get; set; }
            public string APIVersion { get; set; }
        }




    }
}
