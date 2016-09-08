using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class EcommerceContext: DbContext
    {
        public EcommerceContext():base("DBECommerce")
        {
        }

        //• DISABLE CASCADE DELETING RULE
        //Add this method in the context database class:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public System.Data.Entity.DbSet<Ecommerce.Models.Departament> Departaments { get; set; }

        public System.Data.Entity.DbSet<Ecommerce.Models.City> Cities { get; set; }

        public System.Data.Entity.DbSet<Ecommerce.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<Ecommerce.Models.User> Users { get; set; }
    }
}