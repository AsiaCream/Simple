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
        #region 进入店铺方式显示，修改
        [HttpGet]
        public IActionResult JoinShopType()
        {
            var type = DB.FindTypes.OrderBy(x => x.Id).ToList();
            return View(type);
        }
        [HttpGet]
        public IActionResult CreateJoinShopType()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateJoinShopType(FindType findtype)
        {
            DB.FindTypes.Add(findtype);
            DB.SaveChanges();
            return View();
        }
        [HttpGet]
        public IActionResult EditJoinShopType(int id)
        {
            var ret = DB.FindTypes.Where(x => x.Id == id).SingleOrDefault();
            return View(ret);
        }
        [HttpPost]
        public IActionResult EditJoinShopType(int id, FindType findtype)
        {
            var old = DB.FindTypes.Where(x => x.Id == id).SingleOrDefault();
            if (old == null)
            {
                return Content("该进入店铺方式不存在");
            }
            else
            {
                old.Type = findtype.Type;
                old.Price = findtype.Price;
                old.Note = findtype.Note;
                DB.SaveChanges();
                return RedirectToAction("JoinShopType", "Admin");
            }
        }
        [HttpPost]
        public IActionResult DeleteJoinShopType(int id)
        {
            var type = DB.FindTypes
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (type == null)
            {
                return Content("error");
            }
            else
            {
                DB.FindTypes.Remove(type);
                DB.SaveChanges();
                return Content("success");
            }
        }
        #endregion
        [HttpGet]
        public IActionResult CommentTime()
        {
            var ret = DB.CommentTimes.OrderBy(x => x.Id).ToList();
            return View(ret);
        }
        [HttpGet]
        public IActionResult EditCommentTime(int id)
        {
            var commenttime = DB.CommentTimes.Where(x => x.Id == id).SingleOrDefault();
            return View(commenttime);
        }
        [HttpPost]
        public IActionResult EditCommentTime(int id, CommentTime Commentime)
        {
            var old = DB.CommentTimes.Where(x => x.Id == id).SingleOrDefault();
            if (old == null)
            {
                return Content("找不到选项");
            }
            else
            {
                old.Date = Commentime.Date;
                old.Note = Commentime.Note;
                DB.SaveChanges();
                return RedirectToAction("CommentTime", "Admin");
            }
        }
        #region Helpful费用管理
        [HttpGet]
        public IActionResult HelpfulPrice()
        {
            var ret = DB.HelpfulPrices.ToList();
            return View(ret);
        }
        [HttpGet]
        public IActionResult EditHelpfulPrice(int id)
        {
            var ret = DB.HelpfulPrices.Where(x => x.Id == id).SingleOrDefault();
            return View(ret);
        }
        [HttpPost]
        public IActionResult EditHelpfulPrice(int id, HelpfulPrice helpfulprice)
        {
            var old = DB.HelpfulPrices.Where(x => x.Id == id).SingleOrDefault();
            if (old == null)
            {
                return Content("找不到选项");
            }
            else
            {
                old.Price = helpfulprice.Price;
                old.WishListCost = helpfulprice.WishListCost;
                DB.SaveChanges();
                return RedirectToAction("HelpfulPrice", "Admin");
            }

        }
        #endregion 



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
        [HttpGet]  //已支付列表
        public IActionResult PayedOrders()
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
        public IActionResult OrdersIng()
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
    }
}
