using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegistryAPR0400.Models
{
    class StaffHandler
    {
        public static string PrintInfo(Staff staff)
        {
            return String.Format("Id: {0} - {1} {2} - {3} - # Transactions: {4}", staff.Id, staff.FirstName, staff.LastName, staff.SocialSecurityNumber, staff.Transaction.Count);
        }

        internal static void ListAll()
        {
            using (CashRegistryModel db = new CashRegistryModel())
            {
                var staff = db.Staff.ToList();
                staff.ForEach(x => Console.WriteLine(PrintInfo(x)));
            }
        }

        private static bool RemoveStaffMember(int userId)
        {
            using (CashRegistryModel db = new CashRegistryModel())
            {
                Staff staffToRemove = db.Staff.Find(userId);

                if (staffToRemove != null)
                {
                    db.Staff.Remove(staffToRemove);
                    db.SaveChanges();
                    return true;
                }
                else return false;
            }
        }

        internal static void Edit()
        {
            bool editComplete = false;

            Console.Clear();
            Console.WriteLine("** Edit Staff member");
            ListAll();
            Console.WriteLine("\nEnter ID of the staff member you want to edit. 0 (zero) for exit");

            while (!editComplete)
            {
                Console.Write("Id: ");
                string userInput = Console.ReadLine();
                if (userInput == "0") break; // Exit early if 0 is entered.
                try
                {
                    int userId = int.Parse(userInput);
                    using (CashRegistryModel db = new CashRegistryModel())
                    {
                        Staff staffToEdit = db.Staff.Find(userId);

                        if (staffToEdit != null)
                        {
                            bool socialSecurityNumberValid = false;
                            bool isModified = false;
                            string socSec = "";

                            Console.WriteLine("If you want to change the value enter the new. If you don't want to change the value leave the row empty.\n");
                            Console.Write("First name: ");
                            string firstName = Console.ReadLine();

                            Console.Write("Last name: ");
                            string lastName = Console.ReadLine();

                            while (!socialSecurityNumberValid)
                            {
                                Console.Write("Social Security Number: ");
                                string tempSocSec = Console.ReadLine();

                                if (ValidateSocialSecurityNumber(tempSocSec))
                                {
                                    socSec = tempSocSec;
                                    socialSecurityNumberValid = true;
                                }
                            }

                            if (firstName != "")
                            {
                                isModified = true;
                                staffToEdit.FirstName = firstName;
                            }
                            if (lastName != "")
                            {
                                isModified = true;
                                staffToEdit.LastName = lastName;
                            }
                            if (socSec != "")
                            {
                                isModified = true;
                                staffToEdit.SocialSecurityNumber = socSec;
                            }

                            editComplete = true;
                            if (isModified) db.SaveChanges();
                            Console.WriteLine("Edit complete:");
                            Console.WriteLine(PrintInfo(staffToEdit));

                        }
                        else Console.WriteLine(String.Format("No staff member with id {0} was found.", userId));

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
            Console.WriteLine("** Remove Staff member");
            ListAll();
            Console.WriteLine("\nEnter ID of the staff member you want to remove. 0 (zero) for exit");

            while (!removalSuccessful)
            {
                Console.Write("Id: ");
                string userInput = Console.ReadLine();
                if (userInput == "0") break;
                try
                {
                    int userId = int.Parse(userInput);
                    removalSuccessful = RemoveStaffMember(userId);

                    if (removalSuccessful) Console.WriteLine("Staff member deleted");
                    else Console.WriteLine(String.Format("No staff member with id {0} was found.", userId));
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid formatted ID.");
                }
            }

        }

        internal static void Create()
        {
            bool socialSecurityNumberValid = false;
            string socSec = null;
            Console.WriteLine("** Create Staff member");

            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            while (!socialSecurityNumberValid)
            {
                Console.Write("Social Security Number: ");
                string tempSocSec = Console.ReadLine();

                if (ValidateSocialSecurityNumber(tempSocSec))
                {
                    socSec = tempSocSec;
                    socialSecurityNumberValid = true;
                }
            }

            using (CashRegistryModel db = new CashRegistryModel())
            {
                Staff staff = new Staff();

                staff.FirstName = firstName;
                staff.LastName = lastName;
                staff.SocialSecurityNumber = socSec;

                db.Staff.Add(staff);
                db.SaveChanges();
            }

        }



        private static bool ValidateSocialSecurityNumber(string tempSocSec)
        {
            // TODO - Here we *Should* add validation that it aligns with our requirements but I'm making the judgement that it's outside the scope of the assignment.
            return true;
        }
    }
}
