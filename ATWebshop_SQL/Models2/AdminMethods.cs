using ATWebshop_SQL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
namespace ATWebshop_SQL
{

    class AdminMethods
    {

        static string connString = "data source=.\\; initial catalog = ATWebshop; persist security info = True; Integrated Security = True;";
        public static void AddProduct()
        {
            bool keepAsking = true;
            int affectedRows = 0;

            do
            {
                using (var database = new ATWebshopContext())
                {
                    var productList = database.Products;
                    foreach (var product in productList)
                    {
                        Console.WriteLine($"{product.Id}  {product.ProductName,-30}  {product.Price,-50}");
                    }
                }

                Console.WriteLine("\nDo you want to add a product ?");
                string ask = Console.ReadLine();
                if (ask.ToLower() == "yes")
                {
                    Console.Write("\nEnter the following information about the product: ");

                    Console.Write("\nProduct name:  ");
                    var productName = Console.ReadLine();

                    Console.Write("Category ID:   ");
                    var sProductCategory = Console.ReadLine();
                    try
                    {
                        int iProductCategory = Int32.Parse(sProductCategory);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Incorrect answer: '{sProductCategory}'. Please try again.");
                    }
                    Console.Write("Price:  ");
                    var sProductPrice = Console.ReadLine();
                    try
                    {
                        double iProductPrice = double.Parse(sProductPrice);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Incorrect answer: '{sProductPrice}'. Please try again.");
                    }
                    Console.WriteLine("\nProduct information (max 250 characters):   ");
                    string productInfo = Console.ReadLine();

                    using (var database = new ATWebshopContext())
                    {
                        var newProduct = new Product
                        {
                            ProductName = productName,
                            CategoriesId = Convert.ToInt32(sProductCategory),
                            Price = Convert.ToDouble(sProductPrice),
                            ProductInfo = productInfo
                        };

                        database.Add(newProduct);
                        database.SaveChanges();
                        affectedRows++;
                    }
                }


                else if (ask.ToLower() == "no")
                {
                    keepAsking = false;
                    Console.WriteLine($"\nTotal number of products added: {affectedRows}\n");
                    Menu.AskIfBackToMainMenu();
                }

            }
            while (keepAsking);

        }

