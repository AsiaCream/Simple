using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using System.Security;
using Microsoft.AspNet.Authorization;
using Simple.Models;



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
        public async Task<IActionResult> Register()
        {
            ViewBag.MemberCount = (await userManager.GetUsersInRoleAsync("普通用户")).Count();
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Register(string username,string password,string name,string answer,string question,int qq)
        {
            var olduser = DB.Users.Where(x => x.UserName == username).SingleOrDefault();
            if(olduser!=null)
            {
                return Content("error");
            }
            else{
                var user = new User { UserName = username, Name = name, Answer = answer, Question = question, QQ = qq, Level = "1", RegisterTime = DateTime.Now };
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "普通用户");
                DB.SaveChanges();
                return Content("success");
            }
            
        }
        [Authorize(Roles =("系统管理员"))]
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }
        [Authorize(Roles = ("系统管理员"))]
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(string username, string password, string name, string answer, string question, int qq)
        {
            var admin = new User { UserName = username, Name = name, Answer = answer, Question = question, QQ = qq, Level = "99",RegisterTime=DateTime.Now };
            await userManager.CreateAsync(admin, password);
            await userManager.AddToRoleAsync(admin, "系统管理员");
            DB.SaveChanges();
            return Content("success");
        }
        [HttpGet]
        [Authorize(Roles =("系统管理员"))]
        public async Task<IActionResult> Member()
        {
            var users = (await userManager.GetUsersInRoleAsync("普通用户"))
                .OrderBy(x =>x.RegisterTime)
                .ToList();
            ViewBag.MemberCount = (await userManager.GetUsersInRoleAsync("普通用户")).Count();
            return View(users);
        }
        [HttpGet]
        [Authorize(Roles =("系统管理员"))]
        public async Task<IActionResult> Admins()
        {
            var users = (await userManager.GetUsersInRoleAsync("系统管理员"))
                .OrderByDescending(x => x.RegisterTime)
                .ToList();
            ViewBag.AdminCount = (await userManager.GetUsersInRoleAsync("系统管理员")).Count();
            return View(users);
        }
    }
}
