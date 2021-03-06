using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.Filters;
using ParentalControl.Web.Mvc.Models;
using static ParentalControl.Web.Mvc.Models.Enum;

namespace ParentalControl.Web.Mvc.Controllers
{
    public class AccountController : BaseController
    {       
        public ActionResult Login()
        {
            Session["User"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            try
            {
                if (string.IsNullOrEmpty(loginModel.ParentEmail) 
                    || string.IsNullOrEmpty(loginModel.ParentPassword)
                    || !loginModel.ParentEmail.Contains("@") 
                    || !loginModel.ParentEmail.Contains("."))
                {
                    return View();
                }
                else
                {
                    using (var db = new ParentalControlDBEntities())
                    {
                        var loginParent = (from parent in db.Parent
                                           where parent.ParentEmail == loginModel.ParentEmail
                                           && parent.ParentPassword == loginModel.ParentPassword
                                           select parent).FirstOrDefault();

                        if (loginParent != null)
                        {
                            Parent parentModel = new Parent();
                            parentModel.ParentId = loginParent.ParentId;
                            parentModel.ParentUsername = loginParent.ParentUsername;
                            parentModel.ParentEmail = loginParent.ParentEmail;
                            Session["User"] = parentModel;

                            this.SetCurrentUserInfo(parentModel);
                        }
                        else
                        {
                            Alert("Credenciales incorrectas.", NotificationType.error);
                            return View();
                        }
                    }

                    return RedirectToAction("Index", "News");
                }
                
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Register()
        {
            Session["User"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Register(ParentModel parentModel)
        {
            try
            {
                if (string.IsNullOrEmpty(parentModel.ParentEmail) || string.IsNullOrEmpty(parentModel.ParentPassword)
                    || string.IsNullOrEmpty(parentModel.ParentUsername.Trim()) 
                    || !parentModel.ParentEmail.Contains("@") 
                    || !parentModel.ParentEmail.Contains("."))
                {
                    return View();
                }
                else
                {
                    using (var db = new ParentalControlDBEntities())
                    {
                        var verifyUser = (from parent in db.Parent
                                          where parent.ParentEmail == parentModel.ParentEmail.Trim()
                                          select parent).FirstOrDefault();

                        if (verifyUser == null)
                        {
                            Parent parentRegister = new Parent();
                            parentRegister.ParentUsername = parentModel.ParentUsername.Trim();
                            parentRegister.ParentPassword = parentModel.ParentPassword;
                            parentRegister.ParentEmail = parentModel.ParentEmail.Trim();
                            parentRegister.ParentCreationDate = DateTime.Now;
                            db.Parent.Add(parentRegister);
                            db.SaveChanges();
                            
                        }
                        else
                        {
                            Alert("Ya existe un usuario con el mismo correo electrónico.", NotificationType.error);
                            return View();
                        }
                    }

                    Alert("¡Usuario Registrado!", NotificationType.success);
                    ModelState.Clear();
                    return Redirect("Login");
                }                
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AuthorizeParent]
        public ActionResult MyProfile()
        {
            try
            {
                Parent parentdb = new Parent();
                var parent = this.GetCurrentUserInfo();

                using (var db = new ParentalControlDBEntities())
                {
                    parentdb = db.Parent.Find(parent.Id);
                }
                
                ParentModel parentModel = new ParentModel();
                ViewBag.idParent = parent.Id;
                parentModel.ParentUsername = parentdb.ParentUsername;
                parentModel.ParentEmail = parentdb.ParentEmail;

                return View(parentModel);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [AuthorizeParent]
        public ActionResult EditProfile(int? parentId)
        {
            if (parentId != null)
            {
                try
                {
                    ParentModel parentModel = new ParentModel();

                    Parent parentDB = new Parent();

                    using (var db = new ParentalControlDBEntities())
                    {
                        var parentdb = db.Parent.Where(x => x.ParentId == parentId).FirstOrDefault();
                        parentModel.ParentUsername = parentdb.ParentUsername;
                        parentModel.ParentEmail = parentdb.ParentEmail;
                        parentModel.ParentPassword = parentdb.ParentPassword;
                        return View(parentModel);
                    }
                }
                catch (Exception ex)
                {

                    return RedirectToAction("MyProfile");
                }

            }
            return RedirectToAction("MyProfile");
        }

        [AuthorizeParent]
        [HttpPost]
        public ActionResult EditProfile(string name, string email, string password)
        {
            var parent = this.GetCurrentUserInfo();
            ParentModel parentModel = new ParentModel();
            parentModel.ParentUsername = name;
            parentModel.ParentEmail = email;
            parentModel.ParentPassword = password;

            var parentInfo = this.GetCurrentUserInfo();

            if (string.IsNullOrEmpty(parentModel.ParentUsername)
                || string.IsNullOrEmpty(parentModel.ParentEmail)
                || string.IsNullOrEmpty(parentModel.ParentPassword))
            {
                Alert("El registro contiene campos vacíos", NotificationType.warning);
                return View(parentModel);
            }
            else
            {
                if(!parentModel.ParentEmail.Contains("@") || !parentModel.ParentEmail.Contains("."))
                {
                    Alert("Ingrese una dirección de correo válida", NotificationType.warning);
                    return View(parentModel);
                }
                else
                {
                    using (var db = new ParentalControlDBEntities())
                    {
                        Parent parentUpdate = db.Parent.Find(parentInfo.Id);
                        parentUpdate.ParentUsername = parentModel.ParentUsername;
                        parentUpdate.ParentEmail = parentModel.ParentEmail;
                        parentUpdate.ParentPassword = parentModel.ParentPassword;
                        db.Entry(parentUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    Alert("El registro se actualizó correctamente", NotificationType.success);
                    return Redirect("MyProfile");
                }
            }
        }
    }
}