using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Display(Name = "Company")]
        [Index("WarehouseID_Company_Name_Index", 1, IsUnique = true)]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Display(Name = "WareHouse")]
        [Index("WarehouseID_Company_Name_Index", 2, IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(20, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(100, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Display(Name = "Departament")]
        public int DepartamentID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Display(Name = "City")]
        public int CityID { get; set; }

        //al declararse 1 Departament debemos declarar aqui es la clase virtual, werehouse 
        public virtual Departament Departament { get; set; }
        //al declararse 1 city debemos declarar aqui es la clase virtual, werehouse 
        public virtual City City { get; set; }
        //lado varios de User : (n) WareHouse -> Company (1)
        public virtual Company Company { get; set; }

    }
}