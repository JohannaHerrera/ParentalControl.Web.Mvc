﻿using ParentalControl.Web.Mvc.Data;
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

    }
}