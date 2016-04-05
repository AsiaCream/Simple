using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Simple.Models;

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
            var commentTime = DB.CommentTimes.OrderBy(x => x.Id).ToList();
            ViewBag.CommentTime = commentTime;
            ViewBag.Country = c;
            ViewBag.FindType = f;
            ViewBag.Shop = shop;
            return View();
        }
        [HttpPost]
        public IActionResult OneToOne(string id, PreOrder preorder)
        {
            //找到用户提交订单中进入店铺方式，进而找出进入店铺方式所需要的价格
            var findtype = DB.FindTypes.Where(x => x.Type == preorder.FindType).SingleOrDefault();
            var oldnum = DB.IncreasingNumbers.OrderByDescending(x => x.Number).First();
            var num = new IncreasingNumber { Number = oldnum.Number + 1 };
            var user = DB.Users.Where(x => x.Id == id).SingleOrDefault();
            var plattype = DB.ShopOrders.Where(x => x.Title == preorder.ShopName).SingleOrDefault();
            var rate = DB.Rates.Where(x => x.Exchange == preorder.Rate).SingleOrDefault();
            DB.PreOrders.Add(preorder);
            DB.IncreasingNumbers.Add(num);
            var poundage = new Poundage { PreOrderId = preorder.Id, OrderCost = 30.00, AddressCost = 0.00, SearchCost = findtype.Price, ImageCost = 0.00, NextOrToday = 0.00 };
            DB.Poundages.Add(poundage);
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
            preorder.IsFinish = IsFinish.未完成;
            preorder.IsPayfor = IsPayFor.未支付;
            preorder.PlatType = plattype.Type;
            preorder.Total = preorder.GoodsCost + preorder.Freight;
            preorder.RMB = preorder.Total * preorder.Rate;
            preorder.PayTotal = preorder.RMB + preorder.Poundage;
            preorder.PreOrderNumber = DateTime.Now.ToString("yyyyMMddhhmmss") + preorder.Id.ToString()+num.Number.ToString() ;
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
            var p = DB.HelpfulPrices.SingleOrDefault();
            ViewBag.Price = p.Price;
            ViewBag.WishListCost = p.WishListCost;
            ViewBag.Country = c;
            return View();
        }
        [HttpPost]
        public IActionResult HelpfulOrder(string id, HelpfulPreOrder helpfulpreorder)
        {
            var oldnum = DB.IncreasingNumbers.OrderByDescending(x => x.Number).First();
            var num = new IncreasingNumber{ Number = oldnum.Number + 1 };
            var helpfulprice = DB.HelpfulPrices.Where(x=>x.Id==1).SingleOrDefault();//找出当前Helpful价格
            var user = DB.Users.Where(x => x.Id == id).SingleOrDefault();//找出当前提交订单的用户
            DB.HelpfulPreOrders.Add(helpfulpreorder);
            DB.IncreasingNumbers.Add(num);
            helpfulpreorder.UserId = user.Id;
            helpfulpreorder.PayFor = helpfulprice.Price * helpfulpreorder.Times+helpfulprice.WishListCost;//需要支付的价格
            helpfulpreorder.OrderNumber = DateTime.Now.ToString("yyMMddhhmmss") + helpfulpreorder.Id.ToString() + num.Number.ToString(); //订单号=时间+单号id+数据库中自增的数
            helpfulpreorder.Draw = Draw.待审核;
            helpfulpreorder.State = State.未锁定;
            helpfulpreorder.IsFinish = IsFinish.未完成;
            helpfulpreorder.IsPayFor = IsPayFor.未支付;
            helpfulpreorder.PostTime = DateTime.Now;//下单时间
            DB.SaveChanges();
            return RedirectToAction("HelpfulOrder","Manage");
        }
        [HttpGet]
        public IActionResult HelpfulOrderDetails(int id)
        {
            var order = DB.HelpfulPreOrders.Where(x => x.Id == id).SingleOrDefault();
            return View(order);
        }
        [HttpPost]
        public IActionResult DeleteHelpfulOrder(int id)
        {
            var order = DB.HelpfulPreOrders.Where(x => x.Id == id).SingleOrDefault();
            var orderuser = DB.Users.Where(x => x.Id == order.UserId).SingleOrDefault();
            var currentuser = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
            if (orderuser == currentuser)
            {
                DB.HelpfulPreOrders.Remove(order);
                DB.SaveChanges();
                return Content("success");
            }
            else
            {
                return Content("error");
            }
        }
        [HttpGet]
        public IActionResult HelpfulWaitDraw()
        {
            var orderCount = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x=>x.Draw==Draw.待审核)
                .Where(x=>x.State==State.未锁定)
                .Where(x=>x.IsPayFor==IsPayFor.未支付)
                .Where(x=>x.IsFinish==IsFinish.未完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//待支付Helpful订单
        public IActionResult HelpfulWaitPayFor()
        {
            var orderCount = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.Draw == Draw.待审核)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]
        public IActionResult HelpfulPassNotFinish()
        {
            var orderCount=DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x=>x.Draw==Draw.通过)
                .Where(x=>x.State==State.未锁定)
                .Where(x=>x.IsPayFor==IsPayFor.已支付)
                .Where(x=>x.IsFinish==IsFinish.未完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]
        public IActionResult HelpfulTodayOrder()
        {
            return View();
        }
        [HttpGet]
        public IActionResult HelpfulAllOrder()
        {
            var orderCount = DB.HelpfulPreOrders.Where(x => x.UserId == UserCurrent.Id).Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]
        public IActionResult HelpfulErrorOrder()
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
        
    }
}
