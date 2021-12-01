using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParentalControl.Web.Mvc.Controllers
{
    public class NewsController : BaseController
    {
        [AuthorizeParent]
        public ActionResult Index()
        {
            try
            {
                List<News> newsList = new List<News>();
                using (var db = new ParentalControlDBEntities())
                {
                    var query = (from news in db.News
                                select news).ToList();

                    if (query.Count > 0)
                    {
                        newsList.AddRange(query);
                    }
                }

                return View(newsList);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}