using ATWebshop_SQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATWebshop_SQL

{
    class CustomerMethods
    {

        public static void ShowProductDetails()
        {
            string ask1 = Console.ReadLine();
            Console.WriteLine();
            using (var db = new Models.ATWebshopContext())
            {
                var products = db.Products;



                var chosenProduct = from prod in products
                                    where prod.Id == Convert.ToInt32(ask1)
                                    select prod;

                foreach (var prod in chosenProduct)
                {
                    Console.WriteLine($"{prod.ProductName,-20} {prod.Price}kr\n\n{prod.ProductInfo,-50}");
                }
            }
            AddToCart(ask1);

        }
        public static void AddToCart(string answer)
        {

            Console.WriteLine("Do you want to add the product to your shopping cart?");
            string ask = Console.ReadLine();

            if (ask.ToLower() == "yes")
            {
                using (var dbc = new ATWebshopContext())
                {
                    var shopCart = new ShoppingCart
                    {
                        ProductId = Convert.ToInt32(answer),
                        Quantity = 1
                    };

                    dbc.Add(shopCart);
                    dbc.SaveChanges();
                }
                Console.WriteLine("Do you want to go on to your shopping cart?");
                string askCart = Console.ReadLine();
                if (askCart.ToLower() == "yes")
                {
                    ShowCart();
                }
                else AdminMethods.Products();
            }

            else if (ask.ToLower() == "no")
            {
                Menu.BackToMainMenu();
            }

        }
        public static void ShowCart()
        {
            Console.Clear();
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
            ShoppingCartSumAndProd();

        }
        public static void ShoppingCartSumAndProd()
        {
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



                Console.WriteLine($"\nTotal number of items: {prodQuantity}");
                Console.WriteLine($"Total price: {priceTotal} kr ");



            }
        }
        public static void IncreaseAmountInCart()
        {

            ShowCart();

            Console.WriteLine("\nChoose an item to increase quantity");

            string prodToReduce = Console.ReadLine();
            if (prodToReduce == "no")
            {
                Menu.BackToMainMenu();
            }
            else

                using (var db = new ATWebshopContext())
                {
                    var result = db.ShoppingCarts.SingleOrDefault(b => b.Id == Convert.ToInt32(prodToReduce));

                    result.Quantity = result.Quantity + 1;
                    db.SaveChanges();
                }

            ShowCart();


        }
        public static void ReduceAmountInCart()
        {
            bool keepAsking = true;
            int affectedRows1 = 0;
            do
            {
                Console.WriteLine("\nDo you want to reduce a product from the shopping cart?");
                string ask = Console.ReadLine();
                if (ask.ToLower() == "yes")
                {
                    Console.WriteLine("\nWhich product do you want to reduce?");


                    using (var db = new ATWebshopContext())
                    {
                        var reducePost = db.ShoppingCarts.SingleOrDefault(c => c.Id == Convert.ToInt32(Console.ReadLine()));

                        if (reducePost.Quantity != 0)
                        {
                            reducePost.Quantity = reducePost.Quantity - 1;
                            db.SaveChanges();
                            affectedRows1++;
                        }
                        else if (reducePost.Quantity == 1)
                        {
                            RemoveFromCart();
                        }


                    }

                }
                else if (ask.ToLower() == "no")
                {
                    keepAsking = false;
                    Console.WriteLine($"Affected rows: {affectedRows1}");

                }
            } while (keepAsking);

        }
        public static void RemoveFromCart()
        {
            bool keepAsking = true;
            int affectedRows1 = 0;
            do
            {
                Console.WriteLine("\nDo you want to remove a product from the shopping cart?");
                string ask = Console.ReadLine();
                if (ask.ToLower() == "yes")
                {
                    ShowCart();
                    Console.WriteLine("\nWhich product do you want to remove?");



                    using (var db = new ATWebshopContext())
                    {
                        var removePost = db.ShoppingCarts.SingleOrDefault(c => c.Id == Convert.ToInt32(Console.ReadLine()));
                        db.ShoppingCarts.Remove(removePost);
                        db.SaveChanges();
                        affectedRows1++;

                    }
                    ShowCart();

                }
                else if (ask.ToLower() == "no")
                {
                    keepAsking = false;
                    Console.WriteLine($"Affected rows: {affectedRows1}");
                    Menu.BackToMainMenu();
                }
            } while (keepAsking);

        }

        public static void SearchProducts()
        {
            Console.WriteLine("Do you want to search for an item?");
            string ask = Console.ReadLine();
            if (ask.ToLower() == "yes")
            {
                Console.WriteLine("Enter search word");
                string searchProd = Console.ReadLine();

                using (var db = new ATWebshopContext())
                {
                    var products = db.Products;
                    var productsWithShortName = from prod in products
                                                where
                                                prod.ProductName.Contains(searchProd)
                                                orderby prod.ProductName //descending
                                                select prod.Id + " " + prod.ProductName.ToUpper();


                    foreach (var prodList in productsWithShortName)
                    {
                        Console.WriteLine(prodList);
                    }

                    Console.WriteLine("\nPick one item\n");

                    ShowProductDetails();
                }



            }


        }


    }
}

