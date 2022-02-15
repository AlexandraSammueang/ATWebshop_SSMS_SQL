using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATWebshop_SQL.Models;
using Dapper;
using System.Data.SqlClient;

namespace ATWebshop_SQL
{

    class Querys
    {
        public string ProductName { get; set; }
        public double Price { get; set; }

        static string connString = "data source=.\\; initial catalog = ATWebshop; persist security info = True; Integrated Security = True;";
        
        //1.BestSellingProducts
        public static void PrintTopProducts()
        {
            var topProducts = new List<Querys>();
            var sql = @"select top 3 products.ProductName,products.price, sum(quantity) as 'Quantity'
                        from OrderDetails
                        join products on products.Id = OrderDetails.ProductId
                        group by ProductName, products.price
                        order by quantity desc";

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                topProducts = connection.Query<Querys>(sql).ToList();
            }
            Console.WriteLine("Top 3 products: \n ");

            foreach (var top in topProducts)
            {
                Console.WriteLine($"{top.ProductName,-25} {top.Price}kr");
            }
            Console.WriteLine();
        }


    }
}
