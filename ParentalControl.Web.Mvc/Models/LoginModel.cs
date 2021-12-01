using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class LoginModel
    {
        //Correo
        public string ParentEmail { get; set; }
        //Contraseña
        public string ParentPassword { get; set; }
    }
}