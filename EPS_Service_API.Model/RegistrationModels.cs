using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Model
{
    public class RegistrationModels
    {
    }

    public class RegisterViewModel: BasicApiCallingModel
    {
        public bool ShowErrorSummary { get; set; }
        public string PhoneNo { get; set; }
        public string PIN { get; set; }
        public string ConfirmPIN { get; set; }
    }

    public class ObjRegistration
    {
        public int StatusCode { get; set; }        
        public string MobileNumber { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public string Message { get; set; }

    }


    public class ObjOTP_Verification
    {
        public int StatusCode { get; set; }
        public string MobileNumber { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public string Message { get; set; }

    }

    public class ObjPasswordResetManual
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
    }

    public class PasswordResetManual
    {
        public string MobileNumber { get; set; }
    }


    public class OTP_VerificationModel: BasicApiCallingModel
    {
        public string MobileNumber { get; set; }
        public string OTP { get; set; }
    }

    public class PIN_ChangeModel: BasicApiCallingModel
    {
        public string MobileNumber { get; set; }
        public string Old_PIN { get; set; }
        public string New_PIN { get; set; }
    }


    public class PIN_Reset_OTP_Model : BasicApiCallingModel
    {
        public string SecretKey { get; set; }
        public string OTP { get; set; }
    }


    public class ObjPasswordReset
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public string Message { get; set; }

        public string SecretKey { get; set; }

    }

}
