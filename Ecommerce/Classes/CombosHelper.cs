﻿using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

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

        public void Dispose()
        {
            db.Dispose();
        }
    }
}