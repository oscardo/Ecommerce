using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Tax
    {
        [Key]
        public int TaxID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Index("Tax_CompanyID_Description_Index", 2, IsUnique = true)]
        [Display(Name = "Tax")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Rate")]
        [Range(0, 1, ErrorMessage = "The field {0} can take values between {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public double Rate { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Index("Tax_CompanyID_Description_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        //una compania para (n) n'umero Tax
        public virtual Company Company { get; set; }

        //al declarar 1 tax (taxes) en el sector de (n) Product (Products)
        public virtual ICollection<Product> Products { get; set; }

    }
}