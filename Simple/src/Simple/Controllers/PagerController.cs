using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Simple.Models;

namespace Simple.Controllers
{
    [Authorize]
    public class PagerController : BaseController //该控制器用于加载异步分页的另一个页面
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
        [HttpGet]//加载Manage中HelpfulWaitPayFor页面内容
        public IActionResult LoadHelpfulWaitPayOrders(int page)
        {
            var user = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            var myorder = DB.HelpfulPreOrders.Where(x => x.UserId == user.Id).OrderBy(x => x.Id).ToList();
            return View(myorder);
        }
        [HttpGet]//加载Admin中HelpfulWaitDrawFor页面内容
        public IActionResult LoadHelpfulDrawOrders(int page)
        {
            var order = DB.HelpfulPreOrders.Where(x => x.Draw ==Draw.待审核 ).OrderBy(x => x.Id).Skip(page * 3).Take(3).ToList();
            return View(order);
        }
        [HttpGet]//用户待审核helpful订单页面
        public IActionResult LoadHelpfulWaitDrawOrders(int page)
        {
            var user = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            var myorder = DB.HelpfulPreOrders.Where(x => x.UserId == user.Id).Where(x => x.Draw == Draw.待审核).OrderBy(x => x.Id).Skip(page * 3).Take(3).ToList();
            return View(myorder);
        }
        [HttpGet] //用户所有helpful订单
        public IActionResult LoadHelpfulAllOrders(int page)
        {
            var user = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            var order = DB.HelpfulPreOrders.Where(x => x.UserId == user.Id).OrderBy(x => x.PostTime).Skip(page * 3).Take(3).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看所有helpful订单页面
        public IActionResult LoadHelpfulOrders(int page)
        {
            var order = DB.HelpfulPreOrders.OrderByDescending(x => x.PostTime).Skip(page * 3).Take(3).ToList();
            return View(order);
        }
    }
}
