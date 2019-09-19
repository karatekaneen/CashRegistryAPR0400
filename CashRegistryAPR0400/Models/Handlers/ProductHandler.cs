using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegistryAPR0400.Models
{
    class ProductHandler
    {
        internal static void ListAll()
        {
            using (CashRegistryModel db = new CashRegistryModel())
            {
                var products = db.Product.ToList();
                products.ForEach(x => Console.WriteLine(PrintInfo(x)));
            }
        }

        internal static List<Product> GetAllProducts()
        {
            using (CashRegistryModel db = new CashRegistryModel())
            {
                return db.Product.ToList();
            }
        }

        internal static void Create()
        {
            bool successful = false;
            string name, category;
            double price = 0.00;

            while (!successful)
            {

                Console.WriteLine("** Create Product");

                Console.Write("Name: ");
                name = Console.ReadLine();

                Console.Write("Category: ");
                category = Console.ReadLine();

                price = GetPrice(false, null);

                using (CashRegistryModel db = new CashRegistryModel())
                {
                    Product product = new Product { Name = name, Category = category, Price = price };

                    db.Product.Add(product);
                    db.SaveChanges();

                    Console.WriteLine("Created product:");
                    Console.WriteLine(PrintInfo(product));
                    successful = true;
                }
            }
        }

        internal static void Edit()
        {
            bool editComplete = false;

            Console.Clear();
            Console.WriteLine("** Edit Product");
            ListAll();
            Console.WriteLine("\nEnter ID of the product you want to edit. 0 (zero) for exit");

            while (!editComplete)
            {
                Console.Write("Id: ");
                string userInput = Console.ReadLine();
                if (userInput == "0") break; // Exit early if 0 is entered
                try
                {
                    int productId = int.Parse(userInput);
                    using (CashRegistryModel db = new CashRegistryModel())
                    {
                        Product productToEdit = db.Product.Find(productId);

                        if (productToEdit != null)
                        {
                            bool isModified = false;

                            Console.WriteLine("If you want to change the value enter the new. If you don't want to change the value leave the row empty.\n");
                            Console.Write("Name: ");
                            string name = Console.ReadLine();

                            Console.Write("Category: ");
                            string category = Console.ReadLine();

                            double price = GetPrice(true, productToEdit.Price);

                            if (name != "")
                            {
                                isModified = true;
                                productToEdit.Name = name;
                            }
                            if (category != "")
                            {
                                isModified = true;
                                productToEdit.Category = category;
                            }
                            if (price > 0)
                            {
                                isModified = true;
                                productToEdit.Price = price;
                            }

                            editComplete = true;
                            if (isModified) db.SaveChanges();
                            Console.WriteLine("Edit complete:");
                            Console.WriteLine(PrintInfo(productToEdit));

                        }
                        else Console.WriteLine(String.Format("No product with id {0} was found.", productId));

                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid formatted ID.");
                }
            }
        }

        internal static void Delete()
        {
            bool removalSuccessful = false;
            Console.Clear();
            Console.WriteLine("** Remove Product");
            ListAll();
            Console.WriteLine("\nEnter ID of the product you want to remove. 0 (zero) for exit");

            while (!removalSuccessful)
            {
                Console.Write("Id: ");
                string userInput = Console.ReadLine();
                if (userInput == "0") break;
                try
                {
                    int productId = int.Parse(userInput);
                    removalSuccessful = RemoveProduct(productId);

                    if (removalSuccessful) Console.WriteLine("Product deleted");
                    else Console.WriteLine(String.Format("No product with id {0} was found.", productId));
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid formatted ID.");
                }
            }
        }

        private static bool RemoveProduct(int productId)
        {
            using (CashRegistryModel db = new CashRegistryModel())
            {
                Product productToRemove = db.Product.Find(productId);

                if (productToRemove != null)
                {
                    db.Product.Remove(productToRemove);
                    db.SaveChanges();
                    return true;
                }
                else return false;
            }
        }

        private static double GetPrice(bool allowEmpty, double? existingPrice)
        {
            double price = 0.00;
            while (price <= 0.00) // Can not be free or negative.
            {
                Console.Write("Price (use , and not . for decimals): ");
                string inputPrice = Console.ReadLine();

                try
                {
                    // This method is used both for creating and modifying prices.
                    //If we allow empty and enter empty string we return the original value 
                    //(Which is not allowed to be null in db, hence no check).
                    if (allowEmpty && inputPrice == "") price = existingPrice.Value;
                    else
                    {
                        price = double.Parse(inputPrice);
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid formatted price - Try again.");
                }
            }
            return price;
        }

        public static string PrintInfo(Product product)
        {
            return String.Format("Id: {0} \tName: {1} \tPrice: {2} \tCategory: {3}", product.Id, product.Name, product.Price, product.Category);
        }
    }
}