        public static void AddKategori()
        {
            int affectedRows = 0;
            bool keepAsking = true;

            do
            {
                Console.WriteLine("Do you want to add a category?");
                string ask = Console.ReadLine();
                Console.WriteLine();

                if (ask.ToLower() == "yes")
                {
                    var sql = "SELECT * FROM Categories";
                    var cat = new List<Category>();

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        cat = connection.Query<Category>(sql).ToList();
                    }

                    foreach (var category in cat)
                    {
                        Console.WriteLine($"{category.Id} {category.Categories,-20}");
                    }

                    Console.WriteLine("\n Add a category:        ");
                    string answerCategory = Console.ReadLine();

                    sql = $"insert into dbo.Categories Values('{answerCategory}')";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        cat = connection.Query<Category>(sql).ToList();
                        affectedRows++;

                    }

                    Console.WriteLine();
                    foreach (var category1 in cat)
                    {
                        Console.WriteLine($"{category1.Id} New category: {category1.Categories,-20} ");
                    }
                    Console.WriteLine();
                }
                else if (ask.ToLower() == "no")
                {
                    keepAsking = false;
                    Console.WriteLine($"\nNumber of added categories: {affectedRows}");
                }
            } while (keepAsking);

        }

        public static void RemoveProduct()
        {
            bool keepAsking = true;
            int affectedRows1 = 0;
            do
            {
                Console.WriteLine("\nDo you want to delete a product?");
                string ask = Console.ReadLine();
                if (ask.ToLower() == "yes")
                {
                    Console.WriteLine("\nWhich item do you want to remove?");
                    using (var db = new ATWebshopContext())
                    {
                        var products = db.Products;
                        foreach (var prod in products)
                        {
                            Console.WriteLine($"{prod.Id} {prod.ProductName,-20} {prod.Price - 30} {prod.ProductInfo,-40}  {prod.Categories,-50}");
                        }

                    }

                    using (var db = new ATWebshopContext())
                    {
                        var removePost = db.Products.SingleOrDefault(c => c.Id == Convert.ToInt32(Console.ReadLine()));
                        db.Products.Remove(removePost);
                        db.SaveChanges();
                        affectedRows1++;

                    }
                    Menu.BackToMainMenu();

                }
                else if (ask.ToLower() == "no")
                {
                    keepAsking = false;
                    Console.WriteLine($"Affected rows {affectedRows1}");
                    Menu.AskIfBackToMainMenu();
                }
            } while (keepAsking);

        }

        public static void UpdatePrice()
        {
            int affectedRows = 0;
            bool keepAsking = true;

            do
            {
                Console.WriteLine("Do you want to change the price of an item?");
                string ask = Console.ReadLine();
                Console.WriteLine();

                if (ask.ToLower() == "yes")
                {

                    var sql = "SELECT * FROM products";
                    var products = new List<Product>();

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                    }

                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} {prod.ProductName,-20} {prod.Price,-40}");
                    }

                    Console.Write("\nWhich line do you want to update?: ");
                    string rowNumber = Console.ReadLine();
                    Convert.ToInt32(rowNumber);

                    Console.Write("\nWhat price do you want to set: ? ");
                    string newPrice = Console.ReadLine();
                    string convertedNewPrice = newPrice.Replace(',', '.');
                    sql =
                                            $"UPDATE Products " +
                                            $"  SET Price = CAST('{convertedNewPrice}' as float) " +
                                            $"WHERE ID = {rowNumber}";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();

                    }


                    sql =
                        $"SELECT * FROM products " +
                        $" WHERE id = {rowNumber}";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                        affectedRows++;
                    }
                    Console.WriteLine();
                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} {prod.ProductName,-20} New price: {prod.Price,-40}");
                    }
                    Console.WriteLine();
                }
                else if (ask.ToLower() == "no")
                {
                    keepAsking = false;
                    Console.WriteLine($"\nNumber of price changes: {affectedRows}");
                    Menu.AskIfBackToMainMenu();
                }
            } while (keepAsking);

        }

        public static void UpdateProductName()
        {
            int affectedRows = 0;
            bool keepAsking = true;

            do
            {
                Console.WriteLine("Do you want to change the name of an item?");
                string ask = Console.ReadLine();
                Console.WriteLine();

                if (ask.ToLower() == "yes")
                {
                    var sql = "SELECT * FROM products";
                    var products = new List<Product>();

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                    }

                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} {prod.ProductName,-20} {prod.Price,-40}");
                    }

                    Console.Write("\nWhich line do you want to update ?: ");
                    string rowNumber = Console.ReadLine();
                    Convert.ToInt32(rowNumber);

                    Console.Write("\nWhat name do you want to enter ?: ");
                    string newProductName = Console.ReadLine();

                    sql =
                                            $"UPDATE Products " +
                                            $"  SET ProductName = '{newProductName}' " +
                                            $"WHERE ID = {rowNumber}";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();

                    }


                    sql =
                        $"SELECT * FROM products " +
                        $" WHERE id = {rowNumber}";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                        affectedRows++;
                    }
                    Console.WriteLine();
                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} New product name: {prod.ProductName,-20}");
                    }
                    Console.WriteLine();
                }
                else if (ask.ToLower() == "no")
                {
                    keepAsking = false;
                    Console.WriteLine($"\nNumber of changed product names: {affectedRows}");
                    Menu.AskIfBackToMainMenu();
                }
            } while (keepAsking);


            int affectedRows1 = 0;
            bool keepAsking2 = true;

            do
            {
                Console.WriteLine("Do you also want to change the description of the product?");
                string ask = Console.ReadLine();
                Console.WriteLine();

                if (ask.ToLower() == "yes")
                {
                    var sql = "SELECT * FROM products";
                    var products = new List<Product>();

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                    }

                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} {prod.ProductName,-20} {prod.Price,-40} {prod.ProductInfo,-50}");
                    }

                    Console.Write("\nWhich line do you want to update?: ");
                    string rowNumber = Console.ReadLine();
                    Convert.ToInt32(rowNumber);

                    Console.WriteLine("Change description of an item (max. 250 characters):           ");
                    string changetext = Console.ReadLine();

                    sql =
                                            $"UPDATE Products " +
                                            $"  SET ProductInfo = '{changetext}' " +
                                            $"WHERE ID = {rowNumber}";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();

                    }

                    sql =
                         $"SELECT * FROM products " +
                         $" WHERE id = {rowNumber}";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                        affectedRows1++;
                    }
                    Console.WriteLine();
                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} {prod.ProductName,-20} {prod.Price,-40}\n New product description: {prod.ProductInfo,-50} ");

                    }

                }
                else if (ask.ToLower() == "no")
                {

                    keepAsking2 = false;
                    Console.WriteLine($"\nNumber of changed product description: {affectedRows1}");
                    Menu.AskIfBackToMainMenu();

                }
            } while (keepAsking2);


        }

        public static void Products()
        {
            var sql = "SELECT * FROM categories";
            var categories = new List<Category>();

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                categories = connection.Query<Category>(sql).ToList();
            }

            Console.WriteLine("\nChoose a category:");
            foreach (var cat in categories)
            {
                Console.WriteLine($"{cat.Id,-5} {cat.Categories,-20}");
            }

            Console.WriteLine($"\n 6 - Back to main page");

            string sInput = Console.ReadLine();

            switch (Convert.ToInt32(sInput))
            {
                case 1:
                    {
                        Console.WriteLine("Choose a product\n");

                        var productList = Product.GetAllProducts();

                        sql = @$"select products.id, products.ProductName, Products.Price
                                from Products
                                join Categories on products.CategoriesId = Categories.id
                               where Products.CategoriesId = 1";

                        using (var connection = new SqlConnection(connString))
                        {
                            connection.Open();
                            productList = connection.Query<Product>(sql).ToList();
                        }
                        foreach (var prod in productList)
                        {
                            Console.WriteLine($"{prod.Id,-5} {prod.ProductName,-20} {prod.Price,-40}");
                        }
                        CustomerMethods.ShowProductDetails();

                        Menu.AskIfBackToMainMenu();
                    }
                    break;

                case 2:
                    {

                        Console.WriteLine("Choose a product\n");

                        var productList = new List<Product>();

                        sql = @$"select products.id, products.ProductName, Products.Price
                                from Products
                                join Categories on products.CategoriesId = Categories.id
                               where Products.CategoriesId = 2";

                        using (var connection = new SqlConnection(connString))
                        {
                            connection.Open();
                            productList = connection.Query<Product>(sql).ToList();
                        }
                        foreach (var prod in productList)
                        {
                            Console.WriteLine($"{prod.Id,-5} {prod.ProductName,-20} {prod.Price,-40}");
                        }

                        Console.WriteLine("Choose a product to see more about:    ");
                        CustomerMethods.ShowProductDetails();
                        Menu.AskIfBackToMainMenu();
                    }
                    break;


                case 3:
                    {
                        Console.WriteLine("Choose a product\n");

                        var productList = new List<Product>();

                        sql = @$"select products.id, products.ProductName, Products.Price
                                from Products
                                join Categories on products.CategoriesId = Categories.id
                               where Products.CategoriesId = 3";

                        using (var connection = new SqlConnection(connString))
                        {
                            connection.Open();
                            productList = connection.Query<Product>(sql).ToList();
                        }
                        foreach (var prod in productList)
                        {
                            Console.WriteLine($"{prod.Id,-5} {prod.ProductName,-20} {prod.Price,-40}");
                        }
                        CustomerMethods.ShowProductDetails();

                        Menu.AskIfBackToMainMenu();

                    }
                    break;

                case 4:
                    {
                        Console.WriteLine("Choose a product\n");

                        var productList = new List<Product>();

                        sql = @$"select products.id, products.ProductName, Products.Price
                                from Products
                                join Categories on products.CategoriesId = Categories.id
                               where Products.CategoriesId = 4";

                        using (var connection = new SqlConnection(connString))
                        {
                            connection.Open();
                            productList = connection.Query<Product>(sql).ToList();
                        }
                        foreach (var prod in productList)
                        {
                            Console.WriteLine($"{prod.Id,-5} {prod.ProductName,-20} {prod.Price,-40}");
                        }
                        CustomerMethods.ShowProductDetails();
                        Menu.AskIfBackToMainMenu();

                    }
                    break;

                case 6:
                    {
                        Menu.BackToMainMenu();
                    }
                    break;
            }



        }
    }
}