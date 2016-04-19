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
        [HttpGet]//Order详细
        public IActionResult OrderDetails(int id)
        {
            var order = DB.PreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (order != null)
            {
                var plat = DB.ShopOrders
                    .Where(x => x.UserId == order.UserId)
                    .Where(x => x.Title == order.ShopName)
                    .SingleOrDefault();
                ViewBag.Plat = plat.Type;
                return View(order);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]//通过审核
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
        [HttpPost]//审核不通过
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
        [HttpPost]//锁定方法
        public IActionResult Locked(int id)
        {
            var order = DB.PreOrders
                .Where(x=>x.IsFinish==IsFinish.未完成)
                .Where(x=>x.IsPayfor==IsPayFor.已支付)
                .Where(x=>x.State==State.未锁定)
                .Where(x=>x.Draw==Draw.通过)
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (order != null)
            {
                order.State = State.锁定;
                order.StarTime = DateTime.Now;
                DB.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("error");
            }
        }
        [HttpPost]//完成方法/用户升级
        public IActionResult Finish(int id)
        {
            var order = DB.PreOrders
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayfor == IsPayFor.已支付)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.锁定)
                .Where(x => x.Id == id)
                .SingleOrDefault();
                if (order != null)
            {
                var user = DB.Users
                   .Where(x => x.Id == order.UserId)
                   .SingleOrDefault();
                var HelpfulFinish = DB.HelpfulPreOrders
                    .Where(x => x.UserId == order.UserId)
                    .Where(x => x.IsFinish == IsFinish.已完成)
                    .Where(x => x.IsPayFor == IsPayFor.已支付)
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.State == State.锁定)
                    .Count();
                var OrderFinish = DB.PreOrders
                    .Where(x => x.UserId == order.UserId)
                    .Where(x => x.IsFinish == IsFinish.已完成)
                    .Where(x => x.IsPayfor == IsPayFor.已支付)
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.State == State.锁定)
                    .Count();
                var lv = DB.MemberLevels
                    .OrderByDescending(x => x.Level)
                    .ToList();
                foreach (var x in lv)
                {
                    if (HelpfulFinish > x.HelpfulMin && HelpfulFinish < x.HelpfulMax && OrderFinish > x.OrderMin && OrderFinish < x.OrderMax)
                    {
                        user.Level = x.Level;
                    }
                }
                order.IsFinish = IsFinish.已完成;
                order.FinishTime = DateTime.Now;
                DB.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("error");
            }
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
        [HttpPost]//Helpful锁定
        public IActionResult HelpfulLockek(int id)
        {
            var helpfulorder = DB.HelpfulPreOrders
                .Where(x=>x.Draw==Draw.通过)
                .Where(x=>x.IsPayFor==IsPayFor.已支付)
                .Where(x=>x.IsFinish==IsFinish.未完成)
                .Where(x=>x.State==State.未锁定)
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (helpfulorder != null)
            {
                helpfulorder.State = State.锁定;
                helpfulorder.StartTime = DateTime.Now;
                DB.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("error");
            }
        }
        [HttpPost]//完成方法/用户升级
        public IActionResult HelpfulFinish(int id)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.State == State.锁定)
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (order != null)
            {
                var user = DB.Users
                    .Where(x => x.Id == order.UserId)
                    .SingleOrDefault();
                var HelpfulFinish = DB.HelpfulPreOrders
                    .Where(x=>x.UserId==order.UserId)
                    .Where(x => x.IsFinish == IsFinish.已完成)
                    .Where(x => x.IsPayFor == IsPayFor.已支付)
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.State == State.锁定)
                    .Count();
                var OrderFinish = DB.PreOrders
                    .Where(x => x.UserId == order.UserId)
                    .Where(x => x.IsFinish == IsFinish.已完成)
                    .Where(x => x.IsPayfor == IsPayFor.已支付)
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.State == State.锁定)
                    .Count();
                var lv = DB.MemberLevels
                    .OrderByDescending(x => x.Level)
                    .ToList();
                foreach(var x in lv)
                {
                    if (HelpfulFinish > x.HelpfulMin && HelpfulFinish<x.HelpfulMax&&OrderFinish>x.OrderMin&&OrderFinish<x.OrderMax)
                    {
                        user.Level = x.Level;
                    }
                }
                
                order.IsFinish = IsFinish.已完成;
                order.FinishTime = DateTime.Now;
                DB.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("error");
            }
        }

        [HttpGet]//会员店铺信息
        public IActionResult MemberShop()
        {
            var Shop = DB.ShopOrders.Count();
            ViewBag.totalRecord = Shop;
            return View();
        }


    }
}
