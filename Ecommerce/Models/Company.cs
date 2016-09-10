
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Display(Name = "Company")]
        [Index("Company_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(20, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(100, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [NotMapped]
        public HttpPostedFileBase LogoFile { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        public int DepartamentID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        public int CityID { get; set; }

        //al declararse 1 Departament debemos declarar aqui es la clase virtual
        public virtual Departament Departament { get; set; }
        //al declararse 1 city debemos declarar aqui es la clase virtual
        public virtual City City { get; set; }

        //al declarar 1 compania (company) en el sector de (n) Usuarios (Users)
        public virtual ICollection<User> Users { get; set; }

        //al declarar 1 compania (company) en el sector de (n) Category (Categories)
        public virtual ICollection<Category> Categories { get; set; }

        //al declarar 1 compania (company) en el sector de (n) Tax (Taxes)
        public virtual ICollection<Tax> Taxes { get; set; }

        //al declarar 1 compania (company) en el sector de (n) Product (Products)
        public virtual ICollection<Product> Products { get; set; }
        
        //al declarar 1 compania (company) en el sector de (n) WareHouse (Products)
        public virtual ICollection<Warehouse> Warehouses { get; set; }

        //al declarar 1 Company (Company) en el sector de (n) Customer (Customers)
        public virtual ICollection<Customer> Customers { get; set; }
    }
}