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
        public IActionResult EditCommentTime(int id,CommentTime Commentime)
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
        public IActionResult EditHelpfulPrice(int id,HelpfulPrice helpfulprice)
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



        [HttpGet]
        public IActionResult PreOrder()
        {
            var ret = DB.PreOrders.Where(x=>x.Draw==Draw.待审核).OrderBy(x => x.Id).ToList();
            return View(ret);
        }
        [HttpGet]
        public IActionResult HelpfulOrders()
        {
            var ret = DB.HelpfulPreOrders.OrderBy(x => x.Id).ToList();
            return View(ret);
        }
        [HttpGet]
        public IActionResult HelpfulDrawOrders()
        {
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
            var order = DB.HelpfulPreOrders.Where(x => x.Id == id).SingleOrDefault();
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
            var order = DB.HelpfulPreOrders.Where(x => x.Id == id).SingleOrDefault();
            order.Draw = Draw.未通过;
            order.DrawTime = DateTime.Now;
            DB.SaveChanges();
            return Content("success");
        } 
        #endregion
        [HttpGet]
        public IActionResult HelpfulDetails(int id)
        {
            var ret = DB.HelpfulPreOrders.Where(x => x.Id == id).SingleOrDefault();
            return View(ret);
        }
    }
}
