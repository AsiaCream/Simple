using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Simple.Models;

namespace Simple.Controllers
{
    [Authorize(Roles =("系统管理员"))]
    public class SystemController : BaseController
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
            var ret = DB.FindTypes
                .Where(x => x.Id == id)
                .SingleOrDefault();
            return View(ret);
        }
        [HttpPost]
        public IActionResult EditJoinShopType(int id, FindType findtype)
        {
            var old = DB.FindTypes
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (old == null)
            {
                return RedirectToAction("Error","Home");
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
        #region 评论时间设置
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
                return RedirectToAction("Error","Home");
            }
            else
            {
                old.Date = Commentime.Date;
                old.Note = Commentime.Note;
                DB.SaveChanges();
                return RedirectToAction("CommentTime", "Admin");
            }
        } 
        #endregion
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
                return RedirectToAction("Error","Home");
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
        [HttpGet]//平台分类
        public IActionResult PlatType()
        {
            var type = DB.PlatTypes
                .OrderBy(x => x.Id)
                .ToList();
            return View(type);
        }
        [HttpGet]//添加平台分类
        public IActionResult CreatePlatType()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreatePlatType(PlatType plattype)
        {
            DB.PlatTypes.Add(plattype);
            DB.SaveChanges();
            return RedirectToAction("PlatType", "System");
        }
        [HttpGet]//修改平台类型
        public IActionResult EditPlatType(int id)
        {
            var oldtype = DB.PlatTypes
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (oldtype == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                return View(oldtype);
            }
        }
        [HttpPost]
        public IActionResult EditPlatType(int id, PlatType plattype)
        {
            var oldtype = DB.PlatTypes
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (oldtype == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                oldtype.Title = plattype.Title;
                DB.SaveChanges();
                return RedirectToAction("PlatType", "System");
            }
        }
        [HttpPost]//删除平台类型
        public IActionResult DeletePlatType(int id)
        {
            var type = DB.PlatTypes
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (type == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                DB.PlatTypes.Remove(type);
                DB.SaveChanges();
                return Content("success");
            }
        }
    }
}
