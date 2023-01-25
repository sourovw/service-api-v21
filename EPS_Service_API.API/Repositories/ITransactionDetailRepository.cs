using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Model;

namespace EPS_Service_API.API.Repositories
{
    public interface ITransactionDetailRepository
    {
        Task<List<SendMoneyList>> Transaction_SendMoneyList_GetByID(GetTxnDetailListEntity GTDLE);
        Task<List<ReceiveMoneyList>> Transaction_ReceiveMoneyList_GetByID(GetTxnDetailListEntity GTDLE);
        Task<List<TxnStatList>> TransactionStatusListGet_byCusID(GetTxnDetailListEntity GTDLE);
        Task<TxnStat> TransactionDetailGet_byTxnID(int TxnID_in);



        Task<Customeraccount> GetDetailsByToAccountId(int ToAccountId);
        Task<Customeraccount> GetDetailsByFromAccountId(int FromAccountId);


        Task<List<UsedAccounts>> Transaction_PreviousProfile_GetByID(GetTxnDetailListEntity GTDLE);


        Task<int> PG_PaymentTransactionPurchase_Set(PG_PaymentTransactionUser model);

    }
}
