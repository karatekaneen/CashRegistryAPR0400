namespace CashRegistryAPR0400
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transaction")]
    public partial class Transaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transaction()
        {
            TransactionComponent = new HashSet<TransactionComponent>();
        }

        public int Id { get; set; }

        public DateTime TimeOfPurchase { get; set; }

        public int StaffMember { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        public virtual Staff Staff { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionComponent> TransactionComponent { get; set; }
    }
}
