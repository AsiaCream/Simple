using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Simple.Models;
using Simple.ViewModels;
using Microsoft.Data.Entity;

namespace Simple.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("系统管理员"))
            {
                //管理员Index页面中显示Ebay订单数
                var oa = DB.PreOrders
                    .Count();//总单数
                ViewBag.PreOrderAll = oa;

                var ob = DB.PreOrders
                    .Where(x => x.Draw == Draw.待审核)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Where(x => x.IsPayfor == IsPayFor.未支付)
                    .Where(x => x.State == State.未锁定)
                    .Count();//待审核
                ViewBag.PreOrderWaitDraw = ob;

                var oc = DB.PreOrders
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Where(x => x.IsPayfor == IsPayFor.未支付)
                    .Where(x => x.State == State.未锁定)
                    .Count();//待支付
                ViewBag.PreOrderDraw = oc;

                var od = DB.PreOrders
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.IsPayfor == IsPayFor.已支付)
                    .Where(x => x.State == State.锁定)
                    .Where(x => x.IsFinish == IsFinish.已完成)
                    .Count();//已完成订单
                ViewBag.PreOrderFinish = od;

                var of = DB.PreOrders
                    .Where(x => x.Draw == Draw.未通过)
                    .Where(x => x.IsPayfor == IsPayFor.未支付)
                    .Where(x => x.State == State.未锁定)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Count();//审核未通过订单
                ViewBag.PreOrderNotPass = of;

                var og = DB.PreOrders
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.IsPayfor == IsPayFor.已支付)
                    .Where(x => x.State == State.锁定)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Count();//审核进行中订单
                ViewBag.PreOrderIng = og;

                var oda = (double)od / oa;
                ViewBag.oda = Math.Round(oda, 2) * 100;//已完成

                var oba = (double)ob / oa;
                ViewBag.oba = Math.Round(oba, 2) * 100;//待审核

                var oca = (double)oc / oa;
                ViewBag.oca = Math.Round(oca, 2) * 100;//已审核待完成

                var ofa = (double)of / oa;
                ViewBag.ofa = Math.Round(ofa, 2) * 100;//审核未通过订单

                var oga = (double)og / oa;
                ViewBag.oga = Math.Round(oga, 2) * 100;//进行中订单


                //用户Index页面Helpful订单展示

                var a = DB.HelpfulPreOrders
                    .Count();
                ViewBag.HelpfulAll = a;

                var b = DB.HelpfulPreOrders
                    .Where(x => x.Draw == Draw.待审核)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Where(x => x.IsPayFor == IsPayFor.未支付)
                    .Where(x => x.State == State.未锁定)
                    .Count();//待审核
                ViewBag.HelpfulWaitDraw = b;

                var c = DB.HelpfulPreOrders
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Where(x => x.IsPayFor == IsPayFor.未支付)
                    .Where(x => x.State == State.未锁定)
                    .Count();//待支付订单
                ViewBag.HelpfulDraw = c;

                var d = DB.HelpfulPreOrders
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.IsFinish == IsFinish.已完成)
                    .Where(x => x.IsPayFor == IsPayFor.已支付)
                    .Where(x => x.State == State.锁定)
                    .Count();//已完成订单
                ViewBag.HelpfulFinish = d;

                var f = DB.HelpfulPreOrders
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Where(x => x.IsPayFor == IsPayFor.未支付)
                    .Where(x => x.State == State.未锁定)
                    .Where(x => x.Draw == Draw.未通过)
                    .Count();//审核未通过订单
                ViewBag.HelpfulNotPass = f;

                var g = DB.LockHelpfulOrders
                    .Where(x=>x.AdminId==UserCurrent.Id)
                    .Include(x=>x.HelpfulPreOrder)
                    .Where(x => x.HelpfulPreOrder.IsFinish == IsFinish.未完成)
                    .Where(x => x.HelpfulPreOrder.IsPayFor == IsPayFor.已支付)
                    .Where(x => x.HelpfulPreOrder.Draw == Draw.通过)
                    .Where(x => x.HelpfulPreOrder.State == State.锁定)
                    .Count();//进行中订单
                ViewBag.HelpfulOrderIng = g;

                var da = (double)d / a;
                ViewBag.da = Math.Round(da, 2) * 100;//已完成

                var ba = (double)b / a;
                ViewBag.ba = Math.Round(ba, 2) * 100;//待审核

                var ca = (double)c / a;
                ViewBag.ca = Math.Round(ca, 2) * 100;//待支付

                var fa = (double)f / a;
                ViewBag.fa = Math.Round(fa, 2) * 100;//审核未通过

                var ga = (double)g / a;
                ViewBag.ga = Math.Round(ga, 2) * 100;//进行中
                var ebay = new List<EbayMonth>();
                var helpful = new List<HelpfulMonth>();
                var ebayhelpful = new List<EbayHelpfulMonth>();
                var ebaymonthlist = DB.PreOrders
                    .Where(x=>x.FinishTime.Year==DateTime.Now.Year)
                    .OrderBy(x=>x.FinishTime.Month)
                    .GroupBy(x => x.FinishTime.Month)
                    .ToList();
                foreach(var x in ebaymonthlist)
                {
                    ebay.Add(new EbayMonth
                    {
                        Count=x.Count(),
                        Month = x.Key,
                    });
                }
                ViewBag.EbayMonth = ebay;
                var helpfulmonthlist = DB.HelpfulPreOrders
                    .Where(x=>x.FinishTime.Year==DateTime.Now.Year)
                    .OrderBy(x => x.FinishTime.Month)
                    .GroupBy(x => x.FinishTime.Month)
                    .ToList();
                foreach(var x in helpfulmonthlist)
                {
                    helpful.Add(new HelpfulMonth
                    {
                        Count = x.Count(),
                        Month=x.Key,
                    });
                }
                ViewBag.HelpfulMonth = helpful;

            }
            else
            {
                //用户Index页面中显示Ebay订单数
                var oa = DB.PreOrders
                    .Where(x=>x.UserId==UserCurrent.Id)
                    .Count();//总单数
                ViewBag.PreOrderAll = oa;
                var ob = DB.PreOrders
                    .Where(x=>x.UserId==UserCurrent.Id)
                    .Where(x => x.Draw == Draw.待审核)
                    .Where(x=>x.IsFinish==IsFinish.未完成)
                    .Where(x=>x.IsPayfor==IsPayFor.未支付)
                    .Where(x=>x.State==State.未锁定)
                    .Count();//待审核
                ViewBag.PreOrderWaitDraw = ob;
                var oc = DB.PreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x=>x.IsFinish==IsFinish.未完成)
                    .Where(x=>x.IsPayfor==IsPayFor.未支付)
                    .Where(x=>x.State==State.未锁定)
                    .Count();//待支付
                ViewBag.PreOrderWaitPay = oc;
                var od = DB.PreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Where(x=>x.Draw==Draw.通过)
                    .Where(x=>x.IsPayfor==IsPayFor.已支付)
                    .Where(x=>x.State==State.锁定)
                    .Where(x => x.IsFinish == IsFinish.已完成)
                    .Count();//已完成订单
                ViewBag.PreOrderFinish = od;
                var of = DB.PreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Where(x => x.Draw == Draw.未通过)
                    .Where(x => x.IsPayfor == IsPayFor.未支付)
                    .Where(x => x.State == State.未锁定)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Count();//审核未通过订单
                ViewBag.PreOrderNotPass = of;
                var og = DB.PreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x => x.IsPayfor == IsPayFor.已支付)
                    .Where(x => x.State == State.锁定)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Count();//审核进行中订单
                ViewBag.PreOrderIng = og;
                var oda = (double)od / oa;
                ViewBag.oda = Math.Round(oda, 2) * 100;//已完成
                var oba = (double)ob / oa;
                ViewBag.oba = Math.Round(oba, 2) * 100;//待审核
                var oca = (double)oc / oa;
                ViewBag.oca = Math.Round(oca, 2) * 100;//已审核待完成
                var ofa = (double)of / oa;
                ViewBag.ofa = Math.Round(ofa, 2) * 100;//审核未通过订单
                var oga = (double)og / oa;
                ViewBag.oga = Math.Round(oga, 2) * 100;//进行中订单


                //用户Index页面Helpful订单展示

                var a = DB.HelpfulPreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Count();
                ViewBag.HelpfulAll = a;
                var b = DB.HelpfulPreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Where(x => x.Draw == Draw.待审核)
                    .Where(x=>x.IsFinish==IsFinish.未完成)
                    .Where(x=>x.IsPayFor==IsPayFor.未支付)
                    .Where(x=>x.State==State.未锁定)
                    .Count();//待审核
                ViewBag.HelpfulWaitDraw = b;
                var c = DB.HelpfulPreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Where(x => x.Draw == Draw.通过)
                    .Where(x=>x.IsFinish==IsFinish.未完成)
                    .Where(x=>x.IsPayFor==IsPayFor.未支付)
                    .Where(x=>x.State==State.未锁定)
                    .Count();//待支付订单
                ViewBag.HelpfulDraw = c;
                var d = DB.HelpfulPreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Where(x=>x.Draw==Draw.通过)
                    .Where(x => x.IsFinish == IsFinish.已完成)
                    .Where(x=>x.IsPayFor==IsPayFor.已支付)
                    .Where(x=>x.State==State.锁定)
                    .Count();//已完成订单
                ViewBag.HelpfulFinish = d;
                var f = DB.HelpfulPreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Where(x=>x.IsPayFor==IsPayFor.未支付)
                    .Where(x=>x.State==State.未锁定)
                    .Where(x=>x.Draw==Draw.未通过)
                    .Count();//审核未通过订单
                ViewBag.HelpfulNotPass = f;
                var g = DB.HelpfulPreOrders
                    .Where(x => x.UserId == UserCurrent.Id)
                    .Where(x => x.IsFinish == IsFinish.未完成)
                    .Where(x=>x.IsPayFor==IsPayFor.已支付)
                    .Where(x=>x.Draw==Draw.通过)
                    .Where(x=>x.State==State.锁定)
                    .Count();//进行中订单
                ViewBag.HelpfulOrderIng = g;
                var da = (double)d / a;
                ViewBag.da = Math.Round(da, 2) * 100;//已完成
                var ba = (double)b / a;
                ViewBag.ba = Math.Round(ba, 2) * 100;//待审核
                var ca = (double)c / a;
                ViewBag.ca = Math.Round(ca, 2) * 100;//已审核待完成
                var fa = (double)f / a;
                ViewBag.fa = Math.Round(fa, 2) * 100;//待审核
                var ga = (double)g / a;
                ViewBag.ga = Math.Round(ga, 2) * 100;//待审核

                var ebay = new List<EbayMonth>();
                var helpful = new List<HelpfulMonth>();
                var ebayhelpful = new List<EbayHelpfulMonth>();
                var ebaymonthlist = DB.PreOrders
                    .Where(x => x.FinishTime.Year == DateTime.Now.Year)
                    .Where(x=>x.UserId==UserCurrent.Id)
                    .OrderBy(x => x.FinishTime.Month)
                    .GroupBy(x => x.FinishTime.Month)
                    .ToList();
                foreach (var x in ebaymonthlist)
                {
                    ebay.Add(new EbayMonth
                    {
                        Count = x.Count(),
                        Month = x.Key,
                    });
                }
                ViewBag.EbayMonth = ebay;
                var helpfulmonthlist = DB.HelpfulPreOrders
                    .Where(x=>x.UserId==UserCurrent.Id)
                    .Where(x => x.FinishTime.Year == DateTime.Now.Year)
                    .OrderBy(x => x.FinishTime.Month)
                    .GroupBy(x => x.FinishTime.Month)
                    .ToList();
                foreach (var x in helpfulmonthlist)
                {
                    helpful.Add(new HelpfulMonth
                    {
                        Count = x.Count(),
                        Month = x.Key,
                    });
                }
                ViewBag.HelpfulMonth = helpful;

            }
            return View();
        }
        /// <summary>
        /// 错误页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        } 
        [HttpGet]
        public IActionResult UserSearchResult(string key)
        {
            var orders = DB.PreOrders
                .Where(x=>x.UserId==UserCurrent.Id)
                .Where(x => x.PreOrderNumber == key || x.ShopName.Contains(key) || x.TrueOrderNumber == key)
                .OrderBy(x => x.PostTime)
                .ToList();
            var helpful = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.OrderNumber == key)
                .SingleOrDefault();
                ViewBag.HelpfulOrder = helpful;
                return View(orders);
            
        }
        [HttpPost]
        public IActionResult UserSearch(string key)
        {
            var orders = DB.PreOrders
               .Where(x => x.UserId == UserCurrent.Id)
               .Where(x => x.PreOrderNumber == key || x.ShopName.Contains(key) || x.TrueOrderNumber == key)
               .Count();
            var helpful = DB.HelpfulPreOrders
                .Where(x => x.UserId == UserCurrent.Id)
                .Where(x => x.OrderNumber == key)
                .SingleOrDefault();
            if (orders== 0&&helpful==null)
            {
                return Content("error");
            }
            else
            {
                return Content("success");
            }
        }
        [Authorize(Roles =("系统管理员"))]
        [HttpPost]
        public IActionResult AdminSearch(string key)
        {
            var orders = DB.LockOrders
                .Include(x=>x.PreOrder)
                .Where(x=>x.AdminId==UserCurrent.Id)
               .Where(x => x.PreOrder.PreOrderNumber == key || x.PreOrder.ShopName.Contains(key) || x.PreOrder.TrueOrderNumber == key)
               .Count();
            var helpful = DB.LockHelpfulOrders
                .Include(x=>x.HelpfulPreOrder)
                .Where(x=>x.AdminId==UserCurrent.Id)
                .Where(x => x.HelpfulPreOrder.OrderNumber == key)
                .SingleOrDefault();
            if (orders == 0 && helpful == null)
            {
                return Content("error");
            }
            else
            {
                return Content("success");
            }
        }
        [Authorize(Roles =("系统管理员"))]
        [HttpGet]
        public IActionResult AdminSearchResult(string key)
        {
            var orders = DB.LockOrders
                .Include(x=>x.PreOrder)
                .Where(x=>x.AdminId==UserCurrent.Id)
                .Where(x => x.PreOrder.PreOrderNumber == key || x.PreOrder.ShopName.Contains(key) || x.PreOrder.TrueOrderNumber == key)
                .OrderBy(x => x.PreOrder.PostTime)
                .ToList();
            var helpful = DB.LockHelpfulOrders
                .Include(x=>x.HelpfulPreOrder)
                .Where(x=>x.AdminId==UserCurrent.Id)
                .Where(x => x.HelpfulPreOrder.OrderNumber == key)
                .SingleOrDefault();
            ViewBag.HelpfulOrder = helpful;
            return View(orders);

        }
        [HttpPost]
        public IActionResult CompareUserName(string username)
        {
            var old = DB.Users
                .Where(x => x.UserName == username)
                .SingleOrDefault();
            if (old != null)
            {
                return Content("存在");
            }
            else
            {
                return Content("error");
            }
        }
    }
}
