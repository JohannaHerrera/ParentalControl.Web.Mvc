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
            var parent = this.GetCurrentUserInfo();
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
            if (scheduleModel != null)
            {

                if (string.IsNullOrEmpty(scheduleModel.ScheduleStartTime.ToString())
                    || string.IsNullOrEmpty(scheduleModel.ScheduleEndTime.ToString()))
                {
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
                    Response.Write("<script>return alert('¡Horario Añadido!');</script>");
                }
            }
            return RedirectToAction("Index");
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
        public ActionResult Edit( int scheduleId, int parentId, DateTime ScheduleCreationDate, DateTime start, DateTime end)
        {
            var parent = this.GetCurrentUserInfo();
            ScheduleModel scheduleModel = new ScheduleModel();
            scheduleModel.ScheduleId = scheduleId;
            scheduleModel.ScheduleStartTime = start;
            scheduleModel.ScheduleEndTime = end;
            scheduleModel.ParentId = parentId;
            scheduleModel.ScheduleCreationDate = ScheduleCreationDate;
            if (scheduleModel != null)
            {

                if (string.IsNullOrEmpty(scheduleModel.ScheduleStartTime.ToString())
                    || string.IsNullOrEmpty(scheduleModel.ScheduleEndTime.ToString())
                    || string.IsNullOrEmpty(scheduleModel.ScheduleId.ToString()))
                {
                    return View();
                }
                else
                {
                    //ScheduleModel scheduleUpdate = new ScheduleModel();
                    Schedule scheduleUpdate = new Schedule();
                    scheduleUpdate.ScheduleId = scheduleModel.ScheduleId;
                    scheduleUpdate.ParentId = scheduleModel.ParentId;
                    scheduleUpdate.ScheduleCreationDate = scheduleModel.ScheduleCreationDate;
                    scheduleUpdate.ScheduleStartTime = scheduleModel.ScheduleStartTime;
                    scheduleUpdate.ScheduleEndTime = scheduleModel.ScheduleEndTime;


                    using (var db = new ParentalControlDBEntities())
                    {
                        db.Entry(scheduleUpdate).State= EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    Response.Write("<script>return alert('¡Horario Modificado!');</script>");
                     View();
                }
                return RedirectToAction("Index");
            }
            return View();
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
            Schedule schedule = new Schedule();
            using (var db = new ParentalControlDBEntities())
            {
                schedule = db.Schedule.Find(scheduleId);
                db.Schedule.Remove(schedule);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}