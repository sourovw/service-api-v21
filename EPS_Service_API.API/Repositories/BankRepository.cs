using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EPS_Service_API.Repositories
{
    public class BankRepository : IBankRepository
    {
        private DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;

        public BankRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        #region "DataAccessHelper Methods"

        public async Task<List<BankList>> GetBankByCusId(int userID)
        {
            //DynamicParameters p = new DynamicParameters();
            //p.Add("CusID_in", userID);

            //p.Add("Cus_Name", GFI.DeviceTypeID);
            //p.Add("Cus_Name", GFI.DeviceTypeID);
            //p.Add("Cus_Name", GFI.DeviceTypeID);
            //p.Add("Cus_Name", GFI.DeviceTypeID);
            //p.Add("Cus_Name", GFI.DeviceTypeID);
            //p.Add("Cus_Name", GFI.DeviceTypeID);
            // p.Add("Cus_Name", GFI.DeviceTypeID);
            //p.Add("Cus_Name", GFI.DeviceTypeID);

            return (await _dataAccessHelper.QueryData<BankList, dynamic>("BankListForAddNewAccount_GetByCusID", new { CustomerID = userID })).ToList();
        }


        #endregion


        #region Bank Routing Modiule

        public async Task<List<To_BankList>> SA_AllBank_ToSend_Get()
        {

            return (await _dataAccessHelper.QueryData<To_BankList, dynamic>("SA_AllBank_ToSend_Get", new {  })).ToList();
        }

        public async Task<List<To_Bank_Branch_Routing>> SA_Bank_Routing_ToSend_GetbyBankID(int _BankID)
        {

            return (await _dataAccessHelper.QueryData<To_Bank_Branch_Routing, dynamic>("SA_Bank_Routing_ToSend_GetbyBankID", new { _BankID = _BankID })).ToList();
        }


        public async Task<List<To_MobileNumber_List>> BA_CustomeraccountByMobileNumber(string _MobileNumber)
        {

            return (await _dataAccessHelper.QueryData<To_MobileNumber_List, dynamic>("BA_CustomeraccountByMobileNumber", new { MobileNumber = _MobileNumber })).ToList(); 
        }


        public async Task<List<MobileRechargeEntity>> EPS_Service_MobileRechargeEntity_Get()
        {

            return (await _dataAccessHelper.QueryData<MobileRechargeEntity, dynamic>("EPS_Service_MobileRechargeEntity_Get", new {  })).ToList();
        }






        #endregion 
    }
}
