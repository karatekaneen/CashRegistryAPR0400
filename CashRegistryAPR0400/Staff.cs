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
            return String.Format("{0} {1} - {2} - # Transactions: {3}", FirstName, LastName, SocialSecurityNumber, Transaction.Count);
        }

        internal static void ListAll()
        {
            using (GroceryStoreDataModel db = new GroceryStoreDataModel())
            {
                var staff = db.Staff.ToList();
                staff.ForEach(x => Console.WriteLine(x.ToString()));
            }
        }

        internal static void Create()
        {
            bool socialSecurityNumberValid = false;
            string socSec = null;
            Console.WriteLine("Create Staff member");

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


            //throw new NotImplementedException();
        }

        private static bool ValidateSocialSecurityNumber(string tempSocSec)
        {
            // TODO - Here we *Should* add validation that it aligns with our requirements but I'm making the judgement that it's outside the scope of the assignment.
            return true;
        }

        static public List<Staff> GetStaff()
        {
            using (GroceryStoreDataModel db = new GroceryStoreDataModel())
            {
                var staff = db.Staff.ToList();
                staff.ForEach(x => Console.WriteLine(x.ToString()));
                return staff;
            }
        }
    }
}
