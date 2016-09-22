﻿using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class AddProductView
    {

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Product", Prompt = "[Select a product...]")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "You must enter greater than {1} values in {0}")]
        public double Quantity { get; set; }
        
    }
}