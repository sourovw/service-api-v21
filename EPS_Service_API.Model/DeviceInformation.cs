using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EPS_Service_API.Model
{
    public class DeviceInformation
    {
        [Required(ErrorMessage = "Please enter 'DeviceTypeID'.")]
        public string DeviceTypeID { get; set; }
        [Required(ErrorMessage = "Please enter 'DeviceModel'.")]
        public string DeviceModel { get; set; }
        [Required(ErrorMessage = "Please enter 'DeviceOS'.")]
        public string DeviceOS { get; set; }
        [Required(ErrorMessage = "Please enter 'DeviceDetails'.")]
        public string DeviceDetails { get; set; }
        [Required(ErrorMessage = "Please enter 'LocationLattitude'.")]
        public string LocationLattitude { get; set; }
        [Required(ErrorMessage = "Please enter 'LocationLongitude'.")]
        public string LocationLongitude { get; set; }
        [Required(ErrorMessage = "Please enter 'IP_Address'.")]
        public string IP_Address { get; set; }
        [Required(ErrorMessage = "Please enter 'BrowserDetails'.")]
        public string BrowserDetails { get; set; }
        public string ActionMethod { get; set; }
        public string ActionID { get; set; }

    }
}
