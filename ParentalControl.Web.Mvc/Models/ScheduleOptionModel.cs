using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class ScheduleOptionModel
    {
        public int ScheduleId { get; set; }
        public DateTime ScheduleStartTime { get; set; }
        public DateTime ScheduleEndTime { get; set; }
        public string ScheduleTime { get; set; }
    }
}