using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPS_Service_API.Repositories
{
    public interface IBankRepository
    {
        Task<List<BankList>> GetBankByCusId(int userID);
        Task<List<To_BankList>> SA_AllBank_ToSend_Get();
        Task<List<To_Bank_Branch_Routing>> SA_Bank_Routing_ToSend_GetbyBankID(int _BankID);

        Task<List<To_MobileNumber_List>> BA_CustomeraccountByMobileNumber(string _MobileNumber);

        Task<List<MobileRechargeEntity>> EPS_Service_MobileRechargeEntity_Get();
    }
}