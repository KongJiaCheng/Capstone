using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FinalYearProject.Models
{
    public class ProductRespitory
    {
        private readonly string connectionString;

        public ProductRespitory(string connString)
        {
            connectionString = connString;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = new List<Product>();
            string query = "SELECT ProductID, ProductName, ProductDescription, Price, Stock, ImageURL FROM product";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = rdr.GetInt32(0),
                                ProductName = rdr.GetString(1),
                                ProductDescription = rdr.GetString(2),
                                Price = rdr.GetDecimal(3),
                                Stock = rdr.GetInt32(4),
                                ImageURL = rdr.GetString(5)
                            });
                        }
                    }
                }
            }

            return products;
        }

    }
}