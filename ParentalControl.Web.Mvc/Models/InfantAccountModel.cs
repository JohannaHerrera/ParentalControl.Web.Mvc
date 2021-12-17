using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class InfantAccountModel
    {
        public int InfantAccountId { get; set; }
        [Required(ErrorMessage = "Por favor, ingrese el nombre del infante.")]
        [StringLength(100)]
        [Display(Name ="Nombre:")]
        public string InfantName { get; set; }
        [Required(ErrorMessage = "Por favor, seleccione el género del infante.")]
        [Display(Name = "Género")]
        public string InfantGender { get; set; }
        public DateTime InfantCreationDate { get; set; }
        public int ParentId { get; set; }
    }
}