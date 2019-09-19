namespace CashRegistryAPR0400
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Staff")]
    public partial class Staff
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Staff()
        {
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(12)]
        public string SocialSecurityNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transaction { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0} - {1} {2} - {3} - # Transactions: {4}", Id, FirstName, LastName, SocialSecurityNumber, Transaction.Count);
        }

        internal static void ListAll()
        {
            using (GroceryStoreDataModel db = new GroceryStoreDataModel())
            {
                var staff = db.Staff.ToList();
                staff.ForEach(x => Console.WriteLine(x.ToString()));
            }
        }

        private static bool RemoveStaffMember(int userId)
        {
            using (GroceryStoreDataModel db = new GroceryStoreDataModel())
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
                    using (GroceryStoreDataModel db = new GroceryStoreDataModel())
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

                            if (firstName != "") {
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
                            Console.WriteLine(staffToEdit.ToString());

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

            using (GroceryStoreDataModel db = new GroceryStoreDataModel())
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
