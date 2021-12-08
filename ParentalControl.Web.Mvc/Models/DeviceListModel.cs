using ParentalControl.Web.Mvc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class DeviceListModel
    {
        public List<DevicePCModel> devicePCList { get; set; }
        public List<DevicePhoneModel> devicePhoneList { get; set; }
    }
}