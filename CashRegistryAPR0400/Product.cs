namespace CashRegistryAPR0400
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            TransactionComponent = new HashSet<TransactionComponent>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public double Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionComponent> TransactionComponent { get; set; }

        internal static void ListAll()
        {
            using (GroceryStoreDataModel db = new GroceryStoreDataModel())
            {
                var products = db.Product.ToList();
                products.ForEach(x => Console.WriteLine(x.ToString()));
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

                using (GroceryStoreDataModel db = new GroceryStoreDataModel())
                {
                    Product product = new Product();
                    product.Name = name;
                    product.Category = category;
                    product.Price = price;

                    db.Product.Add(product);
                    db.SaveChanges();

                    Console.WriteLine("Created product:");
                    Console.WriteLine(product.ToString());
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
                    using (GroceryStoreDataModel db = new GroceryStoreDataModel())
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
                            Console.WriteLine(productToEdit.ToString());

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
            using (GroceryStoreDataModel db = new GroceryStoreDataModel())
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

        public override string ToString()
        { 
            return String.Format("Id: {0} \tName: {1} \tPrice: {2} \tCategory: {3}", Id, Name, Price, Category);
        }
    }
}
