using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ParentalControl.Web.Mvc.Controllers
{
    public class ScheduleController : BaseController
    {
        //List Schedule
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
        
        public ActionResult Create()
        {
            var parent = this.GetCurrentUserInfo();
            return View();
        }
        [HttpPost]
        public ActionResult Create(ScheduleModel scheduleModel)
        {
            var creationDate = DateTime.Now;
            var parent = this.GetCurrentUserInfo();
            if (scheduleModel != null)
            {
                if (ModelState.IsValid)
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
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
    }
}