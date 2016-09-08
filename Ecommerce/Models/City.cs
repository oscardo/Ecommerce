using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Index("City_Name_Index", 2, IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Index("City_Name_Index", 1, IsUnique = true)]
        public int DepartamentID { get; set; }

        //al declararse n city debemos declarar aqui es la clase virtual
        public virtual Departament Departament { get; set; }

        //al declarar 1 compania en el sector de (n) Companies
        public virtual ICollection<Company> Companies { get; set; }

        //al declarar 1 ciudad (City) en el sector de (n) Usuarios (Users)
        public virtual ICollection<User> Users { get; set; }
        
    }
}