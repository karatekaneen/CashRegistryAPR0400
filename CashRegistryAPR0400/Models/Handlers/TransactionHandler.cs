using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegistryAPR0400.Models.Handlers
{
    class TransactionHandler
    {
        #region Sell

        public static void Sell()
        {
            bool shouldBeOpen = true;

            while (shouldBeOpen)
            {
                List<Staff> availableStaff = StaffHandler.GetAllStaff();
                List<Product> allProducts = ProductHandler.GetAllProducts();
                
                // We must have staff to be able to sell anything:
                if (availableStaff.Count == 0)
                {
                    Console.WriteLine("There must be available staff to be able to sell - Please create those first.");
                    Console.ReadKey();
                    break; // Break the while loop to return to menu
                }

                // We also need products to sell:
                if (allProducts.Count == 0)
                {
                    Console.WriteLine("There must be products available to be able to sell - Please create those first.");
                    Console.ReadKey();
                    break; // Break the while loop to return to menu
                }

                Staff clerk = Login(availableStaff);

                Console.Clear();


                if (clerk == null) break; // If we don't have a clerk we abort.
                Console.WriteLine("Logged in as " + clerk.FirstName + " " + clerk.LastName);
                StartSalesMenu(clerk);
            }


        }

        internal static void StartSalesMenu(Staff clerk)
        {
            bool shouldBeOpen = true;
            Transaction transaction = null;

            while (shouldBeOpen)
            {
                string choice = "Make your choice: \n";
                string salesMenuWithoutOpenTransaction = choice +
                    "\t1 - New transaction\n" +
                    "\t0 - Exit";
                string salesMenuWithOpenTransaction = choice + "" +
                    "\t1 - Edit transaction\n" +
                    "\t2 - Finalize transaction";

                if (transaction == null)
                {
                    Console.WriteLine(salesMenuWithoutOpenTransaction);
                    string userInput = Console.ReadLine();

                    if (userInput == "0") break;
                    else if (userInput == "1") transaction = EditTransaction(transaction, clerk);
                    else Menu.PrintInvalidChoice();

                }
                else
                {
                    PrintSummary(transaction);

                    Console.WriteLine("\n\n");
                    Console.WriteLine(salesMenuWithOpenTransaction);
                    string userInput = Console.ReadLine();

                    if (userInput == "1") transaction = EditTransaction(transaction, clerk);
                    else if (userInput == "2")
                    {
                        FinalizeTransaction(transaction);
                        transaction = null;
                    }
                    else Menu.PrintInvalidChoice();
                }
            }
        }

        private static void FinalizeTransaction(Transaction transaction)
        {

            Console.WriteLine("Select payment method: \n" +
                "\t1 - Cash\n" +
                "\t2 - Card");
            while (transaction.PaymentMethod == null)
            {
                string userInput = Console.ReadLine();

                if (userInput == "1") transaction.PaymentMethod = "Cash";
                else if (userInput == "2") transaction.PaymentMethod = "Card";
                else Menu.PrintInvalidChoice();
            }


            using (CashRegistryModel db = new CashRegistryModel())
            {
                transaction.Staff = db.Staff.Find(transaction.Staff.Id); // fetching the staff "again" to avoid conflicts
                db.Transaction.Add(transaction);
                db.SaveChanges();
            }
        }

        private static Transaction EditTransaction(Transaction transaction, Staff staff)
        {
            if (transaction != null) // If we have an existing transaction to modify
            {
                transaction.Staff = staff; // We only show the latest staff working on the transaction
                transaction = AddProducts(transaction, staff);
                transaction.TimeOfPurchase = DateTime.Now;
                return transaction;
            }
            else // If we have to start from scratch:
            {
                Transaction transactionToHandle = new Transaction
                {
                    Staff = staff,
                    TimeOfPurchase = DateTime.Now,
                };
                transactionToHandle = AddProducts(transactionToHandle, staff);

                return transactionToHandle;
            }
        }

        private static Transaction AddProducts(Transaction transaction, Staff staff)
        {
            bool addingFinished = false;

            List<Product> products = ProductHandler.GetAllProducts();
            Console.WriteLine("** Add products to transaction **\n");

            while (!addingFinished)
            {
                Console.Clear();
                PrintSummary(transaction);
                Console.WriteLine("\n");

                ProductHandler.PrintInfoList(products);

                Console.WriteLine("Enter the id of the product wanted - enter 0 if you are finished.\n");
                Console.Write("Id: ");
                string idInput = Console.ReadLine();
                if (idInput == "0") break;


                try
                {
                    int productId = int.Parse(idInput);
                    Product wantedProduct = products.Find(p => p.Id == productId);

                    if (wantedProduct != null)
                    {
                        Console.WriteLine("Enter the quantity required. Use , if decimals required");
                        Console.Write("Quantity: ");
                        double quantity = double.Parse(Console.ReadLine());

                        TransactionComponent productToAdd = TransactionComponentHandler.Create(transaction, wantedProduct, quantity);
                        transaction.TransactionComponent.Add(productToAdd);

                        Console.WriteLine(String.Format("{0} * {1} à {2} was added to transaction", quantity, productToAdd.ProductName, productToAdd.ProductPrice));
                    }
                    else Console.WriteLine(String.Format("No product with id {0} was found", productId));
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid format of input");
                }
            }
            return transaction;

        }

        internal static void OpenListMenu()
        {
            bool shouldBeOpen = true;

            Console.Clear();
            Console.WriteLine("** List transactions by **");
            Console.WriteLine("0 - Exit\n" +
                "1 - Group by staff member\n" +
                "2 - Order by sum \n" +
                "3 - All transactions >= $100\n" +
                "4 - Health freaks (No dairy or colonial)\n" +
                "5 - Slobs (No fruits)\n");
            while (shouldBeOpen)
            {
                Console.Write("Enter your choice: ");
                string userInput = Console.ReadLine();

                if (userInput == "0") break;
                else if (userInput == "1") PrintByStaffMember();
                //else if (userInput == "2")
                else Menu.PrintInvalidChoice();
            }
        }

        private static void PrintByStaffMember()
        {
            using (CashRegistryModel db = new CashRegistryModel())
            {
                List<Staff> staff = db.Staff
                    .Where(s => s.Transaction.Count > 0) // Only get the staff that has any transactions
                    .ToList();

                foreach (Staff s in staff)
                {
                    Console.WriteLine("\n\n" + StaffHandler.PrintInfo(s));

                    foreach (Transaction t in s.Transaction.OrderByDescending(t => GetTotalSum(t))) // Order by transaction sum
                    {
                        Console.WriteLine(GetShortSummary(t));
                    }
                }
            }
        }

        private static List<Transaction> GetAllTransactionsWithTransactionComponents()
        {
            using (CashRegistryModel db = new CashRegistryModel())
            {
                return db.Transaction.Include("Staff").Include("TransactionComponent").ToList();
            }
        }

        internal static void PrintSummary()
        {
            using (CashRegistryModel db = new CashRegistryModel())
            {
                Console.Clear();

                // Get all of the transactions:
                List<Transaction> allTransactions = db.Transaction.ToList();

                // Print out the summary:
                allTransactions.ForEach(transaction => Console.WriteLine(GetShortSummary(transaction)));

                // Print data about the collection:
                double totalSum = GetTotalSum(allTransactions); // Assign the total to a variable to not run the loop twice
                Console.WriteLine(String.Format("\nTotal number of transactions: {0}\n" +
                    "Total sum: {1}\n" +
                    "Average sum: {2}", allTransactions.Count, totalSum, totalSum/allTransactions.Count ));

            }
        }

        #endregion


        #region Helper methods

        private static void PrintSummary(Transaction transaction)
        {
            Console.WriteLine(String.Format("** Summary **\n" +
                "\tStaff: {0} {1}\n" +
                "\tTime of purchase: {2}\n" +
                "\tProducts: ", 
                transaction.Staff.FirstName,
                transaction.Staff.LastName,
                transaction.TimeOfPurchase.ToShortDateString()));
            foreach (TransactionComponent tc in transaction.TransactionComponent)
            {
                Console.WriteLine("\t\t" + TransactionComponentHandler.Summarize(tc));
            }
            Console.WriteLine(String.Format("\tTotal: {0}",  GetTotalSum(transaction)));
        }

        private static double GetTotalSum(Transaction transaction)
        {
            return TransactionComponentHandler.GetSum(transaction.TransactionComponent);
        }

        private static double GetTotalSum(List<Transaction> transactions) // Overloaded method to be able to summarize lists of transactions
        {
            double totalSum = 0.00;
            transactions.ForEach(transaction => totalSum += GetTotalSum(transaction));
            return totalSum;
        }

        private static string GetShortSummary(Transaction transaction)
        {
            return String.Format("Date: {0}\tNo. products: {1}\tTotal price: {2}", transaction.TimeOfPurchase.ToShortDateString(), transaction.TransactionComponent.Count, GetTotalSum(transaction));
        }



        private static Staff Login(List<Staff> availableStaff)
        {
            bool loggedIn = false;
            Staff staffToLogin = null;
            Console.Clear();
            Console.WriteLine("** Login ** ");
            availableStaff.ForEach(s => Console.WriteLine(StaffHandler.PrintInfo(s)));

            while (!loggedIn)
            {
                try
                {
                    Console.WriteLine("Select which Id to log in as. Enter 0 to exit");
                    Console.Write("Id: ");
                    string userInput = Console.ReadLine();

                    if (userInput == "0") return null;
                    else
                    {
                        int staffId = int.Parse(userInput);
                        staffToLogin = availableStaff.Find(s => s.Id == staffId);

                        if (staffToLogin != null)
                        {
                            loggedIn = true;
                        }
                        else Console.WriteLine(String.Format("No staff with id {0} was found - try again.", staffId));
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Id must be a number. Try again.");
                }
            }
            return staffToLogin;

        }
        #endregion

    }
}
