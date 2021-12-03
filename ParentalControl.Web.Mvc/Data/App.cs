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
    
    public partial class App
    {
        public int AppId { get; set; }
        public Nullable<int> DevicePhoneId { get; set; }
        public int InfantAccountId { get; set; }
        public Nullable<int> ScheduleId { get; set; }
        public Nullable<int> DevicePCId { get; set; }
        public string AppName { get; set; }
        public Nullable<bool> AppStatus { get; set; }
        public Nullable<bool> AppAccessPermission { get; set; }
        public System.DateTime AppCreationDate { get; set; }
    
        public virtual DevicePC DevicePC { get; set; }
        public virtual DevicePhone DevicePhone { get; set; }
        public virtual InfantAccount InfantAccount { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}