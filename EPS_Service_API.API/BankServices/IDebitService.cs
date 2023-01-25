using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EPS_Service_API.API.BankServices
{
    public interface IDebitService
    {
        Task<CreditTransactionModel> ValidateDebit(CreditTransactionModel inputModel);



    }
}