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
        public DbSet<PreOrder> PreOrders { get; set; }
        public DbSet<PassOrder> PassOrders { get; set; }
        public DbSet<FailureOrder> FailureOrders { get; set; }
        public DbSet<MyWallet> MyWallet { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PreOrder>(e =>
            {
                e.HasIndex();
            });
            builder.Entity<PassOrder>(e =>
            {
                e.HasIndex();
            });
            builder.Entity<FailureOrder>(e =>
            {
                e.HasIndex();
            });
            builder.Entity<MyWallet>(e =>
            {
                e.HasIndex();
            });
        }
    }
}
