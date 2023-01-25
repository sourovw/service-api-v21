using System.Collections.Generic;

namespace EPS_Service_API.Model
{
    public class RechargeOfferModel
    {
        public List<ProcessRechargeOfferModel> AvailableOffers { get; set; }
    }

    public class RechargeOfferModel2
    {
        public List<ProcessRechargeOfferModel> VoiceRechargeOffers { get; set; }
        public List<ProcessRechargeOfferModel> InternetRechargeOffers { get; set; }
        public List<ProcessRechargeOfferModel> BundleRechargeOffers { get; set; }
    }

    public class ProcessRechargeOfferModel
    {
        public int OperatorId { get; set; }
        public int OperatorTypeId { get; set; }
        public int OfferTypeId { get; set; }
        public string Operator { get; set; }
        public string OperatorType { get; set; }
        public string OfferName { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
    }
}
