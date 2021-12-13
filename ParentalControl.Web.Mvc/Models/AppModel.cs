using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class AppModel
    {
        public int AppId { get; set; }
        public int InfantAccountId { get; set; }
        public int? ScheduleId { get; set; }
        public string AppName { get; set; }
        public bool? AppAccessPermission { get; set; }
        public string DeviceName { get; set; }
    }
}