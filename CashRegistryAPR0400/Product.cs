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

                price = GetPrice();

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
        private static double GetPrice()
        {
            double price = 0.00;
            while (price <= 0.00) // Can not be free
            {
                Console.Write("Price (use , and not . for decimals): ");
                string inputPrice = Console.ReadLine();

                try
                {
                    price = double.Parse(inputPrice);
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
