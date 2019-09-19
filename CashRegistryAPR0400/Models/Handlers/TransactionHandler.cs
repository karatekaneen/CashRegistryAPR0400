using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegistryAPR0400.Models.Handlers
{
    class TransactionHandler
    {
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

                if (clerk == null) break; // If we don't have a clerk we abort.
                StartSales(clerk);
            }


        }

        private static void StartSales(Staff clerk)
        {
            /*
             * Först bool för att hålla den öppen
             * Printa meny
             * ny transaktion (om null)
             * Ändra transaktion (om !null) -> EditTransaction
             * Stäng transaktion (om !null) -> FinishTransaction + summarizeTransaction
             * Exit();
             * 
             * */
        }

        private static Staff Login(List<Staff> availableStaff)
        {
            bool loggedIn = false;
            Staff staffToLogin = null;
            Console.Clear();
            Console.WriteLine("** Login ** ");
            availableStaff.ForEach(s => StaffHandler.PrintInfo(s));

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
    }
}
