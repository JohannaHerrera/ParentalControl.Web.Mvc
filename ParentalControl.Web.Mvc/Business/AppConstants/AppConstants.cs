using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Business.AppConstants
{
    public class AppConstants
    {
        /// <summary>
        /// Nombre de la sesión que contiene la información del usuario actual
        /// </summary>
        public static string CurrentUser
        {
            get
            {
                return "CurrentUser";
            }
        }

        public string NoProtected
        {
            get
            {
                return "No Protegido";
            }
        }

        public bool Access
        {
            get
            {
                return false;
            }
        }
    }
}