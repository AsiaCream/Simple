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
    [Authorize(Roles ="普通用户")]
    public class ManageController : BaseController
    {
        #region 店铺订单量设置
        [HttpGet]
        public IActionResult ShopOrderAmount()
        {
            var user = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            var shop = DB.ShopOrders.Where(x => x.UserId == user.Id).ToList();
            return View(shop);
        }
        [HttpGet]
        public IActionResult CreateShopOrderAmount()
        {
            var p = DB.PlatTypes.OrderBy(x => x.Id).ToList();
            ViewBag.PlatType = p;
            return View();
        }
        [HttpPost]
        public IActionResult CreateShopOrderAmount(string Id, ShopOrder shop)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var u = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
                if (u == null)
                {
                    return Content("error");
                }
                else
                {               
                    DB.ShopOrders.Add(shop);
                    shop.UserId = u.Id;
                    DB.SaveChanges();
                }
            }
            return RedirectToAction("ShopOrderAmount", "Manage");
        }
        [HttpGet]
        public IActionResult EditShopOrderAmount(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var shop = DB.ShopOrders.Where(x => x.Id == id).SingleOrDefault();
                return View(shop);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public IActionResult EditShopOrderAmount(int id, ShopOrder shop)
        {
            var old = DB.ShopOrders.Where(x => x.Id == id).SingleOrDefault();
            if (old == null)
            {
                return Content("没有找到该记录");
            }
            else
            {
                old.Title = shop.Title;
                old.MaxOneDay = shop.MaxOneDay;
                old.MaxOneEvaluation = shop.MaxOneEvaluation;
                DB.SaveChanges();
                return RedirectToAction("ShopOrderAmount", "Manage");
            }
        }
        #endregion
        #region 发布一产品一单
        [HttpGet]
        public IActionResult OneToOne()
        {
            //现将用户找到
            var user = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            //然后将和用户相关联的店铺信息找到，并用ViewBag返回到前台
            var shop = DB.ShopOrders.Where(x => x.UserId == user.Id).ToList();
            var c = DB.Rates.OrderBy(x => x.Exchange).ToList();
            var f = DB.FindTypes.OrderBy(x => x.Id).ToList();
            ViewBag.Country = c;
            ViewBag.FindType = f;
            ViewBag.Shop = shop;
            return View();
        }
        [HttpPost]
        public IActionResult OneToOne(string id, PreOrder preorder)
        {
            var number = DB.IncreasingNumbers.Max(x => x.Number);
            number  = number + 1;
            var user = DB.Users.Where(x => x.Id == id).SingleOrDefault();
            var plattype = DB.ShopOrders.Where(x => x.Title == preorder.ShopName).SingleOrDefault();
            var rate = DB.Rates.Where(x => x.Exchange == preorder.Rate).SingleOrDefault();
            DB.PreOrders.Add(preorder);
            var poundage = new Poundage { PreOrderId = preorder.Id, OrderCost = 30.00, AddressCost = 0.00, SearchCost = 0.00, ImageCost = 0.00, NextOrToday = 0.00 };
            DB.Poundages.Add(poundage);

            if (preorder.FindType == "搜索进入")
            {
                poundage.SearchCost = 10.00;
            }
            if (preorder.NextOrToday != null)
            {
                poundage.NextOrToday = 10.00;
            }
            if (preorder.ImageUrl1 != null || preorder.ImageUrl2 != null || preorder.ImageUrl3 != null)
            {
                poundage.ImageCost = 20.00;
            }
            if (preorder.Address != null)
            {
                poundage.AddressCost = 5.00;
            }
            poundage.TotalCost = poundage.OrderCost + poundage.SearchCost + poundage.ImageCost + poundage.AddressCost+poundage.NextOrToday;
            preorder.Poundage = poundage.TotalCost;
            preorder.Country = rate.Country;
            preorder.UserId = user.Id;
            preorder.State = State.未锁定;
            preorder.Draw = Draw.待审核;
            preorder.PlatType = plattype.Type;
            preorder.Total = preorder.GoodsCost + preorder.Freight;
            preorder.RMB = preorder.Total * preorder.Rate;
            preorder.PayTotal = preorder.RMB + preorder.Poundage;
            preorder.PreOrderNumber = DateTime.Now.ToString("yyyyMMddhhmmss") + number.ToString() ;
            DB.SaveChanges();

            return RedirectToAction("OneToOne","Manage");
        } 
        #endregion
        [HttpGet]
        public IActionResult OneToMore()
        {
            //现将用户找到
            var user = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            //然后将和用户相关联的店铺信息找到，并用ViewBag返回到前台
            var shop = DB.ShopOrders.Where(x => x.UserId == user.Id).ToList();
            var c = DB.Rates.OrderBy(x => x.Exchange).ToList();
            var f = DB.FindTypes.OrderBy(x => x.Id).ToList();
            ViewBag.Country = c;
            ViewBag.FindType = f;
            ViewBag.Shop = shop;
            return View();
        }
        [HttpPost]
        public IActionResult OneToMore(string id, PreOrder preorder)
        {
            return View();
        }
        [HttpGet]
        public IActionResult WaitPayFor()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TodayOrder()
        {
            return View();
        }
        [HttpGet]
        public IActionResult OrderIng()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Drawed()
        {
            return View();
        }
        [HttpGet]
        public IActionResult FinishOrder()
        {
            return View();
        }
        [HttpGet]
        public IActionResult EditAddress()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TrackingOrder()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ErrorOrder()
        {
            return View();
        }
        [HttpGet]
        public IActionResult HelpfulOrder()
        {
            var c = DB.Rates.OrderBy(x => x.Exchange).ToList();
            ViewBag.Country = c;
            return View();
        }
        [HttpGet]
        public IActionResult WaitPayForOrder()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PassNotFinish()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TodayPlaceOrder()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RestCost()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CostRecord()
        {
            return View();
        }
        public IActionResult LoadWaitPayOrders(int page)
        {
            var u = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            var order = DB.PreOrders.Where(x => x.UserId == u.Id).OrderBy(x => x.Id).Skip(page * 3).Take(3).ToList();
            return View(order);
        }
    }
}
