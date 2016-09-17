using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Collections;

namespace Ecommerce.Classes
{
    public class CombosHelper : IDisposable
    {
        private static EcommerceContext db = new EcommerceContext();

        public static List<Departament> GetDepartament()
        {
            var Departaments = db.Departaments.ToList();
            Departaments.Add(new Departament
            {
                DepartamentID = 0,
                Name = "[Select a Departament]"
            });
            return Departaments.OrderBy(d => d.Name).ToList();
        }

        public static List<City> GetCities()
        {
            var cities = db.Cities.ToList();
            cities.Add(new City
            {
                CityID = 0,
                Name = "[Select a City]",
            });
            return cities.OrderBy(d => d.Name).ToList();
        }

        public static List<Company> GetCompanies()
        {
            var companies = db.Companies.ToList();
            companies.Add(new Company
            {
                CompanyID = 0,
                Name = "[Select a Company]",
            });
            return companies.OrderBy(c => c.Name).ToList();
        }

        public static List<Category> GetCategories(int? CompanyID)
        {
            var categories = db.Categories.Where(c => c.CompanyID == CompanyID).ToList();
            categories.Add(new Category
            {
                CategoryID = 0,
                Description = "[Select a Category]",
            });
            return categories.OrderBy(c => c.Description).ToList();
        }

        public static List<Customer> GetCustomers(int CompanyID)
        {
            var customer = db.Customers.Where(t => t.CompanyID == CompanyID).ToList();
            customer.Add(new Customer
            {
                CustomerID = 0,
                FirstName = "[Select a Customer]",
            });
            return customer.OrderBy(c => c.FirstName).ThenBy(c => c.LastName).ToList();
        }

        public static List<Product> GetProducts(int companyID)
        {
            var product = db.Products.Where(p => p.CompanyID == companyID).ToList();
            product.Add(new Product
            {
                ProductID = 0,
                Description = "[Select a Product]",
            });
            return product.OrderBy(p => p.Description).ToList();
        }

        public static List<Tax> GetTaxes(int? CompanyID)
        {
            var taxes = db.Taxes.Where(t => t.CompanyID == CompanyID).ToList();
            taxes.Add(new Tax
            {
                TaxID = 0,
                Description = "[Select a tax]",
            });
            return taxes.OrderBy(c => c.Description).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}