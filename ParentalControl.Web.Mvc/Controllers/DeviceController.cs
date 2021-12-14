using ParentalControl.Web.Mvc.Business.AppConstants;
using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.Filters;
using ParentalControl.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ParentalControl.Web.Mvc.Models.Enum;

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
                                                    && app.DevicePCId == windowsAccount.DevicePCId
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
                                                        && app.DevicePCId == windowsAccount.DevicePCId
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
                
                Alert("¡La información se actualizó correctamente!", NotificationType.success);
                return RedirectToAction("DevicePcDetails", "Device", new { deviceId = devicePCModel.DevicePCId });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        public ActionResult DeleteDevicePC(int deviceId)
        {
            try
            {
                DevicePCModel devicePCModel = new DevicePCModel();
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    devicePCModel = (from device in db.DevicePC
                                    where device.DevicePCId == deviceId
                                    && device.ParentId == parent.Id
                                    select new DevicePCModel
                                    {
                                        DevicePCId = device.DevicePCId,
                                        DevicePCName = device.DevicePCName,
                                        ParentId = device.ParentId
                                    }).FirstOrDefault();
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
        public ActionResult DeleteDevicePC(DevicePCModel devicePCModel)
        {
            try
            {
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    var devicePC = (from device in db.DevicePC
                                    where device.DevicePCId == devicePCModel.DevicePCId
                                    && device.ParentId == devicePCModel.ParentId
                                    select device).FirstOrDefault();

                    if(devicePC != null)
                    {
                        var windowsAccounts = db.WindowsAccount.Where(x => x.DevicePCId == devicePC.DevicePCId);
                        var apps = db.App.Where(x => x.DevicePCId == devicePC.DevicePCId);
                        var appsDevice = db.AppDevice.Where(x => x.DevicePCId == devicePC.DevicePCId);

                        db.WindowsAccount.RemoveRange(windowsAccounts);
                        db.App.RemoveRange(apps);
                        db.AppDevice.RemoveRange(appsDevice);
                        db.DevicePC.Remove(devicePC);
                        db.SaveChanges();
                    }
                }

                Alert("¡Dispositivo Eliminado!", NotificationType.success);
                return RedirectToAction("Index", "Device");
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

                        List<ScheduleOptionModel> scheduleOptionModelList = new List<ScheduleOptionModel>();

                        var scheduleModelList = (from schedule in db.Schedule
                                                 where schedule.ParentId == parent.Id
                                                 select new ScheduleOptionModel
                                                 {
                                                     ScheduleId = schedule.ScheduleId,
                                                     ScheduleStartTime = schedule.ScheduleStartTime,
                                                     ScheduleEndTime = schedule.ScheduleEndTime                                             
                                                 }).ToList();

                        var deviceUseModelList = (from deviceUse in db.DevicePhoneUse
                                                  where deviceUse.DevicePhoneId == devicePhoneModel.DevicePhoneId
                                                  select deviceUse).ToList();


                        foreach(var schedule in scheduleModelList)
                        {
                            schedule.ScheduleTime = $"{schedule.ScheduleStartTime.ToString("HH:mm")} - {schedule.ScheduleEndTime.ToString("HH:mm")}";
                        }

                        InfantAccountModel noProtected = new InfantAccountModel();
                        noProtected.InfantAccountId = 0;
                        noProtected.InfantName = appConstants.NoProtected;

                        infantAccountModelList.Add(noProtected);
                        infantAccountModelList.AddRange(infantAccounts);

                        var infantAccountList = new SelectList(infantAccountModelList, "InfantAccountId", "InfantName");
                        ViewData["infantAccounts"] = infantAccountList;

                        ScheduleOptionModel scheduleOp = new ScheduleOptionModel();
                        scheduleOp.ScheduleId = 0;
                        scheduleOp.ScheduleTime = "Ninguno";

                        scheduleOptionModelList.Add(scheduleOp);
                        scheduleOptionModelList.AddRange(scheduleModelList);

                        var scheduleList = new SelectList(scheduleOptionModelList, "ScheduleId", "ScheduleTime");
                        ViewData["scheduleList"] = scheduleList;

                        devicePhoneModel.deviceUseList = deviceUseModelList;
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

        [AuthorizeParent]
        [HttpPost]
        public ActionResult DevicePhoneDetails(DevicePhoneModel devicePhoneModel, string infantAccountSelected, string schedules)
        {
            try
            { 
                var parent = this.GetCurrentUserInfo();
                int infantId = Convert.ToInt32(infantAccountSelected);
                char delimitador = ',';
                List<string> scheduleIdList = schedules.Split(delimitador).ToList();

                using (var db = new ParentalControlDBEntities())
                {
                    // Obtengo el dispositivo a actualizar
                    var devicePhone = (from device in db.DevicePhone
                                       where device.DevicePhoneId == devicePhoneModel.DevicePhoneId
                                       && device.ParentId == parent.Id
                                       select device).FirstOrDefault();

                    if(devicePhone != null)
                    {
                        // Verifico si se cambió el nombre del dispositivo
                        if(devicePhoneModel.DevicePhoneName != null)
                        {
                            devicePhone.DevicePhoneName = devicePhoneModel.DevicePhoneName;
                            db.SaveChanges();
                        }

                        // Verifico si se cambió la cuenta infantil vinculada
                        if(devicePhone.InfantAccountId != infantId)
                        {
                            // Valido que no haya estado como No Protegido
                            if (!(devicePhone.InfantAccountId == null && infantId == 0))
                            {
                                // Valido si se cambió a No Protegido
                                if (infantId == 0)
                                {
                                    // Cambio a NULL (No Protegido)
                                    devicePhone.InfantAccountId = null;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    // Actualizo la nueva cuenta infantil vinculada
                                    devicePhone.InfantAccountId = infantId;
                                    db.SaveChanges();

                                    // Registro las aplicaciones con la nueva cuenta infantil
                                    List<App> appList = new List<App>();
                                    AppConstants appConstants = new AppConstants();

                                    var appsDevice = (from app in db.AppDevice
                                                      where app.DevicePhoneId == devicePhoneModel.DevicePhoneId
                                                      select app).ToList();

                                    foreach (var app in appsDevice)
                                    {
                                        App appModel = new App();
                                        appModel.InfantAccountId = infantId;
                                        appModel.DevicePhoneId = devicePhoneModel.DevicePhoneId;
                                        appModel.AppName = app.AppDeviceName;
                                        appModel.AppStatus = appConstants.Access;
                                        appModel.AppAccessPermission = appConstants.Access;
                                        appModel.AppCreationDate = DateTime.Now;
                                        appList.Add(appModel);
                                    }

                                    db.App.AddRange(appList);
                                    db.SaveChanges();
                                }

                                if (devicePhoneModel.InfantAccountId != null)
                                {
                                    // Elimino las aplicaciones registradas de la anterior cuenta infantil
                                    var apps = (from app in db.App
                                                where app.InfantAccountId == devicePhoneModel.InfantAccountId
                                                && app.DevicePhoneId == devicePhoneModel.DevicePhoneId
                                                select app).ToList();

                                    db.App.RemoveRange(apps);
                                    db.SaveChanges();
                                }                              
                            }
                        }

                        // Verifico si hubo cambios en el uso del dispositivo
                        var devicePhoneUseList = (from deviceUse in db.DevicePhoneUse
                                                  where deviceUse.DevicePhoneId == devicePhoneModel.DevicePhoneId
                                                  select deviceUse).ToList();

                        int cont = 0;
                        foreach(var deviceUse in devicePhoneUseList)
                        {
                            int scheduleId = Convert.ToInt32(scheduleIdList[cont]);

                            // Valido si hubo cambios en el horario de uso de cada día
                            if (deviceUse.ScheduleId != scheduleId)
                            {
                                if(!(deviceUse.ScheduleId == null && scheduleId == 0))
                                {

                                    // Valido si se cambió a Ninguno (ningún horario)
                                    if (scheduleId == 0)
                                    {
                                        deviceUse.ScheduleId = null;
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        deviceUse.ScheduleId = scheduleId;
                                        db.SaveChanges();
                                    }
                                }
                            }

                            cont++;
                        }
                    }                   
                }

                Alert("¡La información se actualizó correctamente!", NotificationType.success);
                return RedirectToAction("DevicePhoneDetails", "Device", new { deviceId = devicePhoneModel.DevicePhoneId });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        public ActionResult DeleteDevicePhone(int deviceId)
        {
            try
            {
                DevicePhoneModel devicePhoneModel = new DevicePhoneModel();
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    devicePhoneModel = (from device in db.DevicePhone
                                       where device.DevicePhoneId == deviceId
                                       && device.ParentId == parent.Id
                                       select new DevicePhoneModel
                                       {
                                           DevicePhoneId = device.DevicePhoneId,
                                           DevicePhoneName = device.DevicePhoneName,
                                           ParentId = device.ParentId
                                       }).FirstOrDefault();
                }

                return View(devicePhoneModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        [HttpPost]
        public ActionResult DeleteDevicePhone(DevicePhoneModel devicePhoneModel)
        {
            try
            {
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    var devicePhone = (from device in db.DevicePhone
                                      where device.DevicePhoneId == devicePhoneModel.DevicePhoneId
                                      && device.ParentId == devicePhoneModel.ParentId
                                      select device).FirstOrDefault();
                                      
                    if (devicePhone != null)
                    {
                        var deviceUseSchedules = db.DevicePhoneUse.Where(x => x.DevicePhoneId == devicePhone.DevicePhoneId);
                        var apps = db.App.Where(x => x.DevicePhoneId == devicePhone.DevicePhoneId);
                        var appsDevice = db.AppDevice.Where(x => x.DevicePhoneId == devicePhone.DevicePhoneId);

                        db.DevicePhoneUse.RemoveRange(deviceUseSchedules);
                        db.App.RemoveRange(apps);
                        db.AppDevice.RemoveRange(appsDevice);
                        db.DevicePhone.Remove(devicePhone);
                        db.SaveChanges();
                    }
                }

                Alert("¡Dispositivo Eliminado!", NotificationType.success);
                return RedirectToAction("Index", "Device");
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}