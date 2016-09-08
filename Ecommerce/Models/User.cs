using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(256, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Display(Name = "E-Mail")]
        [Index("User_Name_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(20, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(100, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        public string Address { get; set; }

     
        [NotMapped]
        public HttpPostedFileBase PhotoFile { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Display(Name = "Departament")]
        public int DepartamentID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Display(Name = "City")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        //al declararse 1 Departament debemos declarar aqui es la clase virtual
        public virtual Departament Departament { get; set; }
        //al declararse 1 city debemos declarar aqui es la clase virtual
        public virtual City City { get; set; }
        //lado varios de User : (n) User -> Company (1)
        public virtual Company Company { get; set; }
    }
}