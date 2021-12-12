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

        public string Femenino
        {
            get
            {
                return "Femenino";
            }
        }

        public string Masculino
        {
            get
            {
                return "Masculino";
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

        public int WebConfiguration
        {
            get
            {
                return 1;
            }
        }

        public int AppConfiguration
        {
            get
            {
                return 2;
            }
        }

        public int DeviceConfiguration
        {
            get
            {
                return 3;
            }
        }

        public int RequestStateApproved
        {
            get
            {
                return 1;
            }
        }

        public int RequestStateDisapproved
        {
            get
            {
                return 2;
            }
        }
    }
}