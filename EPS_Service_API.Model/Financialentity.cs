using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPS_Service_API.Model
{


    public class objBankList
    {

        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object BankList { set; get; }
    }


    public class BankList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string LogoImage { get; set; }
    }

    public class GetFinancialentity
    {
        public int CusId { get; set; }

        // API Related Information 
        public string DeviceID { get; set; }

        // API Related Information 

    }


    public class To_BankList
    {
        public string ID { get; set; }
        public string BankName { get; set; }
    }


    public class To_Bank_Branch_Routing
    {
        public string ID { get; set; }
        public string BranchName { get; set; }
        public string RoutingNumber { get; set; }
        public string BranchCode { get; set; }
        public string DistrictCode { get; set; }

    }

    public class To_Bank_Branch_Routing_Arg
    {
        public int BankID { get; set; }
        public string DeviceID { get; set; }

    }



    public class objToBankList
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object To_BankList { set; get; }

    }

    public class objToBankRouting
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object To_Bank_Branch_Routing { set; get; }

    }


    public class To_MobileNumber_List
    {
        public string CustomerId { get; set; }
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IsActive { get; set; }
        public string IsDefault { get; set; }
        public string FinancialEntityId { get; set; }
        public string QueueStatusId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string RoutingNumber { get; set; }

    }


    public class objTo_MobileNumber_List
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object To_MobileNumber_List { set; get; }

    }

    public class To_MobileNumber_List_Arg
    {
        public string MobileNumber { get; set; }
        public string DeviceID { get; set; }

    }







}
