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
            var order = DB.PreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.State == State.未锁定)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]
        public IActionResult LoadTrackingOrders(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//加载Manage中HelpfulWaitPayFor页面内容
        public IActionResult LoadHelpfulWaitPayOrders(int page)
        {
            var myorder = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x=>x.IsPayFor==IsPayFor.未支付)
                .Where(x=>x.State==State.未锁定)
                .Where(x=>x.Draw==Draw.通过)
                .Where(x=>x.IsFinish==IsFinish.未完成)
                .OrderBy(x => x.Id)
                .Skip(page*10).Take(10).ToList();
            return View(myorder);
        }
        [HttpGet]//加载Admin中HelpfulWaitDrawFor页面内容
        public IActionResult LoadHelpfulDrawOrders(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw ==Draw.待审核 )
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户待审核helpful订单页面
        public IActionResult LoadHelpfulWaitDrawOrders(int page)
        {
            var myorder = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x=>x.IsPayFor==IsPayFor.未支付)
                .Where(x=>x.IsFinish==IsFinish.未完成)
                .Where(x=>x.State==State.未锁定)
                .Where(x => x.Draw == Draw.待审核)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(myorder);
        }
        [HttpGet]//用户提交审核通过之后，可以进行支付的订单
        public IActionResult LoadHelpfulWaitPay(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.Draw == Draw.通过)
                .Where(x=>x.IsFinish==IsFinish.未完成)
                .Where(x=>x.IsPayFor==IsPayFor.未支付)
                .Where(x=>x.State==State.未锁定)
                .OrderBy(x => x.DrawTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet] //用户所有helpful订单
        public IActionResult LoadHelpfulAllOrders(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .OrderBy(x => x.PostTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户已审核通过未完成订单页面
        public IActionResult LoadHelpfulPassNotFinish(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x=>x.Draw==Draw.通过)
                .Where(x=>x.State==State.未锁定)
                .Where(x=>x.IsFinish==IsFinish.未完成)
                .Where(x=>x.IsPayFor==IsPayFor.已支付)
                .OrderByDescending(x => x.DrawTime)
                .Skip(page * 10)
                .Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看所有helpful订单页面
        public IActionResult LoadHelpfulOrders(int page)
        {
            var order = DB.HelpfulPreOrders
                .OrderByDescending(x => x.PostTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
    }
}
