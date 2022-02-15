using System;




namespace ATWebshop_SQL
{
    class Menu
    {
        public static void MenuOptions()
        {
            Console.Clear();

            string w = "W";
            string e = "e";
            string l = "l";
            string c = "c";
            string o = "o";
            string m = "m";
            string ee = "e";
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{w}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{e}");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(l);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(c);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(o);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(m);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{ee}");



            Console.WriteLine(" to\nour webshop!!");
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\t\t\t █████  ████████ ██     ██ ███████ ██████  ███████ ██   ██  ██████  ██████  ");
            Console.WriteLine("\t\t\t██   ██    ██    ██     ██ ██      ██   ██ ██      ██   ██ ██    ██ ██   ██");
            Console.WriteLine("\t\t\t███████    ██    ██  █  ██ █████   ██████  ███████ ███████ ██    ██ ██████ ");
            Console.WriteLine("\t\t\t██   ██    ██    ██ ███ ██ ██      ██   ██      ██ ██   ██ ██    ██ ██     ");
            Console.WriteLine("\t\t\t██   ██    ██     ███ ███  ███████ ██████  ███████ ██   ██  ██████  ██  ");



            Console.ForegroundColor = ConsoleColor.White;



            Querys.PrintTopProducts();
            Console.WriteLine("\nSelect an option below\n");
            Console.WriteLine("1 - Admin\n2 - Products\n3 - ShoppingCart\n4 - Search");

        }

        public static bool TryReadChoiceNumber(out int nrChoice)
        {
            string sInput;
            do
            {
                sInput = Console.ReadLine();
                if (int.TryParse(sInput, out nrChoice) && nrChoice <= 6 && nrChoice >= 1)
                {
                    return true;
                }
                else if (sInput != "Q" && sInput != "q")
                {
                    Console.WriteLine("Oh something crazy happened. Try selecting a new number from the menu.");
                }


            } while (nrChoice < 1 || nrChoice > 6);
            return false;
        }

        public static void AdminMenuChoice(int menuChoice)
        {
            Console.Clear();

            int a;
            switch (menuChoice)
            {
                case 1:
                    Console.WriteLine("Select an option below\n");
                    Console.WriteLine("1 - Add a product\n2 - Remove a product\n3 - Update a price \n4 -  Add a category " +
                        "\n5 - Update a product name \n\n6 - Back to main page ");
                    TryReadChoiceNumber(out a);
                    ExecuteAdminChoice(a);
                    break;

                case 2:
                    AdminMethods.Products();
                    break;

                case 3:
                    bool keepAsking = true;
                    do
                    {

                        Console.WriteLine("1 - Show ShoppingCart \n2 - Increase or Reduce a product " +
                            "\n3 - Delete a product from your ShoppingCart\n4 - CheckOut \n5 - Back to main page ");
                        string asking = Console.ReadLine();

                        switch (Convert.ToInt32(asking))
                        {
                            case 1:
                                CustomerMethods.ShowCart();
                                break;
                            case 2:
                                CustomerMethods.IncreaseAmountInCart();
                                CustomerMethods.ReduceAmountInCart();
                                break;
                            case 3:
                                CustomerMethods.RemoveFromCart();
                                break;
                            case 4:
                                CheckOut.FillInCustomer();
                                break;
                            case 5:
                                keepAsking = false;
                                AskIfBackToMainMenu();
                                break;
                        }
                    } while (keepAsking);

                    break;

                case 4:
                    CustomerMethods.SearchProducts();
                    AskIfBackToMainMenu();
                    break;

                default:
                    AskIfBackToMainMenu();
                    break;
            }


        }

        public static void ExecuteAdminChoice(int menuChoice)
        {
            switch (menuChoice)
            {
                case 1:
                    AdminMethods.AddProduct();
                    break;

                case 2:
                    AdminMethods.RemoveProduct();
                    break;

                case 3:
                    AdminMethods.UpdatePrice();
                    break;
                case 4:
                    AdminMethods.AddKategori();

                    break;

                case 5:
                    AdminMethods.UpdateProductName();
                    break;
                default:
                    BackToMainMenu();
                    break;
            }
        }
        public static void AskIfBackToMainMenu()
        {
            Console.WriteLine("\nDo you want to go back to the main menu?");

            if (Console.ReadLine().ToLower() == "yes")
            {
                BackToMainMenu();
            }
            else
                return;
        }
        public static void BackToMainMenu()
        {
            int nrChoice;
            MenuOptions();
            TryReadChoiceNumber(out nrChoice);
            AdminMenuChoice(nrChoice);
        }
    }
}