using System;

namespace EPS_Service_API.Model
{
    //public class CreditTransactionModel
    //{
    //    public Guid TransactionID { get; set; }
    //    public decimal Amount { get; set; }
    //    public string AccountNo { get; set; }
    //    public string TransactionType { get; set; }
    //    public string Hash { get; set; }
    //    public DateTime TransactionDate { get; set; }
    //}


    public class CommitTransactionModel
    {
        public int TransferId { get; set; }
        public int FromAccountId { get; set; }
        public int FromBankId { get; set; }
        public int ToAccountId { get; set; }
        public int ToBankId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal VatAmount { get; set; }
        public string Comments { get; set; }


        public string TransactionID { get; set; }
        public string DebitTransactionID { get; set; }
        public int OTP { get; set; }
        public int OTPReferenceID { get; set; }


        public bool IsSuccess { get; set; }
    }


    public class oBjBankValidateCredit
    {
        public string TransactionID { get; set; }
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object Bankresult { set; get; }
    }

    public class CreditTransactionModel
    {
        public Guid TransactionID { get; set; }
        public int TransferId { get; set; }
        public decimal Amount { get; set; }
        public string AccountNo { get; set; }
        public string TransactionType { get; set; }
        public string Hash { get; set; }
        public DateTime TransactionDate { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }

        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object Bankresult { set; get; }



        public int FromBankId { get; set; }

        public int ToBankId { get; set; }
        public string AccountNumber { get; set; }

        public decimal ChargeAmount { get; set; }
        public decimal VatAmount { get; set; }
        public string Comments { get; set; }

        public bool IsSuccess { get; set; }
    }



    public class oBjBankValidateCnD
    {
        public string TransferId { get; set; }
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object Bankresult { set; get; }
    }

}