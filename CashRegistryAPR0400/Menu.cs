﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegistryAPR0400
{
    class Menu
    {
        

        static public void InitMenu()
        {
            bool shouldBeOpen = true;
            while (shouldBeOpen)
            {
                Console.Clear();
                
                PrintMainMenu();

                string userInput = Console.ReadLine();

                if (userInput == "0") shouldBeOpen = false;
                else if (userInput == "1") OpenStaffMenu();
                else PrintInvalidChoice();
            }
        }

        private static void PrintInvalidChoice()
        {
            Console.WriteLine("Invalid Choice - Press any key to try again");
            Console.ReadKey();
        }

        private static void OpenStaffMenu()
        {
            bool shouldBeOpen = true;

            while (shouldBeOpen)
            {
                Console.Clear();

                PrintStaffMenu();

                string userInput = Console.ReadLine();

                if (userInput == "0") shouldBeOpen = false;
                else if (userInput == "1") {
                    Staff.ListAll();
                    ReturnToMenu();
                } else if (userInput == "2")
                {
                    Staff.Create();
                    ReturnToMenu();
                }
                else PrintInvalidChoice();
            }
        }

        private static void PrintStaffMenu()
        {
            Console.Clear();
            string staffMenu = "Enter your choice:\n" +
                "\t0 - Exit \n" +
                "\t1 - List all\n" +
                "\t2 - Add\n" +
                "\t3 - Delete\n" +
                "\t4 - Edit";
            Console.WriteLine(staffMenu);
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
    }
}
