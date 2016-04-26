using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Simple.Models
{
    public static class SampleData
    {
        public async static Task InitDB(IServiceProvider service)
        {
            var db = service.GetRequiredService<SimpleContext>();

            var userManager = service.GetRequiredService<UserManager<User>>();

            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            if (db.Database != null && db.Database.EnsureCreated())
            {
                //初始化系统用户角色
                await roleManager.CreateAsync(new IdentityRole { Name = "系统管理员" });
                await roleManager.CreateAsync(new IdentityRole { Name = "普通用户" });

                //初始化管理员
                var user = new User { UserName = "Admin", Email = "343224963@qq.com",Name="来自火星的你",Level="99" };
                await userManager.CreateAsync(user, "123456");
                await userManager.AddToRoleAsync(user, "系统管理员");

                //初始化用户
                var guest = new User { UserName = "Guest", Email = "1173056745@qq.com", Name = "来自土星的你",Level="1" };
                await userManager.CreateAsync(guest, "123456");
                await userManager.AddToRoleAsync(guest, "普通用户");

                //初始化用户等级
                db.MemberLevels.Add(new MemberLevel { Level = "1", OrderMax = 2, OrderMin = 1, HelpfulMax = 2, HelpfulMin = 1 });
                db.MemberLevels.Add(new MemberLevel { Level = "2", OrderMax = 3, OrderMin = 2, HelpfulMax = 3, HelpfulMin = 2 });
                db.MemberLevels.Add(new MemberLevel { Level = "3", OrderMax = 4, OrderMin = 3, HelpfulMax = 4, HelpfulMin = 3 });
                db.MemberLevels.Add(new MemberLevel { Level = "4", OrderMax = 5, OrderMin = 4, HelpfulMax = 5, HelpfulMin = 4 });
                db.MemberLevels.Add(new MemberLevel { Level = "5", OrderMax = 6, OrderMin = 5, HelpfulMax = 6, HelpfulMin = 5 });

                //初始化用户的店铺订单量设置
                db.ShopOrders.Add (new ShopOrder {  MaxOneDay = 2, MaxOneEvaluation = 3, Title = "ABC", UserId = guest.Id,Type="Ebay" });

                //初始化隔天下单/首天
                db.NextOrTodays.Add(new NextOrToday { Type = "查看",Price=10.00,Note="" });
                db.NextOrTodays.Add(new NextOrToday { Type = "加购物车",Price = 10.00, Note = "" });
                db.NextOrTodays.Add(new NextOrToday { Type = "加wishlist", Price = 10.00, Note = "选择本项目加收10元，三项不重复计费" });

                //初始化订单类型
                db.OrderTypes.Add(new OrderType { Type = "下单 + feedback + review", Price = 0.00, Note = "" });
                db.OrderTypes.Add(new OrderType { Type = "下单 + feedback", Price = 0.00, Note = "" });
                db.OrderTypes.Add(new OrderType { Type = "下单 +  review", Price = 0.00, Note = "" });
                db.OrderTypes.Add(new OrderType { Type = "只下单不评价", Price = 0.00, Note = "" });

                //初始化进入店铺方式及对应价格
                db.FindTypes.Add(new FindType { Type = "链接进入",Price=0.00,Note="", });
                db.FindTypes.Add(new FindType { Type = "搜索进入",Price=10.00,Note="以搜索方式进入，加收10.00元手续费"});

                //初始化汇率以及对应国家
                db.Rates.Add(new Rate { Country = "美", Exchange = 6.6 });
                db.Rates.Add(new Rate { Country = "英", Exchange = 9.98 });
                db.Rates.Add(new Rate { Country = "日", Exchange = 0.063 });
                db.Rates.Add(new Rate { Country = "加", Exchange = 5.38 });
                db.Rates.Add(new Rate { Country = "欧", Exchange = 7.59 });

                //初始化FeedBack模版
                db.FeedBackModels.Add(new FeedBackModel { Content = "Good quality!Good price!" });
                db.FeedBackModels.Add(new FeedBackModel { Content = "値段の割りにはとっても可愛くレースもゴージャス感があり、気に入りました。" });
                db.FeedBackModels.Add(new FeedBackModel { Content = "Item arrived quickly and in perfect condition. Very satisfied! Will buy more." });

                //初始化平台类型
                db.PlatTypes.Add(new PlatType { Title = "Amazon" });
                db.PlatTypes.Add(new PlatType { Title = "Ebay" });

                //初始化Helpful价格
                db.HelpfulPrices.Add(new HelpfulPrice { Price = 2.00,WishListCost=2.00 });

                //初始化评价时间以及备注
                db.CommentTimes.Add(new CommentTime { Date = 15, Note = "亚马逊订单生成时开始计时" });
                
                //初始化IncreasingNumber
                db.IncreasingNumbers.Add(new IncreasingNumber { Number = 0 });

                //初始化系统标题
                db.SystemInfos.Add(new SystemInfo { BigTitle = "Simple", SmallTitle = "S",Company= "宝印科技有限公司",Url="http://www.asiacream.cn" });

            }
            db.SaveChanges();
        }
    }
}
