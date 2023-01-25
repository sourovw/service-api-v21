using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EPS_Service_API.Model
{
    public class TransactionDetailModel
    {

    }

    public class objTransactionDetail
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object SendMoneyList { set; get; }
        public object ReceiveMoneyList { set; get; }

    }


    public class objTransactionDetail_Send
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object SendMoneyList { set; get; }

    }

    public class objTransactionDetail_Received
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object ReceiveMoneyList { set; get; }

    }

    public class SendMoneyList
    {
        public int TransfarID { get; set; }
        public string TransfarDate { get; set; }
        public decimal Amount { get; set; }
        public string ToBank { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string TransferTypeID { get; set; }

        public string Send_Rec_Type { get; set; }

        public SendMoneyList()
        {
            Send_Rec_Type = "Send";
        }

        }

    public class ReceiveMoneyList
    {
        public int TransfarID { get; set; }
        public string TransfarDate { get; set; }
        public decimal Amount { get; set; }
        public string ToBank { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string TransferTypeID { get; set; }

        public string Send_Rec_Type { get; set; }

        public ReceiveMoneyList()
        {
            Send_Rec_Type = "Rec";
        }
    }


    public class objTxnStatList
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object TxnStatList { set; get; }
      

    }

    public class TxnStatList
    {
        public int TxnId { get; set; }
        public string TxnDate { get; set; }
        public decimal TxnAmount { get; set; }
        public string TxnRef { get; set; }
        public string TxnComment { get; set; }
        public string TxnCurrentStatus { get; set; }
        public string BankName { get; set; }
        public bool CustomerAck { get; set; }
    }

    public class objTxnStat
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object TxnStat { set; get; }


    }


    public class TxnStat
    {
        public int TransferID { get; set; }
        public string DrAcNo { get; set; }

        public string TransfarDate { get; set; }
        public string TransfarTime { get; set; }


        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string FromBankID { get; set; }
        public string FromBankName { get; set; }
        public string ToBankID { get; set; }
        public string ToBankName { get; set; }


        public string DrAcBank { get; set; }
        public decimal Amount { get; set; }
        public decimal SendingCharge { get; set; }
        public decimal Vat { get; set; }
        public decimal TotalAmount { get; set; }
        public string CrAcNo { get; set; }
        public string CrAcName { get; set; }
        public bool ConfirmationType { get; set; }
        public string CrAcBank { get; set; }
        public string Remark { get; set; }
        public string Reference { get; set; }
        public string TxnStatus { get; set; }
        public string DebitConsentUrl { get; set; }
        public string MobileNo { get; set; }
        public string MobileNoUsed { get; set; }
    }


    public class GetTxnDetailListEntity
    {
        public int CusId { get; set; }

        // API Related Information 
        public string DeviceID { get; set; }

        public string p_from_date { get; set; }
        public string p_to_date { get; set; }

        // API Related Information 

    }

    public class GetTxnEntity
    {
        public int TxnId { get; set; }

        // API Related Information 
        public string DeviceID { get; set; }

        // API Related Information 

    }


    public class objHistoryContact
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object UsedAccounts { set; get; }


    }

    public class UsedAccounts
    {
        public string CusId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }

        public string AccountName { get; set; }
        public string AccountNumber { get; set; }

        public string ProfileImage { get; set; }
    }



    public class PG_PaymentTransactionUser
    {
    public string PGI_TxnID { get; set; }
    public decimal Amount{ get; set; }
    public int TransactionType{ get; set; }
    public string PhoneNumber{ get; set; }
    public int MobileOperatorId{ get; set; }
    public int BillCategoryId{ get; set; }
    public int BillId{ get; set; }
    public string BillNumber { get; set; }
    public string BillPeriod { get; set; }
    public string BillType { get; set; }
    public string MeterNumber { get; set; }
    public string BillerAccountNumber{ get; set; }
    public string BillerMobileNumber{ get; set; }
    public string ReferenceId { get; set; }
    public int Status{ get; set; }
    public int UserId{ get; set; }
    }

    public class objPG_PaymentTransactionUser
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
    }








}
