using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class CompanyCustomer
    {
        [Key]
        public int CompanyCustomerID { get; set; }

        public int CompanyID { get; set; }

        public int CustomerID { get; set; }

        public virtual Company Company { get; set; }

        public virtual Customer Customer { get; set; }

    }
}