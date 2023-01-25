using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Model
{
    public class BasicApiCallingModel
    {
        // API Related Information 
        public string DeviceTypeID { get; set; }
        public string DeviceID { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceOS { get; set; }
        public string DeviceDetails { get; set; }
        public string LocationLattitude { get; set; }
        public string LocationLongitude { get; set; }
        public string IP_Address { get; set; }

        public string BrowserDetails { get; set; }
        public string ActionMethod { get; set; }
        public string ActionID { get; set; }
        public string UserId { get; set; }
    // API Related Information 
}
}
