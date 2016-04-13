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
                return RedirectToAction("JoinShopType", "System");
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
                return RedirectToAction("CommentTime", "System");
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
                return RedirectToAction("HelpfulPrice", "System");
            }

        }
        #endregion
        #region 平台分类管理
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
        #endregion
        #region 隔天下单/首天下单类型
        [HttpGet]//显示当前隔天下单/首天下单类型
        public IActionResult NextOrToday()
        {
            var type = DB.NextOrTodays
                .OrderBy(x => x.Id)
                .ToList();
            return View(type);
        }
        [HttpGet]//添加隔天下单/首天下单类型
        public IActionResult CreateNextOrToday()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateNextOrToday(NextOrToday newtype)
        {
            DB.NextOrTodays.Add(newtype);
            DB.SaveChanges();
            return RedirectToAction("NextOrToday", "System");
        }
        [HttpGet]//修改隔天下单/首天下单类型
        public IActionResult EditNextOrToday(int id)
        {
            var old = DB.NextOrTodays
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (old == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                return View(old);
            }
        }
        [HttpPost]
        public IActionResult EditNextOrToday(int id, NextOrToday newtype)
        {
            var old = DB.NextOrTodays
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (old == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                old.Type = newtype.Type;
                old.Price = newtype.Price;
                old.Note = newtype.Note;
                DB.SaveChanges();
                return RedirectToAction("NextOrToday", "System");
            }
        }
        [HttpPost]
        public IActionResult DeleteNextOrToday(int id)
        {
            var type = DB.NextOrTodays
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (type == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                DB.NextOrTodays.Remove(type);
                DB.SaveChanges();
                return Content("success");
            }
        }
        #endregion
        #region 汇率管理
        [HttpGet]//显示当前汇率列表
        public IActionResult Rate()
        {
            var rate = DB.Rates
                .OrderBy(x => x.Id)
                .ToList();
            return View(rate);
        }
        [HttpGet]//添加汇率以及对应国家
        public IActionResult CreateRate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateRate(Rate rate)
        {
            DB.Rates.Add(rate);
            DB.SaveChanges();
            return RedirectToAction("Rate", "System");
        }
        [HttpGet]//修改编辑汇率
        public IActionResult EditRate(int id)
        {
            var old = DB.Rates
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (old == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                return View(old);
            }
        }
        [HttpPost]
        public IActionResult EditRate(int id, Rate newrate)
        {
            var old = DB.Rates
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (old == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                old.Country = newrate.Country;
                old.Exchange = newrate.Exchange;
                DB.SaveChanges();
                return RedirectToAction("Rate", "System");
            }
        }
        [HttpPost]//删除汇率
        public IActionResult DeleteRate(int id)
        {
            var rate = DB.Rates
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (rate == null)
            {
                return Content("error");
            }
            else
            {
                DB.Rates.Remove(rate);
                DB.SaveChanges();
                return View();
            }
        }
        #endregion
        #region FeedBack模版管理
        [HttpGet]//FeedBack模版设置
        public IActionResult FeedBackModel()
        {
            var feedback = DB.FeedBackModels
                .OrderBy(x => x.Id)
                .ToList();
            return View(feedback);
        }
        [HttpGet]//创建Feedback模版
        public IActionResult CreateFeedBackModel()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateFeedBackModel(FeedBackModel newfeedbackmodel)
        {
            DB.FeedBackModels.Add(newfeedbackmodel);
            DB.SaveChanges();
            return RedirectToAction("FeedBackModel", "System");
        }
        [HttpGet]//修改Feedback模版
        public IActionResult EditFeedBackModel(int id)
        {
            var feedback = DB.FeedBackModels
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (feedback == null)
            {
                return RedirectToAction("Error", "System");
            }
            else
            {
                return View(feedback);
            }

        }
        [HttpPost]
        public IActionResult EditFeedBackModel(int id, FeedBackModel newfeedbackmodel)
        {
            var oldfeedback = DB.FeedBackModels
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (oldfeedback == null)
            {
                return RedirectToAction("Error", "System");
            }
            else
            {
                oldfeedback.Content = newfeedbackmodel.Content;
                DB.SaveChanges();
                return RedirectToAction("FeedBackModel", "System");
            }

        }
        [HttpPost]//删除Feedback模版
        public IActionResult DeleteFeedBackModel(int id)
        {
            var feedback = DB.FeedBackModels
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (feedback == null)
            {
                return RedirectToAction("Error", "System");
            }
            else
            {
                DB.FeedBackModels.Remove(feedback);
                DB.SaveChanges();
                return Content("success");
            }
        }
        #endregion
        #region 系统平台设置
        [HttpGet]//系统设置
        public IActionResult BaseInformation()
        {
            var baseinfo = DB.SystemInfos
                .OrderBy(x => x.Id)
                .ToList();
            return View(baseinfo);
        }
        [HttpGet]
        public IActionResult EditBaseInformation(int id)
        {
            var old = DB.SystemInfos
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (old == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                return View(old);
            }
        }
        [HttpPost]
        public IActionResult EditBaseInformation(int id, SystemInfo newsystem)
        {
            var oldsystem = DB.SystemInfos
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (oldsystem == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                oldsystem.BigTitle = newsystem.BigTitle;
                oldsystem.SmallTitle = newsystem.SmallTitle;
                oldsystem.Url = newsystem.Url;
                oldsystem.Company = newsystem.Company;
                DB.SaveChanges();
                return RedirectToAction("BaseInformation", "System");
            }
        } 
        #endregion
    }
}
