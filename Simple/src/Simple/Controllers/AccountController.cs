using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using System.Security;
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
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        } 
        #endregion
        [HttpGet]
        public IActionResult Modify()
        {
            //var CurrentUser = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Modify(string password,string newpwd,string confirmpwd)
        {
            if (confirmpwd != newpwd)
            {
                return Content("两次输入密码不一致");
            }
            var user = DB.Users.Where(x => x.Id == UserCurrent.Id).SingleOrDefault();
            //var result = await userManager.ChangePasswordAsync(userManager.FindByIdAsync(UserCurrent.Id),password, newpwd);
            var result = await userManager.ChangePasswordAsync(UserCurrent, password, newpwd);
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Register(string username,string password,string name,string answer,string question,int qq)
        {
            var user = new User { UserName = username, Name=name,Answer = answer, Question = question, QQ = qq };
            await userManager.CreateAsync(user,password);
            await userManager.AddToRoleAsync(user, "普通用户");
            DB.SaveChanges();
            return Content("success");
        }
    }
}
