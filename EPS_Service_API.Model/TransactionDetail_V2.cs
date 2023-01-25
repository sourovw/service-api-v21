using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Model
{
    public class TransactionDetail_V2
    {
    }

    public class ToAccountDetails
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IsActive { get; set; }
        public string IsDefault { get; set; }
        public string CustomerId { get; set; }
        public string FinancialEntityId { get; set; }
        public string QueueStatusId { get; set; }
        public string ServiceOperatorsId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string RoutingNumber { get; set; }
    }

    public class FromAccountDetails
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string IsActive { get; set; }
        public string IsDefault { get; set; }
        public string CustomerId { get; set; }
        public string FinancialEntityId { get; set; }
        public string QueueStatusId { get; set; }
        public string ServiceOperatorsId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string RoutingNumber { get; set; }
    }

    public class objTrxbByID
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }

        public string TransfarDate { get; set; }
        public string TransfarTime { get; set; }

        public string TransferId { get; set; }
        public string TransactionId { get; set; }
        public string Bankresult { get; set; }
        public string ToAccountId { get; set; }
        public string Amount { get; set; }
        public string ChargeAmount { get; set; }
        public string VatAmount { get; set; }


        public string TransactionNote { get; set; }
        public string TransactionReference { get; set; }

        public object ToAccountDetails { set; get; }
        public object FromAccountDetails { set; get; }
    }

    public partial class Customeraccount
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        // public int CurrencyTypeId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDefault { get; set; }
        public int CustomerId { get; set; }
        public int FinancialEntityId { get; set; }
        public int QueueStatusId { get; set; }
        // public int AccountType { get; set; }
        //   public DateTime FirstCreated { get; set; }
        //  public DateTime LastUpdated { get; set; }
        //public string RejectReason { get; set; }
        public string ServiceOperatorsId { get; set; }

        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string RoutingNumber { get; set; }

        //public virtual ICollection<Transfer> TransferFromAccount { get; set; }
        //public virtual ICollection<Transfer> TransferToAccount { get; set; }
    }

}
