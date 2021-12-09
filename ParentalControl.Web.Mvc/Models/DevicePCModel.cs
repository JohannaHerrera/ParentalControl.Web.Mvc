using ParentalControl.Web.Mvc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class DevicePCModel
    {
        public int DevicePCId { get; set; }
        public string DevicePCName { get; set; }
        public string DevicePCCode { get; set; }
        public int ParentId { get; set; }
        public List<WindowsAccountModel> windowsAccountList { get; set; }

    }
}