namespace CashRegistryAPR0400
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransactionComponent")]
    public partial class TransactionComponent
    {
        public int Id { get; set; }

        public double Quantity { get; set; }

        public int TransactionId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        public double ProductPrice { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductCategory { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
