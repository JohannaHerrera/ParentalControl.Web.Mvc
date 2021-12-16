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
        [Required]
        [StringLength(100)]
        [Display(Name ="Nombre:")]
        public string InfantName { get; set; }
        [Required]
        [Display(Name = "Género")]
        public string InfantGender { get; set; }
        public DateTime InfantCreationDate { get; set; }
        public int ParentId { get; set; }
    }
}