using ParentalControl.Web.Mvc.Business.AppConstants;
using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.Filters;
using ParentalControl.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using System.Windows.Forms;
using static ParentalControl.Web.Mvc.Models.Enum;

namespace ParentalControl.Web.Mvc.Controllers
{
    public class InfantAccountController : BaseController
    {
        //List Schedule
        [AuthorizeParent]
        public ActionResult Index()
        {
            try
            {
                List<InfantAccountModel> infantAccountModelList = new List<InfantAccountModel>();

                var parent = this.GetCurrentUserInfo();
                using (var db = new ParentalControlDBEntities())
                {
                    infantAccountModelList = (from InfantAccount in db.InfantAccount
                                    where InfantAccount.ParentId == parent.Id
                                    select new InfantAccountModel
                                    {
                                        InfantAccountId = InfantAccount.InfantAccountId,
                                        InfantName = InfantAccount.InfantName,
                                        InfantGender = InfantAccount.InfantGender,
                                        InfantCreationDate = InfantAccount.InfantCreationDate,
                                        ParentId = InfantAccount.ParentId
                                    }).ToList();
                }
                ViewBag.list = infantAccountModelList;
                return View(infantAccountModelList);

            }
            catch (Exception ex)
            {
                return View();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthorizeParent]
        public ActionResult AddInfantAccount()
        {
            var parent = this.GetCurrentUserInfo();
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="infantAccountModel"></param>
        /// <returns></returns>
        [AuthorizeParent]
        [HttpPost]
        public ActionResult AddInfantAccount(InfantAccountModel infantAccountModel)
        {
            try
            {
                var creationDate = DateTime.Now;
                var parent = this.GetCurrentUserInfo();
                if (infantAccountModel != null)
                {
                    if (ModelState.IsValid)
                    {
                        using (var db = new ParentalControlDBEntities())
                        {
                            InfantAccount infantAccount = new InfantAccount();
                            infantAccount.InfantName = infantAccountModel.InfantName;
                            infantAccount.InfantGender = infantAccountModel.InfantGender;
                            infantAccount.InfantCreationDate = creationDate;
                            infantAccount.ParentId = parent.Id;
                            db.InfantAccount.Add(infantAccount);
                            db.SaveChanges();
                        }
                        Alert("Se ha creado el Infante con éxito", NotificationType.success);
                        return Redirect("Index");
                    }
                    else
                    {
                        return View(infantAccountModel);
                    }
                }
                return View();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AuthorizeParent]
        public ActionResult EditInfantAccount(int? infantAccountId)
        {
            if (infantAccountId != null)
            {
                try
                {
                    InfantAccount infantAccount = new InfantAccount();
                    InfantAccountModel infantAccountModel = new InfantAccountModel();
                    var parent = this.GetCurrentUserInfo();

                    using (var db = new ParentalControlDBEntities())
                    {
                        infantAccount = db.InfantAccount.Find(infantAccountId);
                    }
                    infantAccountModel.InfantAccountId = infantAccount.InfantAccountId;
                    infantAccountModel.InfantName = infantAccount.InfantName;
                    infantAccountModel.InfantGender = infantAccount.InfantGender;
                    infantAccountModel.InfantCreationDate = infantAccount.InfantCreationDate;
                    infantAccountModel.ParentId = parent.Id;
                    ViewBag.list = infantAccountModel;
                    return View(infantAccountModel);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "News");
                }
            }
            else
            {
                return View();
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="infantAccountModel"></param>
        /// <returns></returns>
        [AuthorizeParent]
        [HttpPost]
        public ActionResult EditInfantAccount(InfantAccountModel infantAccountModel)
        {
            try
            {
                var parent = this.GetCurrentUserInfo();
                if (infantAccountModel != null)
                {
                    if (ModelState.IsValid)
                    {
                        using (var db = new ParentalControlDBEntities())
                        {
                            InfantAccount infantAccount = db.InfantAccount.Find(infantAccountModel.InfantAccountId);
                            infantAccount.InfantName = infantAccountModel.InfantName;
                            infantAccount.InfantGender = infantAccountModel.InfantGender;
                            db.Entry(infantAccount).State=System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        Alert("Se ha editado el Infante con éxito", NotificationType.success);
                        return Redirect("Index");
                    }
                    else
                    {
                        Alert("Ocurrió un problema al editar el Infante", NotificationType.error);
                        return View(infantAccountModel);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="infantAccountId"></param>
        /// <returns></returns>
        [AuthorizeParent]
        [HttpGet]
        public ActionResult DeleteInfantAccount(int? infantAccountId)
        {
            try
            {
                Activity activity = new Activity();
                App app = new App();
                DevicePhone devicePhone = new DevicePhone();
                DeviceUse deviceUse = new DeviceUse();
                Request request = new Request();
                WebConfiguration webConfiguration = new WebConfiguration();
                WindowsAccount windowsAccount = new WindowsAccount();
                InfantAccount infantAccount = new InfantAccount();

                using (var db = new ParentalControlDBEntities())
                {
                    activity = db.Activity.Find(infantAccountId);
                    if (activity != null)
                    {
                        db.Activity.Remove(activity);
                        db.SaveChanges();
                    }
                    app = db.App.Find(infantAccountId);
                    if (app != null)
                    {
                        db.App.Remove(app);
                        db.SaveChanges();
                    }
                    devicePhone = db.DevicePhone.Find(infantAccountId);
                    if (devicePhone != null)
                    {
                        devicePhone.InfantAccountId = null;
                        db.Entry(devicePhone).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    deviceUse = db.DeviceUse.Find(infantAccountId);
                    if (deviceUse != null)
                    {
                        db.DeviceUse.Remove(deviceUse);
                        db.SaveChanges();
                    }
                    request = db.Request.Find(infantAccountId);
                    if (request != null)
                    {
                        db.Request.Remove(request);
                        db.SaveChanges();
                    }
                    webConfiguration = db.WebConfiguration.Find(infantAccountId);
                    if (webConfiguration != null)
                    {
                        db.WebConfiguration.Remove(webConfiguration);
                        db.SaveChanges();
                    }
                    windowsAccount = db.WindowsAccount.Find(infantAccountId);
                    if (windowsAccount != null)
                    {
                        windowsAccount.InfantAccountId = null;
                        db.Entry(windowsAccount).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                    infantAccount = db.InfantAccount.Find(infantAccountId);
                    db.InfantAccount.Remove(infantAccount);
                    db.SaveChanges();
                }
                Alert("Se eliminó la cuenta de infante satisfactoriamente", NotificationType.success);
                return Redirect("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        [AuthorizeParent]
        public ActionResult RulesInfantAccount(int infantAccountId)
        {
            try
            {
                InfantAccountRules infantAccountRulesModel = new InfantAccountRules();
                List<AppModel> appModelList = new List<AppModel>();
                List<WebConfigurationModel> webConfigurationModelList = new List<WebConfigurationModel>();
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    // ************************* CONFIGURACIÓN WEB *************************

                    var webConfiguration = (from webConfig in db.WebConfiguration
                                            where webConfig.InfantAccountId == infantAccountId
                                            select webConfig).ToList();

                    foreach (var web in webConfiguration)
                    {
                        WebConfigurationModel webConfigurationModel = new WebConfigurationModel();
                        var webCategory = db.WebCategory.Where(x => x.CategoryId == web.CategoryId).FirstOrDefault();
                        webConfigurationModel.CategoryName = webCategory.CategoryName;
                        webConfigurationModel.InfantAccountId = web.InfantAccountId;
                        webConfigurationModel.WebConfigurationId = web.WebConfigurationId;
                        webConfigurationModel.WebConfigurationAccess = web.WebConfigurationAccess;
                        webConfigurationModelList.Add(webConfigurationModel);
                    }


                    // ************************* CONFIGURACIÓN DE APLICACIONES *************************

                    var appConfig = (from app in db.App
                                    where app.InfantAccountId == infantAccountId
                                    select app).ToList();                    

                    foreach(var app in appConfig)
                    {
                        AppModel appModel = new AppModel();

                        if (app.DevicePCId != null)
                        {
                            var device = db.DevicePC.Where(x => x.DevicePCId == app.DevicePCId).FirstOrDefault();
                            appModel.DeviceName = device.DevicePCName;
                        }
                        else
                        {
                            var device = db.DevicePhone.Where(x => x.DevicePhoneId == app.DevicePhoneId).FirstOrDefault();
                            appModel.DeviceName = device.DevicePhoneName;
                        }

                        appModel.AppId = app.AppId;
                        appModel.AppName = app.AppName;
                        appModel.AppAccessPermission = app.AppAccessPermission;
                        appModel.ScheduleId = app.ScheduleId;
                        appModel.InfantAccountId = app.InfantAccountId;

                        appModelList.Add(appModel);
                    }


                    // ************************* CONFIGURACIÓN DE USO DEL DISPOSITIVO *************************

                    var deviceUseConfig = (from deviceUse in db.DeviceUse
                                           where deviceUse.InfantAccountId == infantAccountId
                                           select deviceUse).ToList();


                    // ************************* HISTORIAL DE ACTIVIDAD *************************

                    var activityList = (from activity in db.Activity
                                        where activity.InfantAccountId == infantAccountId
                                        select activity).ToList();


                    // ************************* MODELO INFANT ACCOUNT RULES *************************

                    var infantAc = (from infant in db.InfantAccount
                                    where infant.InfantAccountId == infantAccountId
                                    select infant).FirstOrDefault();

                    infantAccountRulesModel.InfantAccountId = infantAccountId;
                    infantAccountRulesModel.InfantName = infantAc.InfantName;
                    infantAccountRulesModel.AppList = appModelList;
                    infantAccountRulesModel.WebConfigurationList = webConfigurationModelList;
                    infantAccountRulesModel.DeviceUseList = deviceUseConfig;
                    infantAccountRulesModel.ActivityList = activityList;


                    // ************************* COMBOBOX HORARIOS *************************

                    List<ScheduleOptionModel> scheduleOptionModelList = new List<ScheduleOptionModel>();

                    var scheduleModelList = (from schedule in db.Schedule
                                             where schedule.ParentId == parent.Id
                                             select new ScheduleOptionModel
                                             {
                                                 ScheduleId = schedule.ScheduleId,
                                                 ScheduleStartTime = schedule.ScheduleStartTime,
                                                 ScheduleEndTime = schedule.ScheduleEndTime
                                             }).ToList();

                    foreach (var schedule in scheduleModelList)
                    {
                        schedule.ScheduleTime = $"{schedule.ScheduleStartTime.ToString("HH:mm")} - {schedule.ScheduleEndTime.ToString("HH:mm")}";
                    }

                    ScheduleOptionModel scheduleOp = new ScheduleOptionModel();
                    scheduleOp.ScheduleId = 0;
                    scheduleOp.ScheduleTime = "Ninguno";

                    scheduleOptionModelList.Add(scheduleOp);
                    scheduleOptionModelList.AddRange(scheduleModelList);

                    var scheduleList = new SelectList(scheduleOptionModelList, "ScheduleId", "ScheduleTime");
                    ViewData["scheduleList"] = scheduleList;
                }
                
                return View(infantAccountRulesModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        [AuthorizeParent]
        [HttpPost]
        public ActionResult RulesInfantAccount(int infantAccountId, string webConfig, string appAccessConfig, string appScheduleConfig, string deviceConfig)
        {
            try
            {
                char delimitador = ',';
                List<string> webConfigList = webConfig.Split(delimitador).ToList();
                List<string> appAcessConfigList = appAccessConfig.Split(delimitador).ToList();
                List<string> appScheduleConfigList = appScheduleConfig.Split(delimitador).ToList();
                List<string> deviceConfigList = deviceConfig.Split(delimitador).ToList();

                InfantAccountRules infantAccountRulesModel = new InfantAccountRules();
                List<AppModel> appModelList = new List<AppModel>();
                List<WebConfigurationModel> webConfigurationModelList = new List<WebConfigurationModel>();
                AppConstants appConstants = new AppConstants();
                int cont = 0;

                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    // ************************* CONFIGURACIÓN WEB *************************

                    var webConfiguration = (from webConf in db.WebConfiguration
                                            where webConf.InfantAccountId == infantAccountId
                                            select webConf).ToList();

                    cont = 0;
                    foreach (var web in webConfiguration)
                    {
                        if (webConfigList[cont] == "0")
                        {
                            web.WebConfigurationAccess = appConstants.Access;
                        }
                        else
                        {
                            web.WebConfigurationAccess = appConstants.NoAccess;
                        }

                        db.SaveChanges();
                        cont++;                       
                    }


                    // ************************* CONFIGURACIÓN DE APLICACIONES *************************

                    var appConfig = (from app in db.App
                                     where app.InfantAccountId == infantAccountId
                                     select app).ToList();

                    cont = 0;
                    foreach (var app in appConfig)
                    {
                        if (appScheduleConfigList[cont] == "0")
                        {
                            if (appAcessConfigList[cont] == "0")
                            {
                                app.AppAccessPermission = appConstants.Access;
                            }
                            else
                            {
                                app.AppAccessPermission = appConstants.NoAccess;
                            }

                            app.ScheduleId = null;
                        }
                        else
                        {
                            app.AppAccessPermission = appConstants.Access;
                            app.ScheduleId = Convert.ToInt32(appScheduleConfigList[cont]);
                        }

                        db.SaveChanges();
                        cont++;
                    }


                    // ************************* CONFIGURACIÓN DE USO DEL DISPOSITIVO *************************
                    var deviceUseConfig = (from deviceUse in db.DeviceUse
                                           where deviceUse.InfantAccountId == infantAccountId
                                           select deviceUse).ToList();

                    cont = 0;
                    foreach (var deviceUse in deviceUseConfig)
                    {
                        if (deviceConfigList[cont] == "0")
                        {
                            deviceUse.ScheduleId = null;
                        }
                        else
                        {
                            deviceUse.ScheduleId = Convert.ToInt32(deviceConfigList[cont]);
                        }

                        db.SaveChanges();
                        cont++;
                    }

                }

                Alert("¡La configuración se actualizó correctamente!", NotificationType.success);
                return RedirectToAction("RulesInfantAccount", "InfantAccount", new { infantAccountId = infantAccountId });
            }
            catch (Exception ex)
            {
                Alert("Ocurrió un error al actualizar la configuración. Inténtelo de nuevo", NotificationType.error);
                return RedirectToAction("RulesInfantAccount", "InfantAccount", new { infantAccountId = infantAccountId });
            }
        }
    }
}