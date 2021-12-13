using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class WebConfigurationModel
    {
        public int WebConfigurationId { get; set; }
        public bool? WebConfigurationAccess { get; set; }
        public string CategoryName { get; set; }
        public int InfantAccountId { get; set; }
    }
}