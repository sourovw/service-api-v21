using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Model;


namespace EPS_Service_API.API.Repositories
{
    public interface ITransactionAccountsOfCustomerRepository
    {
        Task<List<TxnAcc>> AccountList_GetByCusID(int customerID);
        Task<string> AccountsActiveInactive_byCustomer(AccountStatus_UpdateModel ASU);
        Task<List<TxnAccStat>> AccountStatus_AccountList_Get(int customerID);
        Task<string> AccountsDefaultSelection_byCustomer(AccountDefault_UpdateModel ASU);
    }
}
