using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;


namespace Database
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<UE> Units { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<DocumentReceipt> ReceiptDocuments { get; set; }
        public DbSet<ResourceReceipt> ReceiptResources { get; set; }
        public DbSet<DocumentShipping> ShipmentDocuments { get; set; }
        public DbSet<ResourceShipment> ShipmentResources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Уникальные индексы
            modelBuilder.Entity<Resource>().HasIndex(r => r.Name).IsUnique();
            modelBuilder.Entity<UE>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Client>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<DocumentReceipt>().HasIndex(d => d.Number).IsUnique();
            modelBuilder.Entity<DocumentShipping>().HasIndex(d => d.Number).IsUnique();

            // Уникальный составной индекс для Balance
            modelBuilder.Entity<Balance>()
                .HasIndex(b => new { b.ResourceId, b.UE_Id })
                .IsUnique();

            // Ограничения для Quantity
            modelBuilder.Entity<Balance>()
                .Property(b => b.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<ResourceReceipt>()
                .Property(r => r.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<ResourceShipment>()
                .Property(s => s.Quantity)
                .HasPrecision(18, 3);

            // Каскадное удаление для дочерних сущностей
            modelBuilder.Entity<ResourceReceipt>()
                .HasOne(r => r.DocumentReceipt)
                .WithMany(d => d.ResourceReceipts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResourceShipment>()
                .HasOne(s => s.DocumentShipping)
                .WithMany(d => d.ResourceShipments)
                .OnDelete(DeleteBehavior.Cascade);

            // Ограничение на удаление связанных сущностей
            modelBuilder.Entity<Balance>()
                .HasOne(b => b.Resource)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Balance>()
                .HasOne(b => b.Ue)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
