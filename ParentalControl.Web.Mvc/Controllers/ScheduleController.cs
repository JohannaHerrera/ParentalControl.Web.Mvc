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
                ViewBag.list= scheduleList;
                return View(scheduleList);

            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}