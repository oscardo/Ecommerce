using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Models
{
    [Authorize(Roles ="User")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Index("Product_CompanyID_Description_Index", 1, IsUnique = true)]
        [Index("Product_CompanyID_BarCode_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Index("Product_CompanyID_Description_Index", 2, IsUnique = true)]
        [Display(Name = "Product")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(13, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        [Index("Product_CompanyID_BarCode_Index", 2, IsUnique = true)]
        [Display(Name = "Bar Code")]
        public string BarCode { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "you must select a {0}")]
        [Display(Name = "Tax")]
        public int TaxID { get; set; }
        
        [Required(ErrorMessage = "The field {0} is required")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} can take values between {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string Image { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(800, ErrorMessage = "The field {0} must be maximum {1} Character length")]
        public string Remark { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock
        { get
            {
                return Inventories.Sum(i => i.Stock);
            }
        }

        //una compania para (n) n'umero Product
        public virtual Company Company { get; set; }

        //una category para (n) n'umero Product
        public virtual Category Category { get; set; }

        //un tax para (n) n'umero Product
        public virtual Tax Tax { get; set; }

        //muchos productos pueden tener un solo inventario
        public virtual ICollection<Inventory> Inventories { get; set; }

    }
}