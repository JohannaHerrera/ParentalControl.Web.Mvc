using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class ParentModel
    {
        // Id del Padre
        public int ParentId { get; set; }
        // Nombre de usuario
        public string ParentUsername { get; set; }
        // Correo
        public string ParentEmail { get; set; }
        // Contraseña
        public string ParentPassword { get; set; }
        // Fecha de Creación
        public DateTime ParentCreationDate { get; set; }
    }
}