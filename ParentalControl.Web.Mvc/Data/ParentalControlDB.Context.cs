﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ParentalControlDBEntities : DbContext
    {
        public ParentalControlDBEntities()
            : base("name=ParentalControlDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<App> App { get; set; }
        public virtual DbSet<DevicePC> DevicePC { get; set; }
        public virtual DbSet<DevicePhone> DevicePhone { get; set; }
        public virtual DbSet<DevicePhoneUse> DevicePhoneUse { get; set; }
        public virtual DbSet<DeviceUse> DeviceUse { get; set; }
        public virtual DbSet<InfantAccount> InfantAccount { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Parent> Parent { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<RequestType> RequestType { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<WebCategory> WebCategory { get; set; }
        public virtual DbSet<WebConfiguration> WebConfiguration { get; set; }
        public virtual DbSet<WindowsAccount> WindowsAccount { get; set; }
    }
}