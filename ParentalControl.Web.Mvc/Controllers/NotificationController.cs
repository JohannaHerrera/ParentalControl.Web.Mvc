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
    public class NotificationController : BaseController
    {
        // GET: Notification
        [AuthorizeParent]
        public ActionResult Index()
        {
            try
            {
                var parent = this.GetCurrentUserInfo();
                List<RequestModel> requestModelList = new List<RequestModel>();
                AppConstants constants = new AppConstants();

                using (var db = new ParentalControlDBEntities())
                {
                    var requestList = (from request in db.Request
                                       join infant in db.InfantAccount
                                       on request.InfantAccountId
                                       equals infant.InfantAccountId
                                       where request.ParentId == parent.Id
                                       select new RequestModel 
                                       {
                                           RequestId = request.RequestId,
                                           RequestTypeId = request.RequestTypeId,
                                           InfantAccountId = request.InfantAccountId,
                                           RequestObject = request.RequestObject,
                                           RequestState = request.RequestState,
                                           RequestTime = request.RequestTime,
                                           InfantGender = infant.InfantGender,
                                           InfantName = infant.InfantName,
                                           DevicePCId = request.DevicePCId,
                                           DevicePhoneId = request.DevicePhoneId
                                       }).ToList();
                    
                    foreach(var request in requestList)
                    {
                        string nameDevice = string.Empty;

                        if (request.DevicePhoneId != null)
                        {
                            nameDevice = (from device in db.DevicePhone
                                          where device.DevicePhoneId == request.DevicePhoneId
                                          select device).FirstOrDefault().DevicePhoneName;
                        }
                        else if (request.DevicePCId != null)
                        {
                            nameDevice = (from device in db.DevicePC
                                          where device.DevicePCId == request.DevicePCId
                                          select device).FirstOrDefault().DevicePCName;
                        }

                        if (request.RequestTypeId == constants.WebConfiguration)
                        {
                            request.MessageNotification = $"Petición para habilitar el acceso a la categoría" +
                                                          $" web: {request.RequestObject}.";
                        }
                        else if (request.RequestTypeId == constants.AppConfiguration)
                        {
                            request.MessageNotification = $"Petición para habilitar el acceso a la aplicación:" +
                                                          $" {request.RequestObject} del dispositivo {nameDevice}.";
                        }
                        else if (request.RequestTypeId == constants.DeviceConfiguration)
                        {
                            string[] time = request.RequestTime.ToString().Split('.');
                            int numEntero = 0;
                            int numDecimal = 0;

                            if (time.Count() > 1)
                            {
                                numEntero = int.Parse(time[0]);
                                numDecimal = int.Parse(time[1]);
                            }
                            else
                            {
                                numEntero = int.Parse(time[0]);
                            }

                            if (numEntero > 0)
                            {
                                if (numEntero == 1)
                                {
                                    if (numDecimal > 0)
                                    {
                                        request.MessageNotification = $"Petición para extender el tiempo de uso del " +
                                                                      $"dispositivo {nameDevice} por {numEntero} hora " +
                                                                      $"y {numDecimal}" +
                                                                      $" minutos.";
                                    }
                                    else
                                    {
                                        request.MessageNotification = $"Petición para extender el tiempo de uso del " +
                                                                      $"dispositivo {nameDevice} por {numEntero} hora.";
                                    }
                                }
                                else
                                {
                                    if (numDecimal > 0)
                                    {
                                        request.MessageNotification = $"Petición para extender el tiempo de uso del " +
                                                                      $"dispositivo {nameDevice} por {numEntero} horas " +
                                                                      $"y {numDecimal}" +
                                                                      $" minutos.";
                                    }
                                    else
                                    {
                                        request.MessageNotification = $"Petición para extender el tiempo de uso del " +
                                                                      $"dispositivo {nameDevice} por {numEntero} horas.";
                                    }
                                }
                            }
                            else
                            {
                                request.MessageNotification = $"Petición para extender el tiempo de uso del " +
                                                              $"dispositivo {nameDevice} por {numDecimal} minutos.";
                            }
                        }
                    }

                    requestModelList = requestList;
                }

                return View(requestModelList);
            }
            catch (Exception ex)
            { 
                return View();
            }
        }

        [AuthorizeParent]
        public ActionResult ApproveRequest(int requestId)
        {
            try
            {
                AppConstants appConstants = new AppConstants();
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    var requestModel = (from request in db.Request
                                        where request.RequestId == requestId
                                        && request.ParentId == parent.Id
                                        select request).FirstOrDefault();

                    if (requestModel != null)
                    {
                        requestModel.RequestState = appConstants.RequestStateApproved;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index", "Notification");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        public ActionResult DispproveRequest(int requestId)
        {
            try
            {
                AppConstants appConstants = new AppConstants();
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    var requestModel = (from request in db.Request
                                        where request.RequestId == requestId
                                        && request.ParentId == parent.Id
                                        select request).FirstOrDefault();

                    if (requestModel != null)
                    {
                        requestModel.RequestState = appConstants.RequestStateDisapproved;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index", "Notification");
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}