using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Model
{
    public class EPS_CommonModel
    {

    }

    public class HelpAndFAQ
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class Support
    {
        public string MobileNumber { get; set; }
        public string Email { get; set; }
    }

    public class TermsAndCondition
    {
        public string Headline { get; set; }
        public string Text { get; set; }
    }


    public class limitsettings
    {
        public string Daily_Limit { set; get; }
        public string Monthly_Limit { set; get; }
        public string Daily_Limit_Usage { set; get; }
        public string Monthly_Limit_Usage { set; get; }

    }

    public class Daily_Limit
    {
        public string SendInAmount { get; set; }
        public string SendInQuantity { get; set; }
        public string ReceivedAmount { get; set; }
        public string ReceivedQuantity { get; set; }
        public string MobileRechargeAmount { get; set; }
        public string MobileRechargeQuantity { get; set; }
        public string PayBillAmount { get; set; }
        public string PayBillQuantity { get; set; }

    }

    public class Monthly_Limit
    {
        public string SendInAmount { get; set; }
        public string SendInQuantity { get; set; }
        public string ReceivedAmount { get; set; }
        public string ReceivedQuantity { get; set; }
        public string MobileRechargeAmount { get; set; }
        public string MobileRechargeQuantity { get; set; }
        public string PayBillAmount { get; set; }
        public string PayBillQuantity { get; set; }

    }


    public class objlimitsettings
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object limitsettings { set; get; }

    }





    public class objTermsAndCondition
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object TermsAndCondition { set; get; }

    }


    public class objSupport
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object Support { set; get; }

    }

    public class objHelpAndFAQ
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object HelpAndFAQ { set; get; }

    }


    public class offersGetModel
    {
        public int operatorId { get; set; }
        public int operatorTypeId { get; set; }
    }


}
