using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Display(Name = "City")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public int DepartamentID { get; set; }

        //al declararse n city debemos declarar aqui es la clase virtual
        public virtual Departament Departament { get; set; }


    }
}