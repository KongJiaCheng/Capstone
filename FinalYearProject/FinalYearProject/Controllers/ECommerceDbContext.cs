using FinalYearProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalYearProject.Controllers
{
    public class ECommerceDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Use the connection string from Web.config
                optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ECommerceDbContext"].ConnectionString);
            }
        }

        public DbSet<Product> Products { get; set; }
    }
}