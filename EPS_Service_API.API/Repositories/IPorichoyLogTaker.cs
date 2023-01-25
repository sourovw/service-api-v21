using EPS_Service_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPS_Service_API.API.Repositories
{
    public interface IPorichoyLogTaker
    {
        Task<int> Porichoy_API_Hit(PorichoyAPIHitLogModel PARM);
        Task<int> Porichoy_API_Response(PorichoyAPIResponseLogModel PARM);
    }
}
