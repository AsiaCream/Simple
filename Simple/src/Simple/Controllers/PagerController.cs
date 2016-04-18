using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.Data.Entity;
using Simple.Models;
namespace Simple.Controllers
{
    [Authorize]
    public class PagerController : BaseController //该控制器用于加载异步分页的另一个页面
    {
        #region 用户订单显示页面
        [HttpGet]//用户所有订单页面
        public IActionResult LoadAllOrder(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户待审核订单
        public IActionResult LoadWaitDraw(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.待审核)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayfor == IsPayFor.未支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet] //用户待支付订单
        public IActionResult LoadWaitPayOrder(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayfor == IsPayFor.未支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户审核未通过订单
        public IActionResult LoadNotPassOrder(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.未通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayfor == IsPayFor.未支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户可撤销订单
        public IActionResult LoadErrorOrder(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayfor == IsPayFor.已支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户进行中订单
        public IActionResult LoadOrderIng(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.State == State.锁定)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayfor == IsPayFor.已支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户已完成订单
        public IActionResult LoadFinishOrder(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.State == State.锁定)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.已完成)
                .Where(x => x.IsPayfor == IsPayFor.已支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        /// <summary>
        /// 用户Helpful订单显示页面
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet] //用户所有helpful订单
        public IActionResult LoadHelpfulOrder(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .OrderBy(x => x.PostTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户待审核helpful订单页面
        public IActionResult LoadHelpfulDrawOrder(int page)
        {
            var myorder = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.待审核)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(myorder);
        }
        [HttpGet]//用户待支付的Helpful订单
        public IActionResult LoadHelpfulWaitPay(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .Where(x => x.State == State.未锁定)
                .OrderBy(x => x.DrawTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户审核失败Helpful
        public IActionResult LoadHelpfulFailure(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.Draw == Draw.未通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .Where(x => x.State == State.未锁定)
                .OrderBy(x => x.DrawTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet] //用户可以撤销Helpful订单列表
        public IActionResult LoadHelpfulErrorOrder(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .OrderByDescending(x => x.DrawTime)
                .Skip(page * 10)
                .Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户进行中helpful订单
        public IActionResult LoadHelpfulOrderIng(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.锁定)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .OrderByDescending(x => x.DrawTime)
                .Skip(page * 10)
                .Take(10).ToList();
            return View(order);
        }
        [HttpGet]//用户已完成helpful订单
        public IActionResult LoadHelpfulFinish(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.锁定)
                .Where(x => x.IsFinish == IsFinish.已完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .OrderByDescending(x => x.DrawTime)
                .Skip(page * 10)
                .Take(10).ToList();
            return View(order);
        }
        #endregion

        #region 管理员订单显示页面
        [HttpGet]//管理员查看所有订单页面
        public IActionResult LoadAllOrders(int page)
        {
            var order = DB.PreOrders
                .OrderByDescending(x=>x.PostTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看待审订单页面
        public IActionResult LoadWaitDrawOrders(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.待审核)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayfor == IsPayFor.未支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看待支付页面
        public IActionResult LoadWaitPayOrders(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayfor == IsPayFor.未支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看审核未通过页面
        public IActionResult LoadNotPassOrders(int page)
        {
            var order = DB.PreOrders
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.未通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayfor == IsPayFor.未支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看用户支付后，可撤销订单显示
        public IActionResult LoadErrorOrders(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看进行中列表
        public IActionResult LoadOrderIngs(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.锁定)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看已完成列表
        public IActionResult LoadFinishOrders(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.锁定)
                .Where(x => x.IsFinish == IsFinish.已完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .OrderBy(x => x.Id)
                .Skip(page * 10).Take(10).ToList();
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
        [HttpGet]//管理员中Helpful待审核页面内容
        public IActionResult LoadHelpfulDrawOrders(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw == Draw.待审核)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .OrderBy(x => x.PostTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        } 
        [HttpGet]//管理员查看Helpful待支付列表
        public IActionResult LoadHelpfulWaitPays(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .OrderByDescending(x => x.PostTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看Helpful审核不通过订单
        public IActionResult LoadHelpfulFailures(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw == Draw.未通过)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .OrderByDescending(x => x.DrawTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看Helpful可撤销订单
        public IActionResult LoadHelpfulErrorOrders(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .OrderByDescending(x => x.DrawTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看Helpful进行中订单
        public IActionResult LoadHelpfulOrderIngs(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.锁定)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .OrderByDescending(x => x.DrawTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]//管理员查看Helpful已完成订单
        public IActionResult LoadHelpfulFinishs(int page)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.锁定)
                .Where(x => x.IsFinish == IsFinish.已完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .OrderByDescending(x => x.DrawTime)
                .Skip(page * 10).Take(10).ToList();
            return View(order);
        }
        [HttpGet]
        public IActionResult LoadMemberShop(int page)
        {
            var shops = DB.ShopOrders
                .Include(x=>x.User)
                .OrderBy(x => x.UserId)
                .ToList(); 
            
            return View(shops);
        }

        #endregion

    }
}
