using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.Filters;
using ParentalControl.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParentalControl.Web.Mvc.Controllers
{    
    public class DeviceController : BaseController
    {
        // GET: Device
        [AuthorizeParent]
        public ActionResult Index()
        {
            try
            {
                List<DevicePCModel> devicePCList = new List<DevicePCModel>();
                List<DevicePhoneModel> devicePhoneList = new List<DevicePhoneModel>();
                DeviceListModel deviceListModel = new DeviceListModel();
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    devicePCList = (from devicePC in db.DevicePC
                                    where devicePC.ParentId == parent.Id
                                    select new DevicePCModel
                                    {
                                        DevicePCId = devicePC.DevicePCId,
                                        DevicePCName = devicePC.DevicePCName,
                                        DevicePCCode = devicePC.DevicePCCode,
                                        ParentId = devicePC.ParentId
                                    }).ToList();

                    devicePhoneList = (from devicePhone in db.DevicePhone
                                    where devicePhone.ParentId == parent.Id
                                    select new DevicePhoneModel
                                    {
                                        DevicePhoneId = devicePhone.DevicePhoneId,
                                        DevicePhoneName = devicePhone.DevicePhoneName,
                                        DevicePhoneCode = devicePhone.DevicePhoneCode,
                                        ParentId = devicePhone.ParentId,
                                        InfantAccountId = devicePhone.InfantAccountId
                                    }).ToList();

                    deviceListModel.devicePCList = devicePCList;
                    deviceListModel.devicePhoneList = devicePhoneList;
                }

                return View(deviceListModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        public ActionResult DevicePcDetails(int deviceId)
        {
            try
            {
                DevicePCModel devicePCModel = new DevicePCModel();
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    devicePCModel = (from devicePC in db.DevicePC
                                     where devicePC.DevicePCId == deviceId
                                     select new DevicePCModel
                                    {
                                        DevicePCId = devicePC.DevicePCId,
                                        DevicePCName = devicePC.DevicePCName,
                                        DevicePCCode = devicePC.DevicePCCode,
                                        ParentId = devicePC.ParentId
                                    }).FirstOrDefault();

                    if (devicePCModel != null)
                    {
                        var infantAccounts = (from infantAccount in db.InfantAccount
                                              where infantAccount.ParentId == parent.Id
                                              select infantAccount).ToList();

                        var windowsAccounts = (from windowsAccount in db.WindowsAccount
                                               join devicePc in db.DevicePC
                                               on windowsAccount.DevicePCId
                                               equals devicePc.DevicePCId
                                               where windowsAccount.DevicePCId == deviceId
                                               && devicePc.ParentId == parent.Id
                                               select new WindowsAccountModel 
                                               {
                                                   WindowsAccountId = windowsAccount.WindowsAccountId,
                                                   WindowsAccountName = windowsAccount.WindowsAccountName,
                                                   DevicePCId = windowsAccount.DevicePCId,
                                                   InfantAccountId = windowsAccount.InfantAccountId
                                               }).ToList();

                        ViewBag.InfantAccounts = infantAccounts;
                        devicePCModel.windowsAccountList = windowsAccounts;                        
                    }
                    else
                    {
                        Response.Write("<script>alert('Ha ocurrido un error inesperado. Vuelva a intentarlo.');</script>");
                        return View();
                    }
                }


                return View(devicePCModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        [HttpPost]
        public ActionResult DevicePcDetails(DevicePCModel devicePCModel)
        {
            try
            {
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    
                }


                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        public ActionResult DevicePhoneDetails(string deviceCode)
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}