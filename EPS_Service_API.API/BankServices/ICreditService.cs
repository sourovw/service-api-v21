using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Model;

namespace EPS_Service_API.API.BankServices
{
    public interface ICreditService
    {
        Task<CreditTransactionModel> ValidateCredit(CreditTransactionModel inputModel);
    }
}
