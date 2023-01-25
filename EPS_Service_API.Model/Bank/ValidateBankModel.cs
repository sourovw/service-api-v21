using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Model
{
    public class ValidateBankModel
    {
        public int TransferId { get; set; }
        public int FromAccountId { get; set; }
        public int FromBankId { get; set; }
        public int ToAccountId { get; set; }
        public int ToBankId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal VatAmount { get; set; }
        public string Comments { get; set; }

        public bool IsSuccess { get; set; }
        public string OTPValue { get; set; }
    }
}
