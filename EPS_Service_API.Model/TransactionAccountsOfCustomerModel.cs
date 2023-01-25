using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EPS_Service_API.Model
{

    public class objTxnAcc
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object TxnAcc { set; get; }

    }


    public class TxnAcc
    {
        public int AccId { get; set; }
        public int CustomerId { get; set; }
        public string FinancialEntityImg { get; set; }
        public string FinancialEntityName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountStatus { get; set; }
        public string AppliedDate { get; set; }
        public bool? isDefaultCreditAccount { get; set; }
        public string AccountType { get; set; }

        public string Code { get; set; }
        // New 
        public bool? IsActive { get; set; }


        public string FinancialEntityID { get; set; }
        public string AccountName { get; set; }
        public string BranchName { get; set; }
        public string RoutingNumber { get; set; }
    }


    public class AccountStatus_UpdateModel
    {
        [Required(ErrorMessage = "Please Provide 'Customer Id'.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Please Provide 'Account Id'.")]
        public int AccId { get; set; }
        [Required(ErrorMessage = "Please Provide 'The Account Active [True]or[False]'.")]
        public bool isActive { get; set; }
        public int CurrentStatus { get; set; }

        // API Related Information 
        public string DeviceID { get; set; }

        // API Related Information 
    }

    public class AccountDefault_UpdateModel
    {
        [Required(ErrorMessage = "Please Provide 'Customer Id'.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Please Provide 'Account ID'.")]
        public int AccId { get; set; }
        [Required(ErrorMessage = "Please Provide 'The Account Active [True]or[False]'.")]
        public bool isActive { get; set; }
        [Required(ErrorMessage = "Please Provide 'The Account Default of Not [True]or[False]'.")]
        public bool isDefaultCreditAccount { get; set; }
        public int CurrentStatus { get; set; }

        // API Related Information 
        public string DeviceID { get; set; }

        // API Related Information 
    }


    public class objAccStat
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object TxnAccStat { set; get; }

    }

    public class TxnAccStat
    {
        public string AccountID { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountStatus { get; set; }
        public string BankName { get; set; }
        public string BankLogo { get; set; } 
        public string ApplicationDate { get; set; }
        public bool IsActive { get; set; }
        public bool isDefaultCreditAccount { get; set; }

        public string Code { get; set; }

        public string AccountType { get; set; }


    }


    public class GetAccountDetailEntity
    {
        public int CusId { get; set; }

        // API Related Information 
        public string DeviceID { get; set; }

        // API Related Information 

    }








}
