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

        public int Product { get; set; }

        public double Quantity { get; set; }

        public int Transaction { get; set; }

        public virtual Product Product1 { get; set; }

        public virtual Transaction Transaction1 { get; set; }
    }
}
