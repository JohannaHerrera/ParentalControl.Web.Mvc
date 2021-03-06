using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class LoginModel
    {
        //Correo
        [Required(ErrorMessage = "Por favor, ingresa tu correo.")]
        [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
        public string ParentEmail { get; set; }

        //Contraseña
        [Required(ErrorMessage = "Por favor, ingresa tu contraseña.")]
        public string ParentPassword { get; set; }
    }
}