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


        public DbSet<Departament> Departaments { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tax> Taxes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public System.Data.Entity.DbSet<Ecommerce.Models.Customer> Customers { get; set; }
    }
}