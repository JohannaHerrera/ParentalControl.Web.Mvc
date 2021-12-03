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
    
    public partial class Request
    {
        public int RequestId { get; set; }
        public int RequestTypeId { get; set; }
        public int InfantAccountId { get; set; }
        public string RequestObject { get; set; }
        public Nullable<decimal> RequestTime { get; set; }
        public int RequestState { get; set; }
        public System.DateTime RequestCreationDate { get; set; }
        public int ParentId { get; set; }
    
        public virtual InfantAccount InfantAccount { get; set; }
        public virtual Parent Parent { get; set; }
        public virtual RequestType RequestType { get; set; }
    }
}