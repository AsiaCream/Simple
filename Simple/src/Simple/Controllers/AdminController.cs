using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Simple.Models;

namespace Simple.Controllers
{
    [Authorize(Roles = "系统管理员")]
    public class AdminController : BaseController
    {
        [HttpGet]//所有订单
        public IActionResult AllOrders()
        {
            var orderCount = DB.PreOrders
                .OrderByDescending(x => x.PostTime)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//待审核订单
        public IActionResult WaitDrawOrders()
        {
            var orderCount = DB.PreOrders
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.待审核)
                .Where(x => x.IsPayfor == IsPayFor.未支付)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//待支付订单
        public IActionResult WaitPayOrders()
        {
            var orderCount = DB.PreOrders
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsPayfor == IsPayFor.未支付)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//审核未通过列表
        public IActionResult NotPassOrders()
        {
            var orderCount = DB.PreOrders
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.未通过)
                .Where(x => x.IsPayfor == IsPayFor.未支付)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]  //已可撤销列表
        public IActionResult ErrorOrders()
        {
            var orderCount = DB.PreOrders
                .Where(x => x.State == State.未锁定)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsPayfor == IsPayFor.已支付)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//进行中订单
        public IActionResult OrderIng()
        {
            var orderCount = DB.PreOrders
                .Where(x => x.State == State.锁定)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsPayfor == IsPayFor.已支付)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//已完成订单
        public IActionResult FinishOrders()
        {
            var orderCount = DB.PreOrders
                .Where(x => x.State == State.锁定)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsPayfor == IsPayFor.已支付)
                .Where(x => x.IsFinish == IsFinish.已完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpPost]
        public IActionResult OrderPass(int id)
        {
            var order = DB.PreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            order.Draw = Draw.通过;
            order.DrawTime = DateTime.Now;
            DB.SaveChanges();
            return Content("success");
        }
        [HttpPost]
        public IActionResult OrderFailure(int id)
        {
            var order = DB.PreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            order.Draw = Draw.未通过;
            order.DrawTime = DateTime.Now;
            DB.SaveChanges();
            return Content("success");
        }



        [HttpGet]//helpful所有订单
        public IActionResult HelpfulOrders()
        {
            var orderCount = DB.HelpfulPreOrders
                .OrderByDescending(x=>x.PostTime)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//helpful待审核订单
        public IActionResult HelpfulDrawOrders()
        {
            var orderCount = DB.HelpfulPreOrders
                .OrderByDescending(x => x.PostTime)
                .Where(x=>x.Draw==Draw.待审核)
                .Where(x=>x.IsFinish==IsFinish.未完成)
                .Where(x=>x.IsPayFor==IsPayFor.未支付)
                .Where(x=>x.State==State.未锁定)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//helpful待支付订单
        public IActionResult HelpfulWaitPayFor()
        {
            var orderCount = DB.HelpfulPreOrders
                .OrderByDescending(x => x.PostTime)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .Where(x => x.State == State.未锁定)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//Helpful审核失败订单
        public IActionResult HelpfulFailure()
        {
            var orderCount = DB.HelpfulPreOrders
                .OrderByDescending(x => x.PostTime)
                .Where(x => x.Draw == Draw.未通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .Where(x => x.State == State.未锁定)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//Helpful可撤销订单列表
        public IActionResult HelpfulErrorOrder()
        {
            var orderCount = DB.HelpfulPreOrders
                .OrderByDescending(x => x.PostTime)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .Where(x => x.State == State.未锁定)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//Helpful进行中订单
        public IActionResult HelpfulOrderIng()
        {
            var orderCount = DB.HelpfulPreOrders
                .OrderByDescending(x => x.PostTime)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .Where(x => x.State == State.锁定)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//Helpful已完成订单
        public IActionResult HelpfulFinish()
        {
            var orderCount = DB.HelpfulPreOrders
                .OrderByDescending(x => x.PostTime)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.已完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .Where(x => x.State == State.锁定)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        #region Helpful审核方法
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id">通过传进来的helpulf的id</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult HelpfulPass(int id)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            order.Draw = Draw.通过;
            order.DrawTime = DateTime.Now;
            DB.SaveChanges();
            return Content("success");
        }
        /// <summary>
        /// 审核不通过的
        /// </summary>
        /// <param name="id">通过传进来的helpulf的id</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult HelpfulFailure(int id)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            order.Draw = Draw.未通过;
            order.DrawTime = DateTime.Now;
            DB.SaveChanges();
            return Content("success");
        } 
        #endregion
        [HttpGet]//用户Helpful详细
        public IActionResult HelpfulDetails(int id)
        {
            var ret = DB.HelpfulPreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            return View(ret);
        }

        [HttpGet]
        public async Task<IActionResult> MemberShop()
        {
            var Shop = (await userManager.GetUsersInRoleAsync("普通用户")).Count();
            ViewBag.totalRecord = Shop;
            return View();
        }
    }
}
