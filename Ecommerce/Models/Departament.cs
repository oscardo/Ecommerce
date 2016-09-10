using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Index("Departament_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        //al declararse 1 Departament debemos declarar aqui es la clase virtual
        public virtual ICollection<City> Cities { get; set; }

        //al declarar 1 compania en el sector de (n) Companies
        public virtual ICollection<Company> Companies { get; set; }

        //al declarar 1 departament (departament) en el sector de (n) Usuarios (Users)
        public virtual ICollection<User> Users { get; set; }

        //al declarar 1 Deparment (Deparment) en el sector de (n) WareHouse (Products)
        public virtual ICollection<Warehouse> Warehouses { get; set; }

        //al declarar 1 Deparment (Deparment) en el sector de (n) Customer (Customers)
        public virtual ICollection<Customer> Customers { get; set; }

    }
}