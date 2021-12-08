using ParentalControl.Web.Mvc.Data;
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

                    deviceListModel = new DeviceListModel();
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
    }
}