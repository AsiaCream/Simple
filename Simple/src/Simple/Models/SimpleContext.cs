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
        public DbSet<MyWallet> MyWallet { get; set; }
        public DbSet<SystemInfo> SystemInfos { get; set; }
        public DbSet<NextOrToday> NextOrTodays { get; set; }
        public DbSet<FindType> FindTypes { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<IncreasingNumber> IncreasingNumbers { get; set; }
        public DbSet<PlatType> PlatTypes { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<HelpfulPreOrder> HelpfulPreOrders { get; set; }
        public DbSet<HelpfulPrice> HelpfulPrices { get; set; }
        public DbSet<CommentTime> CommentTimes { get; set; }
        public DbSet<FeedBackModel> FeedBackModels { get; set; }
        public DbSet<FeedBackStar> FeedBackStars { get; set; } 
        public DbSet<MemberLevel> MemberLevels { get; set; }
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
                e.HasIndex(x => x.PostTime);
                e.HasIndex(x => x.DrawTime);
                e.HasIndex(x => x.StarTime);
                e.HasIndex(x => x.FinishTime);
            });
            builder.Entity<MyWallet>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<SystemInfo>(e =>
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
                e.HasIndex(x => x.Price);
            });
            builder.Entity<OrderType>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<IncreasingNumber>(e =>
            {
                e.HasIndex(x => x.Id);
                e.HasIndex(x => x.Number);
            });
            builder.Entity<PlatType>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<Poundage>(e =>
            {
                e.HasIndex(x => x.Id);
                e.HasIndex(x => x.TotalCost);
            });
            builder.Entity<Rate>(e =>
            {
                e.HasIndex(x => x.Id);
                e.HasIndex(x => x.Exchange);
            });
            builder.Entity<HelpfulPreOrder>(e =>
            {
                e.HasIndex(x => x.Id);
                e.HasIndex(x => x.PayFor);
            });
            builder.Entity<HelpfulPrice>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<CommentTime>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<FeedBackModel>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<FeedBackStar>(e =>
            {
                e.HasIndex(x => x.Id);
            });
            builder.Entity<MemberLevel>(e =>
            {
                e.HasIndex(x => x.Id);
            });
        }
    }
}
