using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class EcommerceContext: DbContext
    {
        public EcommerceContext():base("DBECommerce")
        {
        }

        public System.Data.Entity.DbSet<Ecommerce.Models.Departament> Departaments { get; set; }

        public System.Data.Entity.DbSet<Ecommerce.Models.City> Cities { get; set; }
    }
}