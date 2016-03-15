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

                var user = new User { UserName = "Admin", Email = "343224963@qq.com" };
                await userManager.CreateAsync(user, "Cream2015!@#");
                await userManager.AddToRoleAsync(user, "系统管理员");

                var guest = new User { UserName = "Guest", Email = "1173056745@qq.com" };
                await userManager.CreateAsync(guest, "Cream2015!@#");
                await userManager.AddToRoleAsync(guest, "普通用户");
            }
            db.SaveChanges();
        }
    }
}
