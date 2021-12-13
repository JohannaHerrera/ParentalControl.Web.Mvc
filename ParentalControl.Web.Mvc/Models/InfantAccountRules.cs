using ParentalControl.Web.Mvc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class InfantAccountRules
    {
        public int InfantAccountId { get; set; }
        public string InfantName { get; set; }
        public List<AppModel> AppList { get; set; }
        public List<WebConfigurationModel> WebConfigurationList { get; set; }
        public List<DeviceUse> DeviceUseList { get; set; }
        public List<Activity> ActivityList { get; set; }
    }
}