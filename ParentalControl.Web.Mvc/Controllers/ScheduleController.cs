using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using ParentalControl.Web.Mvc.Filters;
using ParentalControl.Web.Mvc.Business.AppConstants;
using System.Windows;
using System.Windows.Forms;
using static ParentalControl.Web.Mvc.Models.Enum;

namespace ParentalControl.Web.Mvc.Controllers
{
    public class ScheduleController : BaseController
    {
        //List Schedule
        [AuthorizeParent]
        public ActionResult Index()
        {
            try
            {
                List<ScheduleModel> scheduleList = new List<ScheduleModel>();

                var parent = this.GetCurrentUserInfo();
                using (var db = new ParentalControlDBEntities())
                {
                    scheduleList = (from schedule in db.Schedule
                                    where schedule.ParentId == parent.Id
                                    select new ScheduleModel
                                    {
                                        ScheduleId = schedule.ScheduleId,
                                        ScheduleStartTime = schedule.ScheduleStartTime,
                                        ScheduleEndTime = schedule.ScheduleEndTime,
                                        ParentId = schedule.ParentId
                                    }).ToList();
                }
                ViewBag.list = scheduleList;
                return View(scheduleList);

            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        public ActionResult Create()
        {    
            return View();
        }

        [AuthorizeParent]
        [HttpPost]
        public ActionResult Create(DateTime start, DateTime end)
        {
            
            ScheduleModel scheduleModel = new ScheduleModel();
            scheduleModel.ScheduleStartTime = start;
            scheduleModel.ScheduleEndTime = end;
            var creationDate = DateTime.Now;
            var parent = this.GetCurrentUserInfo();

            if (string.IsNullOrEmpty(scheduleModel.ScheduleStartTime.ToString())
                || string.IsNullOrEmpty(scheduleModel.ScheduleEndTime.ToString()))
            {
                Alert("El registro contiene campos vacíos", NotificationType.warning);
                return View();
            }
            else
            {
                if (scheduleModel.Validate(scheduleModel))
                {
                    List<ScheduleModel> scheduleModeList = new List<ScheduleModel>();
                    scheduleModeList = ValidationSchedule(start, end, parent.Id);
                    if (scheduleModeList.Count > 0)
                    {
                        Alert("El registro ya existe", NotificationType.error);
                        return View();
                    }
                    else
                    {
                        using (var db = new ParentalControlDBEntities())
                        {
                            Schedule scheduleCreate = new Schedule();
                            scheduleCreate.ScheduleStartTime = scheduleModel.ScheduleStartTime;
                            scheduleCreate.ScheduleEndTime = scheduleModel.ScheduleEndTime;
                            scheduleCreate.ScheduleCreationDate = creationDate;
                            scheduleCreate.ParentId = parent.Id;
                            db.Schedule.Add(scheduleCreate);
                            db.SaveChanges();
                        }
                        Alert("¡Se agregó el horario correctamente!", NotificationType.success);
                        return Redirect("Index");

                    }
                }
                else
                {
                    Alert("La hora de inicio debe ser MENOR a la hora final", NotificationType.warning);
                    return View();
                }
                
            }

        }

        [AuthorizeParent]
        public ActionResult Edit(int? scheduleId)
        {

            if (scheduleId != null)
            {
                try
                {
                    //Se manda un dato de Data no de Modelo para usar el metodo Find
                    Schedule schedule = new Schedule();

                    var parent = this.GetCurrentUserInfo();
                    using (var db = new ParentalControlDBEntities())
                    {
                        schedule = db.Schedule.Find(scheduleId);
                    }
                    ViewBag.list = schedule;
                    return View(schedule);
                }
                catch (Exception ex)
                {

                    return RedirectToAction("Index");
                }
                
            }
            return RedirectToAction("Index");

        }

        [AuthorizeParent]
        [HttpPost]
        public ActionResult Edit( int scheduleId/*, int parentId*/, DateTime start, DateTime end)
        {
            var parent = this.GetCurrentUserInfo();
            //ScheduleModel scheduleModel = new ScheduleModel();
            //scheduleModel.ScheduleId = scheduleId;
            //scheduleModel.ScheduleStartTime = start;
            //scheduleModel.ScheduleEndTime = end;
            //scheduleModel.ParentId = parentId;
            //scheduleModel.ScheduleCreationDate = ScheduleCreationDate;

            if (string.IsNullOrEmpty(start.ToString())
                || string.IsNullOrEmpty(end.ToString())
                || string.IsNullOrEmpty(scheduleId.ToString()))
            {
                Alert("El registro presenta campos vacíos", NotificationType.warning);
                return View();
                
            }
            else
            {
                ScheduleModel scheduleModel = new ScheduleModel();
                scheduleModel.ScheduleId = scheduleId;
                scheduleModel.ScheduleStartTime = start;
                scheduleModel.ScheduleEndTime = end;

                
                if (scheduleModel.Validate(scheduleModel))
                {
                    //ScheduleModel scheduleUpdate = new ScheduleModel();
                    List<ScheduleModel> scheduleModeList = new List<ScheduleModel>();
                    scheduleModeList = ValidationSchedule(start, end, parent.Id);
                    if (scheduleModeList.Count > 0)
                    {

                        Alert("El registro ya existe", NotificationType.error);
                        return Redirect("Index");
                    }
                    else
                    {
                        
                        using (var db = new ParentalControlDBEntities())
                        {
                            Schedule scheduleUpdate = db.Schedule.Find(scheduleModel.ScheduleId);

                            scheduleUpdate.ScheduleStartTime = scheduleModel.ScheduleStartTime;
                            scheduleUpdate.ScheduleEndTime = scheduleModel.ScheduleEndTime;
                            db.Entry(scheduleUpdate).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        Alert("El registro se actualizó correctamente", NotificationType.success);
                        return Redirect("Index");
                    }
                }
                else
                {
                    Alert("La hora inicio debe ser MENOR a la hora final", NotificationType.warning);
                    Schedule sch = new Schedule();
                    sch.ScheduleId = scheduleModel.ScheduleId;
                    sch.ParentId = scheduleModel.ParentId;
                    sch.ScheduleStartTime = scheduleModel.ScheduleStartTime;
                    sch.ScheduleEndTime = scheduleModel.ScheduleEndTime;
                    return View(sch);
                }
                
            }

        }

        [AuthorizeParent]
        public ActionResult Delete(int? scheduleId)
        {
            Schedule schedule = new Schedule();
            if (scheduleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var db = new ParentalControlDBEntities())
            {
                schedule = db.Schedule.Find(scheduleId);
            }
            
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        [AuthorizeParent]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? scheduleId)
        {
            try
            {
                Schedule schedule = new Schedule();
                using (var db = new ParentalControlDBEntities())
                {
                    schedule = db.Schedule.Find(scheduleId);
                    if (schedule != null)
                    {
                        var apps = db.App.Where(x => x.ScheduleId == schedule.ScheduleId);
                        var deviceUse = db.DeviceUse.Where(x => x.ScheduleId == schedule.ScheduleId);
                        var devicePhoneUse = db.DevicePhoneUse.Where(x => x.ScheduleId == schedule.ScheduleId);
                        //Valida que se borre la llave foranea
                        db.App.RemoveRange(apps);
                        db.DevicePhoneUse.RemoveRange(devicePhoneUse);
                        db.DeviceUse.RemoveRange(deviceUse);
                        db.Schedule.Remove(schedule);
                        db.SaveChanges();
                    }

                }
                Alert("El registro se eliminó correctamente", NotificationType.success);
                return View();
            }
            catch (Exception e)
            {
                Alert("Ha ocurrido un error", NotificationType.error);
                return View();      
            }
        }
        [AuthorizeParent]
        [HttpGet]
        public ActionResult DeleteSchedule(int? scheduleId)
        {
            try
            {
                Schedule schedule = new Schedule();
                using (var db = new ParentalControlDBEntities())
                {
                    schedule = db.Schedule.Find(scheduleId);
                    if (schedule != null)
                    {
                        var apps = db.App.Where(x => x.ScheduleId == schedule.ScheduleId);
                        var deviceUse = db.DeviceUse.Where(x => x.ScheduleId == schedule.ScheduleId);
                        var devicePhoneUse = db.DevicePhoneUse.Where(x => x.ScheduleId == schedule.ScheduleId);
                        //Valida que se borre la llave foranea
                        db.App.RemoveRange(apps);
                        db.DevicePhoneUse.RemoveRange(devicePhoneUse);
                        db.DeviceUse.RemoveRange(deviceUse);
                        db.Schedule.Remove(schedule);
                        db.SaveChanges();
                    }

                }
                Alert("El registro se eliminó correctamente", NotificationType.success);
                
                return Redirect("Index");
            }
            catch (Exception e)
            {
                Alert("Ha ocurrido un error", NotificationType.error);
                return Redirect("Index");
            }
        }

        public List<ScheduleModel> ValidationSchedule(DateTime start, DateTime end, int parentId)
        {

            List<ScheduleModel> scheduleList = new List<ScheduleModel>();
            using (var db = new ParentalControlDBEntities())
            {
                scheduleList = (from schedule in db.Schedule
                                where schedule.ParentId == parentId
                                where schedule.ScheduleStartTime== start
                                where schedule.ScheduleEndTime== end
                                select new ScheduleModel
                                {
                                    ScheduleId = schedule.ScheduleId,
                                    ScheduleStartTime = schedule.ScheduleStartTime,
                                    ScheduleEndTime = schedule.ScheduleEndTime,
                                    ParentId = schedule.ParentId
                                }).ToList();
            }
            return scheduleList;

        }
    }
}