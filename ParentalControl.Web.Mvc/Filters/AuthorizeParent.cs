using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParentalControl.Web.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeParent : AuthorizeAttribute
    {
        private ParentModel parentUser;
        private ParentalControlDBEntities db = new ParentalControlDBEntities();

        public AuthorizeParent()
        {

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                parentUser = (ParentModel)HttpContext.Current.Session["User"];

                if(parentUser == null)
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}