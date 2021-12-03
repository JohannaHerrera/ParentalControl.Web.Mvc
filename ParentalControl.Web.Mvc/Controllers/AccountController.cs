﻿using System;
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
                            Response.Write("<script>alert('Credenciales incorrectas.');</script>");
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
                            Response.Write("<script>alert('Ya existe un usuario con el mismo correo electrónico.');</script>");
                            return View();
                        }
                    }

                    Response.Write("<script>return alert('¡Usuario Registrado!');</script>");
                    return View();
                }                
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}