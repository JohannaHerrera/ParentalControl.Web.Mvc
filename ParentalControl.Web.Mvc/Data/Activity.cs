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
    
    public partial class Activity
    {
        public int ActivityId { get; set; }
        public int InfantAccountId { get; set; }
        public string ActivityObject { get; set; }
        public string ActivityDescription { get; set; }
        public System.DateTime ActivityCreationDate { get; set; }
        public int ActivityTimesAccess { get; set; }
    
        public virtual InfantAccount InfantAccount { get; set; }
    }
}