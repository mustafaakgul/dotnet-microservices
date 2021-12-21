using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.Order.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
        public DbSet<Domain.OrderAggregate.OrderItem> OrderItems { get; set; }

        //address yok o bir value object db de karıslıgı olmayacak

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //burada bire cok ilskiyi tanmlamadık isimlendrmeden efcore anlıyor custom isim verseydk burada onetomany vermemiz lazımdı
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);

            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(o => o.Address).WithOwner();  //entityde owned type annotation olarak vermek yerine burada tnaımlıyoruz cunku 
            //hangi entity ile calıstıgını entty bilmemeis lazım encapsulationdan felan veya bir suru seyden bilgi sızıntısı olmamalı

            base.OnModelCreating(modelBuilder);
        }
    }
}
