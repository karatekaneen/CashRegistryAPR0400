﻿using CashRegistryAPR0400.Models;
using CashRegistryAPR0400.Models.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CashRegistryAPR0400
{
    class Menu
    {

        #region General

        static public void InitMenu()
        {
            bool shouldBeOpen = true;
            while (shouldBeOpen)
            {
                Console.Clear();
                
                PrintMainMenu();

                string userInput = Console.ReadLine();

                if (userInput == "0") shouldBeOpen = false;
                else if (userInput == "1") OpenStaffHandlerMenu();
                else if (userInput == "2") OpenProductHandlerMenu();
                else if (userInput == "3") OpenTransactionHandlerMenu();
                else PrintInvalidChoice();
            }
        }

        private static void PrintMainMenu()
        {
            Console.Clear();

            string mainMenu = "Enter your choice:\n" +
                "\t0 - Exit \n" +
                "\t1 - Staff\n" +
                "\t2 - Products\n" +
                "\t3 - Transactions";
            Console.WriteLine(mainMenu);

        }

        private static void ReturnToMenu()
        {
            Console.WriteLine("\n\n** Press any key to return to menu");
            Console.ReadKey();
        }

        public static void PrintInvalidChoice()
        {
            Console.WriteLine("Invalid Choice - Press any key to try again");
            Console.ReadKey();
        }


        #endregion

        #region Products

        private static void OpenProductHandlerMenu()
        {
            bool shouldBeOpen = true;

            while (shouldBeOpen)
            {
                Console.Clear();

                PrintProductHandlerMenu();

                string userInput = Console.ReadLine();

                if (userInput == "0") shouldBeOpen = false;
               else if (userInput == "1")
                {
                    ProductHandler.ListAll();
                    ReturnToMenu();
                }
                else if (userInput == "2")
                {
                    ProductHandler.Create();
                    ReturnToMenu();
                }
                else if (userInput == "3")
                {
                    ProductHandler.Delete();
                    ReturnToMenu();
                }
                else if (userInput == "4")
                {
                    ProductHandler.Edit();
                    ReturnToMenu();
                } 
                else PrintInvalidChoice();
            }
        }

        private static void PrintProductHandlerMenu()
        {
            Console.Clear();
            Console.WriteLine("\t *** Products *** \n");
            ProductHandler.ListAll(); // List all the staff every time

            string staffMenu = "\nEnter your choice:\n" +
                "\t0 - Exit \n" +
                "\t1 - List all\n" +
                "\t2 - Add\n" +
                "\t3 - Delete\n" +
                "\t4 - Edit";
            Console.WriteLine(staffMenu);
        }

        #endregion

        #region Staff


        private static void OpenStaffHandlerMenu()
        {
            bool shouldBeOpen = true;

            while (shouldBeOpen)
            {
                Console.Clear();

                PrintStaffHandlerMenu();

                string userInput = Console.ReadLine();

                if (userInput == "0") shouldBeOpen = false;
                else if (userInput == "1") {
                    StaffHandler.ListAll();
                    ReturnToMenu();
                } else if (userInput == "2")
                {
                    StaffHandler.Create();
                    ReturnToMenu();
                } else if (userInput == "3")
                {
                    StaffHandler.Delete();
                    ReturnToMenu();
                } else if (userInput == "4")
                {
                    StaffHandler.Edit();
                    ReturnToMenu();
                }
                else PrintInvalidChoice();
            }
        }

        private static void PrintStaffHandlerMenu()
        {
            Console.Clear();
            Console.WriteLine("\t *** Staff *** \n");
            StaffHandler.ListAll(); // List all the staff every time

            string staffMenu = "\nEnter your choice:\n" +
                "\t0 - Exit \n" +
                "\t1 - List all\n" +
                "\t2 - Add\n" +
                "\t3 - Delete\n" +
                "\t4 - Edit";
            Console.WriteLine(staffMenu);
        }

        #endregion

        #region Transactions

        private static void PrintTransactionHandlerMenu()
        {
            Console.Clear();
            Console.WriteLine("\t *** Transactions *** \n");

            string transactionMenu = "\nEnter your choice:\n" +
                "\t0 - Exit \n" +
                "\t1 - Summary\n" +
                "\t2 - List\n" +
                "\t3 - Sell\n" +
                "\t4 - Delete transaction"; // Not allowing edit due to security reasons. Will be implemented in v2.0
            Console.WriteLine(transactionMenu);
        }

        private static void OpenTransactionHandlerMenu()
        {
            bool shouldBeOpen = true;

            while (shouldBeOpen)
            {
                Console.Clear();

                PrintTransactionHandlerMenu();

                string userInput = Console.ReadLine();

                if (userInput == "0") shouldBeOpen = false;
                /*else if (userInput == "1")
                {
                    ProductHandler.ListAll();
                    ReturnToMenu();
                }
                else if (userInput == "2")
                {
                    ProductHandler.Create();
                    ReturnToMenu();
                }
                */
                else if (userInput == "3")
                {
                    TransactionHandler.Sell();
                    ReturnToMenu();
                }
                /*
                else if (userInput == "4")
                {
                    ProductHandler.Edit();
                    ReturnToMenu();
                }
                */
                else PrintInvalidChoice();
            }
        }

        #endregion


    }
}
