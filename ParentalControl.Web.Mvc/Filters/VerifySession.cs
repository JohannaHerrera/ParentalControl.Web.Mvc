using ParentalControl.Web.Mvc.Controllers;
using ParentalControl.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParentalControl.Web.Mvc.Filters
{
    public class VerifySession : ActionFilterAttribute
    {
        private ParentModel user;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);
                user = (ParentModel)HttpContext.Current.Session["User"];

                if (user == null)
                {
                    if (filterContext.Controller is AccountController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("/Account/Login");
                    }
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}