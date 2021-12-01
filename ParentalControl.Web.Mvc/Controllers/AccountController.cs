using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.Models;

namespace ParentalControl.Web.Mvc.Controllers
{
    public class AccountController : BaseController
    {       
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            try
            {
                using (var db = new ParentalControlDBEntities())
                {
                    var loginParent = (from parent in db.Parent
                                       where parent.ParentEmail == loginModel.ParentEmail
                                       && parent.ParentPassword == loginModel.ParentPassword
                                       select parent).FirstOrDefault();

                    if (loginParent != null)
                    {
                        ParentModel parentModel = new ParentModel();
                        parentModel.ParentId = loginParent.ParentId;
                        parentModel.ParentUsername = loginParent.ParentUsername;
                        parentModel.ParentEmail = loginParent.ParentEmail;
                        Session["User"] = parentModel;

                        this.SetCurrentUserInfo(parentModel);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Credenciales incorrectas.";
                        return View();
                    }
                }

                return RedirectToAction("Index", "News");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(ParentModel parentModel)
        {
            return View();
        }
    }
}