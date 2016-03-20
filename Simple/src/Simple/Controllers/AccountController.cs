﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Simple.Models;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Simple.Controllers
{
    public class AccountController : Controller
    {
        [FromServices]
        public SignInManager<User> signInManager { get; set; }
        public UserManager<User> userManager { get; set; }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username,string password,bool remember)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, remember, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult Modify()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Modify(string password,string newpwd,string confirmpwd)
        {
            if (confirmpwd != newpwd)
            {
                return Content("pwderror");
            }
            //var result = await userManager.ChangePasswordAsync(userManager.FindByIdAsync(User.Current.Id), pwd, newpwd);
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
