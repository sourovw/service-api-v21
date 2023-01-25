using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPS_Service_API.API.Repositories
{
    public interface IRechargeOfferRepository
    {
        Task<object> GetRechargeOffers(int operatorId, int typeId);
    }
}
