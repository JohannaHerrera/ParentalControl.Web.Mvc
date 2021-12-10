using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class DevicePhoneModel
    {
        public int DevicePhoneId { get; set; }
        public string DevicePhoneName { get; set; }
        public string DevicePhoneCode { get; set; }
        public int ParentId { get; set; }
        public int? InfantAccountId { get; set; }
        public bool deviceProtected { get; set; }
    }
}