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
    
    public partial class Schedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Schedule()
        {
            this.App = new HashSet<App>();
            this.DevicePhoneUse = new HashSet<DevicePhoneUse>();
            this.DeviceUse = new HashSet<DeviceUse>();
        }
    
        public int ScheduleId { get; set; }
        public System.DateTime ScheduleStartTime { get; set; }
        public System.DateTime ScheduleEndTime { get; set; }
        public System.DateTime ScheduleCreationDate { get; set; }
        public int ParentId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<App> App { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DevicePhoneUse> DevicePhoneUse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeviceUse> DeviceUse { get; set; }
        public virtual Parent Parent { get; set; }
    }
}
