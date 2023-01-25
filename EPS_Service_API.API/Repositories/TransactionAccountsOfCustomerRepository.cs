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
    public class TransactionAccountsOfCustomerRepository: ITransactionAccountsOfCustomerRepository
    {
        private DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;

        public TransactionAccountsOfCustomerRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<List<TxnAcc>> AccountList_GetByCusID(int customerID)
        {
            return (await _dataAccessHelper.QueryData<TxnAcc, dynamic>("AccountList_GetByCusID", new { CusID = customerID })).ToList();
        }



        public async Task<string> AccountsActiveInactive_byCustomer(AccountStatus_UpdateModel ASU)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id_out", DbType.String, direction: ParameterDirection.Output);
            p.Add("CustomerID_in", ASU.CustomerId);
            p.Add("AccountID_in", ASU.AccId);
            p.Add("CurrentStatus_in", ASU.CurrentStatus);


            await _dataAccessHelper.ExecuteData("AccountsActiveInactive_byCustomer", p);
            int Op_status= p.Get<int>("Id_out");
            if (Op_status == 1)
            {
                return ("Disabled");
            }

            else if(Op_status == 2)
            {
                return ("Enabled");
            }

            else
            {
                return ("Failed");
            }

            
        }



        public async Task<List<TxnAccStat>> AccountStatus_AccountList_Get(int customerID)
        {
            return (await _dataAccessHelper.QueryData<TxnAccStat, dynamic>("AccountStatus_AccountList_Get", new { CustomerID_in = customerID })).ToList();
        }


        public async Task<string> AccountsDefaultSelection_byCustomer(AccountDefault_UpdateModel ASU)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id_out", DbType.String, direction: ParameterDirection.Output);
            p.Add("CustomerID_in", ASU.CustomerId);
            p.Add("AccountID_in", ASU.AccId);
            p.Add("CurrentStatus_in", ASU.CurrentStatus);


            await _dataAccessHelper.ExecuteData("AccountsDefaultSelection_byCustomer", p);
            int Op_status = p.Get<int>("Id_out");
            if (Op_status == 1)
            {
                return ("Default");
            }

            else if (Op_status == 2)
            {
                return ("RemovedDefault");
            }

            else
            {
                return ("Failed");
            }


        }




    }
}
