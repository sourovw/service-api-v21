using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Repositories;

namespace EPS_Service_API.API.Repositories
{
    public class TransactionDetailRepository: ITransactionDetailRepository
    {
        private DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;

        public TransactionDetailRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }


        public async Task<List<SendMoneyList>> Transaction_SendMoneyList_GetByID(GetTxnDetailListEntity GTDLE)
        {
            return (await _dataAccessHelper.QueryData<SendMoneyList, dynamic>("Transaction_SendMoneyList_GetByID", new { CustomerID_in = GTDLE.CusId, p_from_date = GTDLE.p_from_date, p_to_date = GTDLE.p_to_date })).ToList();
        }

        public async Task<List<ReceiveMoneyList>> Transaction_ReceiveMoneyList_GetByID(GetTxnDetailListEntity GTDLE)
        {
            return (await _dataAccessHelper.QueryData<ReceiveMoneyList, dynamic>("Transaction_ReceiveMoneyList_GetByID", new { CustomerID_in = GTDLE.CusId, p_from_date = GTDLE.p_from_date, p_to_date = GTDLE.p_to_date })).ToList();
        }

        public async Task<List<TxnStatList>> TransactionStatusListGet_byCusID(GetTxnDetailListEntity GTDLE)
        {
            return (await _dataAccessHelper.QueryData<TxnStatList, dynamic>("TransactionStatus_TransactionList_byCusID", new { CustomerID_in = GTDLE.CusId, p_from_date = GTDLE.p_from_date, p_to_date = GTDLE.p_to_date })).ToList();
        }

        public async Task<TxnStat> TransactionDetailGet_byTxnID(int TxnID_in)
        {
            return (await _dataAccessHelper.QueryData<TxnStat, dynamic>("TransactionStatus_TransactionDetail_byTxnID", new { TxnID_in= TxnID_in })).FirstOrDefault();
        }


        public async Task<Customeraccount> GetDetailsByToAccountId(int ToAccountId)
        {
            var data = (await _dataAccessHelper.QueryData<Customeraccount, dynamic>("BA_CustomeraccountByAccId", new { AccId = ToAccountId })).ToList();
            return data[0];
        }

        public async Task<Customeraccount> GetDetailsByFromAccountId(int FromAccountId)
        {
            var data = (await _dataAccessHelper.QueryData<Customeraccount, dynamic>("BA_CustomeraccountByAccId", new { AccId = FromAccountId })).ToList();
            return data[0];
        }


        public async Task<List<UsedAccounts>> Transaction_PreviousProfile_GetByID(GetTxnDetailListEntity GTDLE)
        {           
            return (await _dataAccessHelper.QueryData<UsedAccounts, dynamic>("Transaction_PreviousProfile_GetByID", new { CustomerID_in = GTDLE.CusId})).ToList();        
        }

        
        public async Task<int> PG_PaymentTransactionPurchase_Set(PG_PaymentTransactionUser model)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            
            
            p.Add("PGI_TxnID_in", model.PGI_TxnID);
            p.Add("Amount_in", model.Amount);
            p.Add("TransactionType_in", model.TransactionType);
            p.Add("PhoneNumber_in", model.PhoneNumber);
            p.Add("MobileOperatorId_in", model.MobileOperatorId);
            p.Add("BillCategoryId_in", model.BillCategoryId);
            p.Add("BillId_in", model.BillId);
            p.Add("BillNumber_in", model.BillNumber);
            p.Add("BillPeriod_in", model.BillPeriod);
            p.Add("BillType_in", model.BillType);
            p.Add("MeterNumber_in", model.MeterNumber);
            p.Add("BillerAccountNumber_in", model.BillerAccountNumber);
            p.Add("BillerMobileNumber_in", model.BillerMobileNumber);
            p.Add("ReferenceId_in", model.ReferenceId);
            p.Add("Status_in", model.Status);
            p.Add("UserId_in", model.UserId);


            await _dataAccessHelper.ExecuteData("USP_PG_PaymentTransactionPurchase_Set", p);
            return p.Get<int>("Id");
        }


    }
}
