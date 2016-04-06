using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using System.Security;
using Microsoft.AspNet.Authorization;
using Simple.Models;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Simple.Controllers
{
    public class AccountController : BaseController
    {
        

        #region 登录页面实现
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, false, false);
            if (result.Succeeded)
            {
                //return RedirectToAction("Index", "Home");
                //var u= DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
                return Content("success");
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
        }
        #endregion
        #region 登出页面
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        } 
        #endregion
        [Authorize]
        [HttpGet]
        public IActionResult Modify()
        {
            //var CurrentUser = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Modify(string password,string newpwd)
        {
            if (UserCurrent == null)
            {
                return Content("error");
            }
            else
            {
                //await userManager.ChangePasswordAsync(userManager.FindByIdAsync(UserCurrent.Id), password, newpwd);
                await userManager.ChangePasswordAsync(UserCurrent, password, newpwd);
                await signInManager.SignOutAsync();
                return Content("success");
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Register(string username,string password,string name,string answer,string question,int qq)
        {
            var user = new User { UserName = username, Name=name,Answer = answer, Question = question, QQ = qq,Level=1 };
            await userManager.CreateAsync(user,password);
            await userManager.AddToRoleAsync(user, "普通用户");
            DB.SaveChanges();
            return Content("success");
        }
        [Authorize(Roles =("系统管理员"))]
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }
        [Authorize(Roles = ("系统管理员"))]
        public async Task<IActionResult> CreateAdmin(string username, string password, string name, string answer, string question, int qq)
        {
            var admin = new User { UserName = username, Name = name, Answer = answer, Question = question, QQ = qq, Level = 99 };
            await userManager.CreateAsync(admin, password);
            await userManager.AddToRoleAsync(admin, "系统管理员");
            DB.SaveChanges();
            return Content("success");
        }
    }
}
