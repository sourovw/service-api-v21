using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Model.Nagad
{
    public class NagadPayment
    {
        public string amount { get; set; }


    }

    public class objPaymentNagad
    {
        public string status { get; set; }
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }


    }

    public class objPaymentStatus
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object PaymentStatus { set; get; }
    }


    public class PaymentStatus
    {
        public string merchantId { get; set; }
        public string orderId { get; set; }
        public string paymentRefId { get; set; }
        public string amount { get; set; }
        public string clientMobileNo { get; set; }
        public string merchantMobileNo { get; set; }
        public string orderDateTime { get; set; }
        public string issuerPaymentDateTime { get; set; }
        public string issuerPaymentRefNo { get; set; }
        public string additionalMerchantInfo { get; set; }
        public string status { get; set; }
        public string statusCode { get; set; }
        public string message { get; set; }
        public string reason { get; set; }
    }

    public class PaymentStatusCapture
    {
        public string merchant { get; set; }
        public string order_id { get; set; }
        public string payment_ref_id { get; set; }
        public string status { get; set; }
        public string status_code { get; set; }
        public string message { get; set; }
        public string payment_dt { get; set; }
        public string issuer_payment_ref { get; set; }
    }

}
