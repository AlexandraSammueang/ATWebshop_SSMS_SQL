using Dapper;
using ATWebshop_SQL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ATWebshop_SQL
{
    class CheckOut
    {
        static string connString = "data source=.\\; initial catalog = ATWebshop; persist security info = True; Integrated Security = True;";
        public static void FillInCustomer()
        {
            Console.Clear();
            Console.WriteLine("Proceed to order\n");

            Console.WriteLine("Please fill in your contact details bellow\n");

            Console.WriteLine("Please fill in a Firstname: ");
            string ask = Console.ReadLine();
            Console.WriteLine("Please fill in your Lastname: ");
            string ask2 = Console.ReadLine();
            Console.WriteLine("Please fill in your Address: ");
            string ask3 = Console.ReadLine();
            Console.WriteLine("Please fill in your PostalCode: ");
            string ask4 = Console.ReadLine();
            Console.WriteLine("Please fill in your City: ");
            string ask5 = Console.ReadLine();
            Console.WriteLine("Please fill in your Phonenumber: ");
            string ask6 = Console.ReadLine();

            using (var dbc = new ATWebshopContext())
            {
                var customer = new Customer
                {
                    Firstname = ask,
                    Lastname = ask2,
                    Address = ask3,
                    PostalCode = ask4,
                    City = ask5,
                    PhoneNumber = ask6

                };

                dbc.Add(customer);
                dbc.SaveChanges();
            }


            Console.WriteLine("Move forrward? ");
            string asking = Console.ReadLine();
            if (asking.ToLower() == "yes")
            {
                ShippingMetod();
            }
            else if (asking.ToLower() == "no")
            {
                Menu.BackToMainMenu();
            }

        }
        public static void ShippingMetod()
        {
            Console.WriteLine("Press (C) if you like to pay with card and press (S) if you like to pay with swish?");
            string ask = Console.ReadLine();
            try
            {
                if (ask.ToLower() == "c")
                {
                    Console.WriteLine("Fill in your card information");
                }
                else if (ask.ToLower() == "s")
                {
                    Console.WriteLine("Fill in your phone number to swish");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("You did not choose any payment method. Exception thrown " + ex.Message);
            }
            int Tappar_aldrig_bort_ett_brev_Ab = 25;
            int PostNord_HomeDelivery = 80;

            Console.WriteLine("Press (T) for shipping method 'Tappar aldrig bort ett brev AB' or press (P) for 'PostNord HomeDelivery'. ");
            string ask2 = Console.ReadLine();
            try
            {

                if (ask2.ToLower() == "t")
                {
                    Console.WriteLine($"'Tappar aldrig bort ett brev Ab' cost {Tappar_aldrig_bort_ett_brev_Ab} kr");
                    Console.WriteLine("You have now selected 'Tappar aldrig bort ett brev AB' here is your ShoppingCart press yes to Order ");
                    T();
                    ShowRecipt1();

                }
                else if (ask2.ToLower() == "p")
                {
                    Console.WriteLine($"'PostNord HomeDelivery' cost {PostNord_HomeDelivery} kr");
                    Console.WriteLine("You have now selected 'PostNord HomeDelivery' here is your ShoppingCart press yes to Order ");
                    P();
                    ShowRecipt2();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("You did not choose any shipping method. Exception thrown " + ex.Message);
            }
        }

        public static void T()
        {
            using (var db = new ATWebshopContext())
            {

                var priceTotal = (from cart in db.ShoppingCarts
                                  join
                                       prod in db.Products on cart.ProductId equals prod.Id
                                  select prod.Price).Sum() + 25;

                Console.WriteLine($"Total price: {priceTotal}");

            }
        }
        public static void P()
        {
            using (var db = new ATWebshopContext())
            {

                var priceTotal = (from cart in db.ShoppingCarts
                                  join
                                       prod in db.Products on cart.ProductId equals prod.Id
                                  select prod.Price).Sum() + 80;

                Console.WriteLine($"Total price: {priceTotal}");

            }
        }

        public static void ShowRecipt1()
        {
            Console.Clear();
            decimal shipCost = 25.00m;
            decimal VAT = 0.25m;

            var sql = @"select top 1 id from customers order by id desc";
            var customers = new List<Customer>();

            using (var db = new ATWebshopContext())
            {
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    customers = connection.Query<Customer>(sql).ToList();
                    foreach (var cust in customers)
                    {
                        Console.WriteLine($"Customer details\n {cust.Firstname}\n {cust.Lastname}\n {cust.Address}\n {cust.PostalCode}\n {cust.City}\n {cust.PhoneNumber}");

                    }
                }

            }


            using (var db = new ATWebshopContext())
            {

                var ShopCart = from cart in db.ShoppingCarts
                               join
                                    prod in db.Products on cart.ProductId equals prod.Id
                               select new ShoppingCartQuery { Id = cart.Id, ProductName = prod.ProductName, Price = prod.Price, Quantity = cart.Quantity };

                foreach (var cart in ShopCart)
                {
                    Console.WriteLine($"{cart.Id,-3} {cart.ProductName,-20} {cart.Price,-10} {cart.Quantity}");

                }

            }
            using (var db = new ATWebshopContext())
            {
                var prodQuantity = (from cart in db.ShoppingCarts
                                    join
                                         prod in db.Products on cart.ProductId equals prod.Id
                                    select cart.Quantity).Sum();

                var priceTotal = (from cart in db.ShoppingCarts
                                  join
                                       prod in db.Products on cart.ProductId equals prod.Id
                                  select prod.Price).Sum();


                Console.WriteLine($"\nTotal number of items: {prodQuantity}\n" +
                  $"Total price: {Math.Round((decimal)priceTotal, 2)} kr\nShippingCost: " +
                  $"{Math.Round((decimal)shipCost, 2)} kr\nVAT: {Math.Round((decimal)priceTotal * VAT, 2)} kr");
                InsertCustomerIntoOrder();

            }



        }
        public static void ShowRecipt2()
        {
            Console.Clear();
            decimal shipCost = 80.00m;
            decimal VAT = 0.25m;

            var sql = @"select top 1 id from customers order by id desc";
            var customers = new List<Customer>();

            using (var db = new ATWebshopContext())
            {
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    customers = connection.Query<Customer>(sql).ToList();
                    foreach (var cust in customers)
                    {
                        Console.WriteLine($"Customer details\n {cust.Firstname}\n {cust.Lastname}\n {cust.Address}\n {cust.PostalCode}\n {cust.City}\n {cust.PhoneNumber}");

                    }
                }

            }


            using (var db = new ATWebshopContext())
            {

                var ShopCart = from cart in db.ShoppingCarts
                               join
                                    prod in db.Products on cart.ProductId equals prod.Id
                               select new ShoppingCartQuery { Id = cart.Id, ProductName = prod.ProductName, Price = prod.Price, Quantity = cart.Quantity };

                foreach (var cart in ShopCart)
                {
                    Console.WriteLine($"{cart.Id,-3} {cart.ProductName,-20} {cart.Price,-10} {cart.Quantity}");

                }

            }
            using (var db = new ATWebshopContext())
            {
                var prodQuantity = (from cart in db.ShoppingCarts
                                    join
                                         prod in db.Products on cart.ProductId equals prod.Id
                                    select cart.Quantity).Sum();

                var priceTotal = (from cart in db.ShoppingCarts
                                  join
                                       prod in db.Products on cart.ProductId equals prod.Id
                                  select prod.Price).Sum();


                Console.WriteLine($"\nTotal number of items: {prodQuantity}\n" +
                  $"Total price: {Math.Round((decimal)priceTotal, 2)} kr\nShippingCost: " +
                  $"{Math.Round((decimal)shipCost, 2)} kr\nVAT: {Math.Round((decimal)priceTotal * VAT, 2)} kr");
                InsertCustomerIntoOrder();

            }


        }

        public static void InsertCustomerIntoOrder()
        {


            using (var db = new ATWebshopContext())
            {
                var list = db.Customers.OrderByDescending(c => c.Id).FirstOrDefault();


                var order = new Order
                {
                    CustomerId = list.Id,
                    CustomerAddress = list.Address,
                    CustomerPostalCode = list.PostalCode,
                    CustomerCity = list.City
                };

                db.Add(order);
                db.SaveChanges();
            }
            InsertIntoOrderDetails();
        }

        public static void InsertIntoOrderDetails()
        {
            var sql = @"INSERT INTO orderdetails (productid,unitprice, quantity)
                      SELECT productid, price, quantity
                      FROM shoppingcart
                       join products on products.id = shoppingcart.productid";


            var orderDetails = new List<OrderDetail>();

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                orderDetails = connection.Query<OrderDetail>(sql).ToList();
            }

            sql = @" UPDATE orderdetails
                       SET orderid = (SELECT top 1 id FROM customers order by id desc)
                       where orderid is null";


            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                orderDetails = connection.Query<OrderDetail>(sql).ToList();
            }
            DeleteShoppingCart();

        }

        public static void DeleteShoppingCart()
        {
            var sql = @"Delete from ShoppingCart";

            var deleteCart = new List<ShoppingCart>();

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                deleteCart = connection.Query<ShoppingCart>(sql).ToList();
            }
        }
    }
}
