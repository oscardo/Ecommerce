using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Departament
    {
        [Key]
        public int DepartamentID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Display(Name = "Departament")]
        public string Name { get; set; }

        //al declararse 1 Departament debemos declarar aqui es la clase virtual
        public virtual ICollection<City> Cities { get; set; }
    }
}