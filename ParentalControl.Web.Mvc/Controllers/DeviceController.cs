using ParentalControl.Web.Mvc.Business.AppConstants;
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
                    // Dispositivos PC
                    devicePCList = (from devicePC in db.DevicePC
                                    where devicePC.ParentId == parent.Id
                                    select new DevicePCModel
                                    {
                                        DevicePCId = devicePC.DevicePCId,
                                        DevicePCName = devicePC.DevicePCName,
                                        DevicePCCode = devicePC.DevicePCCode,
                                        ParentId = devicePC.ParentId
                                    }).ToList();

                    // Verifico si hay cuentas infantiles vinculadas a cuentas Windows
                    foreach (var device in devicePCList)
                    {
                        var windowsAccountList = (from windowsAccount in db.WindowsAccount
                                               join devicePc in db.DevicePC
                                               on windowsAccount.DevicePCId
                                               equals devicePc.DevicePCId
                                               where windowsAccount.DevicePCId == device.DevicePCId
                                               select new WindowsAccountModel
                                               {
                                                   WindowsAccountId = windowsAccount.WindowsAccountId,
                                                   WindowsAccountName = windowsAccount.WindowsAccountName,
                                                   DevicePCId = windowsAccount.DevicePCId,
                                                   InfantAccountId = windowsAccount.InfantAccountId
                                               }).ToList();

                        device.deviceProtected = false;

                        foreach (var windowsAccount in windowsAccountList)
                        {
                            if (windowsAccount.InfantAccountId != null || windowsAccount.InfantAccountId > 0)
                            {
                                device.deviceProtected = true;
                                break;
                            }
                        }
                    }


                    // Dispositivos Móviles
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

                    // Verifico si el dispositivo está vinculado a una cuenta infantil
                    foreach (var device in devicePhoneList)
                    {                       
                        device.deviceProtected = false;
                        
                        if (device.InfantAccountId != null || device.InfantAccountId > 0)
                        {
                            device.deviceProtected = true;
                            break;
                        }                        
                    }

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
                AppConstants appConstants = new AppConstants();
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
                        List<InfantAccountModel> infantAccountModelList = new List<InfantAccountModel>();

                        var infantAccounts = (from infantAccount in db.InfantAccount
                                              where infantAccount.ParentId == parent.Id
                                              select new InfantAccountModel()
                                              { 
                                                  InfantAccountId = infantAccount.InfantAccountId,
                                                  InfantName = infantAccount.InfantName
                                              }).ToList();

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


                        InfantAccountModel noProtected = new InfantAccountModel();
                        noProtected.InfantAccountId = 0;
                        noProtected.InfantName = appConstants.NoProtected;

                        infantAccountModelList.Add(noProtected);
                        infantAccountModelList.AddRange(infantAccounts);

                        var infantAccountList = new SelectList(infantAccountModelList, "InfantAccountId", "InfantName");
                        ViewData["infantAccounts"] = infantAccountList;

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
        public ActionResult DevicePcDetails(DevicePCModel devicePCModel, string windowsAccounts, string infantAccountsSelected)
        {
            try
            {
                char delimitador = ',';
                List<string> windAccount = windowsAccounts.Split(delimitador).ToList();
                List<string> infAccount = infantAccountsSelected.Split(delimitador).ToList();

                using (var db = new ParentalControlDBEntities())
                {
                    var windowsAccountList = (from windowsAccount in db.WindowsAccount
                                              join devicePc in db.DevicePC
                                              on windowsAccount.DevicePCId
                                              equals devicePc.DevicePCId
                                              where windowsAccount.DevicePCId == devicePCModel.DevicePCId
                                              && devicePc.ParentId == devicePCModel.ParentId
                                              select new WindowsAccountModel
                                              {
                                                  InfantAccountId = windowsAccount.InfantAccountId,
                                                  WindowsAccountName = windowsAccount.WindowsAccountName,
                                                  WindowsAccountId = windowsAccount.WindowsAccountId,
                                                  DevicePCId = windowsAccount.DevicePCId
                                              }).ToList();

                    foreach (var windowsAccount in windowsAccountList)
                    {
                        int index = windAccount.IndexOf(windowsAccount.WindowsAccountName);
                        int infantId = Convert.ToInt32(infAccount[index]);

                        // Se asignó una cuenta infantil
                        if (windowsAccount.InfantAccountId != infantId)
                        {
                            if (!(windowsAccount.InfantAccountId == null && infantId == 0))
                            {
                                // Valido si se cambió a No Protegido
                                if (infantId == 0)
                                {
                                    // Verifico si hay más cuentas Windows asignadas a la cuenta infantil anterior
                                    var windowsAccountsLinked = (from account in windowsAccountList
                                                                 where account.InfantAccountId == windowsAccount.InfantAccountId
                                                                 select account).ToList();

                                    // Query para obtener el registro a actualizar
                                    var windowsAccountUpdate = (from account in db.WindowsAccount
                                                                where account.WindowsAccountId == windowsAccount.WindowsAccountId
                                                                && account.WindowsAccountId == windowsAccount.WindowsAccountId
                                                                select account).FirstOrDefault();

                                    windowsAccountUpdate.InfantAccountId = null;
                                    db.SaveChanges();

                                    // Si no hay más cuentas Windows asignadas a la cuenta infantil anterior
                                    // Elimino las aplicaciones asignadas al infante anterior
                                    if (windowsAccountsLinked.Count == 1)
                                    {
                                        var apps = (from app in db.App
                                                    where app.InfantAccountId == windowsAccount.InfantAccountId
                                                    select app).ToList();

                                        db.App.RemoveRange(apps);
                                        db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    // Query para obtener el registro a actualizar
                                    var windowsAccountUpdate = (from account in db.WindowsAccount
                                                                where account.WindowsAccountId == windowsAccount.WindowsAccountId
                                                                && account.WindowsAccountId == windowsAccount.WindowsAccountId
                                                                select account).FirstOrDefault();

                                    windowsAccountUpdate.InfantAccountId = infantId;
                                    db.SaveChanges();

                                    // Verifico si anteriormente estaba como No Protegido
                                    if (windowsAccount.InfantAccountId == null)
                                    {
                                        //Verifico si ya existe la cuenta infantil vinculada a cuenta windows
                                        var infantIsRegister = (from infant in db.WindowsAccount
                                                                where infant.InfantAccountId == infantId
                                                                && infant.DevicePCId == windowsAccount.DevicePCId
                                                                select infant).ToList();

                                        if(infantIsRegister.Count == 1)
                                        {
                                            List<App> appList = new List<App>();
                                            AppConstants appConstants = new AppConstants();

                                            // Registro las aplicaciones
                                            var appsDevice = (from appDevice in db.AppDevice
                                                              where appDevice.DevicePCId == windowsAccount.DevicePCId
                                                              select appDevice).ToList();

                                            foreach (var app in appsDevice)
                                            {
                                                App appModel = new App();
                                                appModel.InfantAccountId = infantId;
                                                appModel.DevicePCId = windowsAccount.DevicePCId;
                                                appModel.AppName = app.AppDeviceName;
                                                appModel.AppStatus = appConstants.Access;
                                                appModel.AppAccessPermission = appConstants.Access;
                                                appModel.AppCreationDate = DateTime.Now;
                                                appList.Add(appModel);
                                            }

                                            db.App.AddRange(appList);
                                            db.SaveChanges();
                                        }                                       
                                    }
                                    else
                                    {
                                        // Verifico si hay más cuentas Windows asignadas a la cuenta infantil anterior
                                        var windowsAccountsLinked = (from account in windowsAccountList
                                                                     where account.InfantAccountId == windowsAccount.InfantAccountId
                                                                     select account).ToList();

                                        // Si no hay más cuentas Windows asignadas a la cuenta infantil anterior
                                        // Elimino las aplicaciones asignadas al infante anterior
                                        if (windowsAccountsLinked.Count == 1)
                                        {
                                            var apps = (from app in db.App
                                                        where app.InfantAccountId == windowsAccount.InfantAccountId
                                                        select app).ToList();

                                            db.App.RemoveRange(apps);
                                            db.SaveChanges();
                                        }

                                        //Verifico si ya existe la cuenta infantil vinculada a cuenta windows
                                        var infantIsRegister = (from infant in db.WindowsAccount
                                                                where infant.InfantAccountId == infantId
                                                                && infant.DevicePCId == windowsAccount.DevicePCId
                                                                select infant).ToList();

                                        if (infantIsRegister.Count == 1)
                                        {
                                            List<App> appList = new List<App>();
                                            AppConstants appConstants = new AppConstants();

                                            // Registro las aplicaciones
                                            var appsDevice = (from appDevice in db.AppDevice
                                                              where appDevice.DevicePCId == windowsAccount.DevicePCId
                                                              select appDevice).ToList();

                                            foreach (var app in appsDevice)
                                            {
                                                App appModel = new App();
                                                appModel.InfantAccountId = infantId;
                                                appModel.DevicePCId = windowsAccount.DevicePCId;
                                                appModel.AppName = app.AppDeviceName;
                                                appModel.AppStatus = appConstants.Access;
                                                appModel.AppAccessPermission = appConstants.Access;
                                                appModel.AppCreationDate = DateTime.Now;
                                                appList.Add(appModel);
                                            }

                                            db.App.AddRange(appList);
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }                        
                    }

                    // Si se actualizó el nombre realizo UPDATE
                    if (devicePCModel.DevicePCName != null)
                    {
                        // Query para obtener el registro a actualizar
                        var devicePc = (from device in db.DevicePC
                                        where device.DevicePCId == devicePCModel.DevicePCId
                                        select device).FirstOrDefault();

                        devicePc.DevicePCName = devicePCModel.DevicePCName;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("DevicePcDetails", "Device", new { deviceId = devicePCModel.DevicePCId });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        public ActionResult DevicePhoneDetails(int deviceId)
        {
            try
            {
                DevicePhoneModel devicePhoneModel = new DevicePhoneModel();
                AppConstants appConstants = new AppConstants();
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    devicePhoneModel = (from devicePhone in db.DevicePhone
                                       where devicePhone.DevicePhoneId == deviceId
                                       select new DevicePhoneModel
                                       {
                                           DevicePhoneId = devicePhone.DevicePhoneId,
                                           DevicePhoneName = devicePhone.DevicePhoneName,
                                           DevicePhoneCode = devicePhone.DevicePhoneCode,
                                           ParentId = devicePhone.ParentId,
                                           InfantAccountId = devicePhone.InfantAccountId
                                       }).FirstOrDefault();

                    if (devicePhoneModel != null)
                    {
                        List<InfantAccountModel> infantAccountModelList = new List<InfantAccountModel>();

                        var infantAccounts = (from infantAccount in db.InfantAccount
                                              where infantAccount.ParentId == parent.Id
                                              select new InfantAccountModel()
                                              {
                                                  InfantAccountId = infantAccount.InfantAccountId,
                                                  InfantName = infantAccount.InfantName
                                              }).ToList();

                        InfantAccountModel noProtected = new InfantAccountModel();
                        noProtected.InfantAccountId = 0;
                        noProtected.InfantName = appConstants.NoProtected;

                        infantAccountModelList.Add(noProtected);
                        infantAccountModelList.AddRange(infantAccounts);

                        var infantAccountList = new SelectList(infantAccountModelList, "InfantAccountId", "InfantName");
                        ViewData["infantAccounts"] = infantAccountList;
                    }
                    else
                    {
                        Response.Write("<script>alert('Ha ocurrido un error inesperado. Vuelva a intentarlo.');</script>");
                        return View();
                    }
                }

                return View(devicePhoneModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}