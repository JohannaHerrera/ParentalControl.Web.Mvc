//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParentalControl.Web.Mvc.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class WebConfiguration
    {
        public int WebConfigurationId { get; set; }
        public Nullable<bool> WebConfigurationAccess { get; set; }
        public int CategoryId { get; set; }
        public int InfantAccountId { get; set; }
    
        public virtual InfantAccount InfantAccount { get; set; }
        public virtual WebCategory WebCategory { get; set; }
    }
}