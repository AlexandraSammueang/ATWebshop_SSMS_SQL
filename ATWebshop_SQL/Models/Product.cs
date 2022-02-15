using System;
using System.Collections.Generic;
using System.Linq;
using ATWebshop_SQL.Models;
using Dapper;
using System.Data.SqlClient;

#nullable disable

namespace ATWebshop_SQL
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int Id { get; set; }
        public int? CategoriesId { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
        public string ProductInfo { get; set; }

        public virtual Category Categories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        static string connString = "data source=.\\; initial catalog = ATWebshop; persist security info = True; Integrated Security = True;";
        public static List<Product> GetAllProducts()
        {

            var products = new List<Product>();
            var sql = "SELECT * FROM products";

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                products = connection.Query<Product>(sql).ToList();
            }

            return products;
        }
    }
}
