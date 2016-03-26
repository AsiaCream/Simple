using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Simple.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Simple.Controllers
{
    [Authorize]
    public class PagerController : BaseController
    {
        [HttpGet]
        public IActionResult LoadWaitPayOrders(int page)
        {
            var u = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            var order = DB.PreOrders.Where(x => x.UserId == u.Id).Where(x => x.State == State.未锁定).OrderBy(x => x.Id).Skip(page * 3).Take(3).ToList();
            return View(order);
        }
        [HttpGet]
        public IActionResult LoadTrackingOrders(int page)
        {
            var u = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            var order = DB.PreOrders.Where(x => x.UserId == u.Id).OrderBy(x => x.Id).Skip(page * 3).Take(3).ToList();
            return View(order);
        }
        [HttpGet]
        public IActionResult LoadHelpfulWaitPayOrders(int page)
        {
            var u = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            var order = DB.HelpfulPreOrders.Where(x => x.UserId == u.Id).Where(x => x.State == State.未锁定).OrderBy(x => x.Id).Skip(page * 3).Take(3).ToList();
            return View(order);
        }
    }
}
