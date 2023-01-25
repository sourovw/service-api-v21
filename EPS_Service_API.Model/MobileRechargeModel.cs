using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Model
{
    public class MobileRechargeModel
    {

    }

    public class MobileRechargeInitiate
    {
        public int Amount { get; set; }
        public string MobileNumber { get; set; }
        public string Name { get; set; }

        // API Related Information 
        public string DeviceID { get; set; }

        // API Related Information 

    }

    public class objMobileRechargeInitiate
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public string Operator { get; set; }

        public object MobileRechargeEntity { set; get; }
        public object MobileRechargeChargeInfo { set; get; }
        public object MobileRechargeBenInfo { set; get; }
    }


    public class MobileRechargeEntity
    {
        public string BankEntity { get; set; }
        public string MFSEntity { get; set; }

    }

    public class MobileRechargeChargeInfo
    {
        public int Amount { get; set; }
        public int Charge { get; set; }
        public int Total { get; set; }

    }

    public class MobileRechargeBenInfo
    {
        public string MobileNumber { get; set; }
        public string Name { get; set; }
    }


    public class MobileRechargeVerify
    {
        public int Amount { get; set; }
        public int Charge { get; set; }
        public int Total { get; set; }


        public string MobileNumber { get; set; }
        public string Name { get; set; }

        public string SIM_Type { get; set; }
        public string Operator { get; set; }

        public string FinancialEntityID { get; set; }
        public string FinancialEntityName { get; set; }


        // API Related Information 
        public string DeviceID { get; set; }

        // API Related Information 

    }


    public class objMobileRechargeVerify
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object MobileRechargeVerify { set; get; }

    }





    public class MobileRechargeConfirm
    {
        public int Amount { get; set; }
        public int Charge { get; set; }
        public int Total { get; set; }


        public string MobileNumber { get; set; }
        public string Name { get; set; }

        public string SIM_Type { get; set; }
        public string Operator { get; set; }

        public string FinancialEntityID { get; set; }
        public string FinancialEntityName { get; set; }

        public string OTP { get; set; }


        // API Related Information 
        public string DeviceID { get; set; }

        // API Related Information 

    }


    public class objMobileRechargeConfirm
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public string Message { get; set; }

    }





}
