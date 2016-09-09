using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Index("Category_CompanyID_Description_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Index("Category_CompanyID_Description_Index", 2, IsUnique = true)]
        [Display(Name = "Category")]
        public string Description { get; set; }
        
        public virtual Company Company { get; set; }


    }
}