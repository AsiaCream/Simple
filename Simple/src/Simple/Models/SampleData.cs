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
                await roleManager.CreateAsync(new IdentityRole { Name = "系统管理员" });
                await roleManager.CreateAsync(new IdentityRole { Name = "普通用户" });

                //初始化管理员
                var user = new User { UserName = "Admin", Email = "343224963@qq.com",Name="来自火星的你" };
                await userManager.CreateAsync(user, "Cream2015!@#");
                await userManager.AddToRoleAsync(user, "系统管理员");

                //初始化用户
                var guest = new User { UserName = "Guest", Email = "1173056745@qq.com", Name = "来自土星的你" };
                await userManager.CreateAsync(guest, "Cream2015!@#");
                await userManager.AddToRoleAsync(guest, "普通用户");

                //初始化用户的店铺订单量设置
                db.ShopOrders.Add (new ShopOrder {  MaxOneDay = 2, MaxOneEvaluation = 3, Title = "ABC", UserId = guest.Id,Type="Ebay" });

                //初始化隔天下单/首天
                db.NextOrTodays.Add(new NextOrToday { Type = "查看" });
                db.NextOrTodays.Add(new NextOrToday { Type = "加购物车" });
                db.NextOrTodays.Add(new NextOrToday { Type = "加wishlist" });

                //初始化进入店铺方式
                db.FindTypes.Add(new FindType { Type = "链接进入" });
                db.FindTypes.Add(new FindType { Type = "搜索进入" });

                //初始化汇率以及对应国家
                db.Rates.Add(new Rate { Country = "美", Exchange = 6.6 });
                db.Rates.Add(new Rate { Country = "英", Exchange = 9.98 });
                db.Rates.Add(new Rate { Country = "日", Exchange = 0.063 });
                db.Rates.Add(new Rate { Country = "加", Exchange = 5.38 });
                db.Rates.Add(new Rate { Country = "欧", Exchange = 7.59 });

                //初始化平台类型
                db.PlatTypes.Add(new PlatType { Title = "Amazon" });
                db.PlatTypes.Add(new PlatType { Title = "Ebay" });

                //初始化IncreasingNumber
                db.IncreasingNumbers.Add(new IncreasingNumber { Number = 0 });


            }
            db.SaveChanges();
        }
    }
}
