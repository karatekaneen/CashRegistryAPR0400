namespace CashRegistryAPR0400
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GroceryStoreDataModel : DbContext
    {
        public GroceryStoreDataModel()
            : base("name=GroceryStoreDataModel")
        {
        }

        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionComponent> TransactionComponent { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.TransactionComponent)
                .WithRequired(e => e.Product1)
                .HasForeignKey(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .Property(e => e.SocialSecurityNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Staff>()
                .HasMany(e => e.Transaction)
                .WithRequired(e => e.Staff)
                .HasForeignKey(e => e.StaffMember)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.PaymentMethod)
                .IsUnicode(false);

            modelBuilder.Entity<Transaction>()
                .HasMany(e => e.TransactionComponent)
                .WithRequired(e => e.Transaction1)
                .HasForeignKey(e => e.Transaction)
                .WillCascadeOnDelete(false);
        }
    }
}
