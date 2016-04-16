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
                var shop = DB.ShopOrders
                    .Where(x => x.Id == id)
                    .SingleOrDefault();
                var plattype = DB.PlatTypes
                    .Where(x => x.Title!=shop.Type)
                    .ToList();
                ViewBag.PlatType = plattype;
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
                old.Type = shop.Type;
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
            var shop = DB.ShopOrders
                .Where(x => x.UserId == user.Id)
                .ToList();
            var c = DB.Rates
                .OrderBy(x => x.Exchange)
                .ToList();
            var f = DB.FindTypes
                .OrderBy(x => x.Id)
                .ToList();
            var commentTime = DB.CommentTimes
                .OrderBy(x => x.Id)
                .ToList();
            var n = DB.NextOrTodays
                .OrderBy(x => x.Id)
                .ToList();
            var fb = DB.FeedBackModels
                .OrderBy(x => x.Id)
                .ToList();
            ViewBag.CommentTime = commentTime;
            ViewBag.Country = c;
            ViewBag.FindType = f;
            ViewBag.Shop = shop;
            ViewBag.NextOrToday = n;
            ViewBag.FeedBackModel = fb;
            return View();
        }
        [HttpPost]
        public IActionResult OneToOne(string id,double Rate,string FindType,string GoodsUrl,string ShopName,double GoodsCost,double Freight,string OrderType,string NextOrToday,bool AvoidWeekend,bool Extension,int CommentTime,string Address,string FeedBackStar,string FeedBackContent,string ReviewStar,string ReviewContent,string ReviewTitle,int Times,string Note)
        {
            var preorder = new PreOrder
            {
                Rate = Rate,
                FindType = FindType,
                GoodsUrl = GoodsUrl,
                ShopName = ShopName,
                GoodsCost = GoodsCost,
                Freight = Freight,
                OrderType = OrderType,
                NextOrToday = NextOrToday,
                AvoidWeekend = AvoidWeekend,
                Extension = Extension,
                CommentTime = CommentTime,
                FeedBackStar = FeedBackStar,
                FeedBackContent = FeedBackContent,
                ReviewStar = ReviewStar,
                ReviewContent = ReviewContent,
                ReviewTitle = ReviewTitle,
                Times = Times,
                Note = Note
            };
            //找到用户提交订单中进入店铺方式，进而找出进入店铺方式所需要的价格
            var findtype = DB.FindTypes
                .Where(x => x.Type == preorder.FindType)
                .SingleOrDefault();
            //找到隔天下单/首天下单类型以及找到最高价格
            var nextortoday = DB.NextOrTodays
                .OrderByDescending(x => x.Price)
                .FirstOrDefault();
                
            var oldnum = DB.IncreasingNumbers
                .OrderByDescending(x => x.Number)
                .First();

            var num = new IncreasingNumber { Number = oldnum.Number + 1 };
            //找出对应的id用户
            var user = DB.Users
                .Where(x => x.Id == id)
                .SingleOrDefault();

            //找出对应订单店铺名所在平台
            var plattype = DB.ShopOrders
                .Where(x => x.Title == preorder.ShopName)
                .SingleOrDefault();
            //找出订单汇率，通过这个找到对应国家
            var rate = DB.Rates
                .Where(x => x.Exchange == preorder.Rate)
                .SingleOrDefault();

            DB.PreOrders.Add(preorder);

            DB.IncreasingNumbers.Add(num);

            var s = preorder.Id;

            var poundage = new Poundage
            {
                PreOrderId = preorder.Id,
                OrderCost = 30.00,
                AddressCost = 0.00,
                SearchCost = findtype.Price,
                ImageCost = 0.00,
                NextOrToday = 0.00
            };

            DB.Poundages.Add(poundage);
            if (preorder.NextOrToday != null)
            {
                //把隔天下单/首天下单价格加入手续费中
                poundage.NextOrToday = nextortoday.Price;
            }
            if (preorder.ImageUrl1 != null || preorder.ImageUrl2 != null || preorder.ImageUrl3 != null)
            {
                //把图片价格加入手续费中
                poundage.ImageCost = 20.00;
            }
            if (preorder.Address != null)
            {
                //把地址价格加入手续费中
                poundage.AddressCost = 5.00;
            }
            //计算手续费总价=订单价格+搜索价格+图片价格+地址价格+隔天下单价格
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
            preorder.PostTime = DateTime.Now;
            DB.SaveChanges();

            return Content("success");
        }
        #endregion
        [HttpGet]
        public IActionResult EditOrder(int id)
        {
            var order = DB.PreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (order == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                ViewBag.FeedBackModel = DB.FeedBackModels
                    .OrderBy(x => x.Id)
                    .ToList();
                ViewBag.Country = DB.Rates
                    .Where(x => x.Exchange != order.Rate)
                    .ToList();
                ViewBag.FindType = DB.FindTypes
                    .Where(x => x.Type != order.FindType)
                    .ToList();
                ViewBag.Shop = DB.ShopOrders
                    .Where(x => x.UserId == order.UserId)
                    .Where(x => x.Title != order.ShopName)
                    .ToList();
                ViewBag.NextOrToday = DB.NextOrTodays
                    .Where(x => x.Type != order.NextOrToday)
                    .ToList();
                return View(order);
            }
        }
        [HttpPost]
        public IActionResult EditOrder(int id,double Rate, string FindType, string GoodsUrl, string ShopName, 
            double GoodsCost, double Freight, string OrderType, string NextOrToday, bool AvoidWeekend, 
            bool Extension, int CommentTime, string Address, string FeedBackStar, string FeedBackContent, 
            string ReviewStar, string ReviewContent, string ReviewTitle, int Times, string Note)
        {
            var old = DB.PreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (old == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                //找到之前手续费
                var poundage = DB.Poundages
                    .Where(x => x.PreOrderId == old.Id)
                    .Single();
                old.Rate = Rate;
                old.FindType = FindType;
                old.GoodsUrl = GoodsUrl;
                old.ShopName = ShopName;
                old.GoodsCost = GoodsCost;
                old.Freight = Freight;
                old.OrderType = OrderType;
                old.NextOrToday = NextOrToday;
                old.AvoidWeekend = AvoidWeekend;
                old.Extension = Extension;
                old.CommentTime = CommentTime;
                old.Address = Address;
                old.FeedBackStar = FeedBackStar;
                old.FeedBackContent = FeedBackContent;
                old.ReviewStar = ReviewStar;
                old.ReviewContent = ReviewContent;
                old.ReviewTitle = ReviewTitle;
                old.Times = Times;
                old.Note = Note;
                DB.SaveChanges();

                return Content("success");
            }
        }
        [HttpPost]
        public IActionResult DeleteOrder(int id)
        {
            var order = DB.PreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (order == null)
            {
                return Content("error");
            }
            else
            {
                DB.PreOrders.Remove(order);
                DB.SaveChanges();
                return Content("success");
            }
        }
        #region 用户订单页面管理
        [HttpGet] //所有订单
        public IActionResult AllOrders()
        {
            var orderCount = DB.PreOrders
                .OrderByDescending(x => x.PostTime)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet] //待审核订单
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
        [HttpGet]  //待支付订单
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
        [HttpGet]//用户审核未通过订单
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
        [HttpGet]//用户今日订单
        public IActionResult TodayOrder()
        {
            return View();
        }
        [HttpGet]   //用户可撤销订单
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
        [HttpGet]  //进行中订单
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
        [HttpGet]  //已完成订单
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
        #endregion
        #region 发布Helpful订单
        [HttpGet]
        public IActionResult CreateHelpfulOrder()
        {
            var c = DB.Rates.OrderBy(x => x.Exchange).ToList();
            var p = DB.HelpfulPrices.SingleOrDefault();
            ViewBag.Price = p.Price;
            ViewBag.WishListCost = p.WishListCost;
            ViewBag.Country = c;
            return View();
        }
        [HttpPost]
        public IActionResult CreateHelpfulOrder(string id, string Country, string Url, int Times, bool HelpfulType, bool IsCollection, string Review1, int ReviewStar1, string Review2, int ReviewStar2)
        {
            var helpfulpreorder = new HelpfulPreOrder { Country = Country, Url = Url, Times = Times, HelpfulType = HelpfulType, IsCollection = IsCollection, Review1 = Review1, ReviewStar1 = ReviewStar1, Review2 = Review2, ReviewStar2 = ReviewStar2 };
            var oldnum = DB.IncreasingNumbers.OrderByDescending(x => x.Number).First();
            var num = new IncreasingNumber { Number = oldnum.Number + 1 };
            var helpfulprice = DB.HelpfulPrices.Where(x => x.Id == 1).SingleOrDefault();//找出当前Helpful价格
            var user = DB.Users.Where(x => x.Id == id).SingleOrDefault();//找出当前提交订单的用户
            DB.HelpfulPreOrders.Add(helpfulpreorder);
            DB.IncreasingNumbers.Add(num);
            helpfulpreorder.UserId = user.Id;
            if (IsCollection == true)
            {
                helpfulpreorder.PayFor = helpfulprice.Price * helpfulpreorder.Times + helpfulprice.WishListCost;//需要支付的价格
            }
            else
            {
                helpfulpreorder.PayFor = helpfulprice.Price * helpfulpreorder.Times;
            }
            helpfulpreorder.OrderNumber = DateTime.Now.ToString("yyMMddhhmmss") + helpfulpreorder.Id.ToString() + num.Number.ToString(); //订单号=时间+单号id+数据库中自增的数
            helpfulpreorder.PostTime = DateTime.Now;//下单时间
            helpfulpreorder.Draw = Draw.待审核;
            helpfulpreorder.State = State.未锁定;
            helpfulpreorder.IsFinish = IsFinish.未完成;
            helpfulpreorder.IsPayFor = IsPayFor.未支付;
            DB.SaveChanges();
            return Content("success");
        }
        #endregion
        #region Helpful订单管理
        [HttpGet]//用户Helpful详细
        public IActionResult HelpfulOrderDetails(int id)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Id == id)
                .SingleOrDefault();
            return View(order);
        }
        [HttpPost]//删除helpful订单
        public IActionResult DeleteHelpfulOrder(int id)
        {
            var order = DB.HelpfulPreOrders
                .Where(x => x.Id == id)
                .Where(x => x.State == State.未锁定)
                .SingleOrDefault();

            var orderuser = DB.Users
                .Where(x => x.Id == order.UserId)
                .SingleOrDefault();

            var currentuser = DB.Users
                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                .SingleOrDefault();

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
        [HttpGet]//Helpful所有订单
        public IActionResult HelpfulOrder()
        {
            var orderCount = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Count();

            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]//用户Helpful待审核订单
        public IActionResult HelpfulWaitDraw()
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
        [HttpGet]//审核未通过
        public IActionResult HelpfulFailure()
        {
            var orderCount = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.Draw == Draw.未通过)
                .Where(x => x.State == State.未锁定)
                .Where(x => x.IsPayFor == IsPayFor.未支付)
                .Where(x => x.IsFinish == IsFinish.未完成)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        }
        [HttpGet]
        public IActionResult HelpfulTodayOrder()
        {
            return View();
        }
        [HttpGet]//可撤销订单
        public IActionResult HelpfulErrorOrder()
        {
            var orderCount = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
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
                .Where(x => x.UserId == UserCurrent.Id)
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
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.Draw == Draw.通过)
                .Where(x => x.IsFinish == IsFinish.已完成)
                .Where(x => x.IsPayFor == IsPayFor.已支付)
                .Where(x => x.State == State.锁定)
                .Count();
            ViewBag.totalRecord = orderCount;
            return View();
        } 
        #endregion

    }
}
