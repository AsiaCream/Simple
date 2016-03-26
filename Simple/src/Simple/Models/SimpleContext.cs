using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Simple.Models;

namespace Simple.Models
{
    public class SimpleContext : IdentityDbContext<User>
    {
        public DbSet<ShopOrder> ShopOrders { get; set; }
        public DbSet<Poundage> Poundages { get; set; }
        public DbSet<PreOrder> PreOrders { get; set; }
        public DbSet<PassOrder> PassOrders { get; set; }
        public DbSet<FailureOrder> FailureOrders { get; set; }
        public DbSet<MyWallet> MyWallet { get; set; }
        public DbSet<System> Systems { get; set; }
        public DbSet<NextOrToday> NextOrTodays { get; set; }
        public DbSet<FindType> FindTypes { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<IncreasingNumber> IncreasingNumbers { get; set; }
        public DbSet<PlatType> PlatTypes { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<HelpfulPreOrder> HelpfulPreOrders { get; set; }
        public DbSet<HelpfulPrice> HelpfulPrices { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ShopOrder>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<PreOrder>(e =>
            {
                e.HasIndex(x=>x.Id);
            });
            builder.Entity<PassOrder>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<FailureOrder>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<MyWallet>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<System>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<NextOrToday>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<FindType>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<OrderType>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<IncreasingNumber>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<PlatType>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<Poundage>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<Rate>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<HelpfulPreOrder>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<HelpfulPrice>(e =>
            {
                e.HasIndex(x => x.Id);
            });
        }
    }
}
