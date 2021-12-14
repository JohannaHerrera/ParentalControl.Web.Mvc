using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParentalControl.Web.Mvc.Models
{
    public class WindowsAccountModel
    {
        public int WindowsAccountId { get; set; }
        public string WindowsAccountName { get; set; }
        public int DevicePCId { get; set; }
        public int? InfantAccountId { get; set; }
    }
}