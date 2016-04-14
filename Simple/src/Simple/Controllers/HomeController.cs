using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Simple.Models;

namespace Simple.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("系统管理员"))
            {
                //管理员Index页面中显示订单数
                var oa = DB.PreOrders.Count();//总单数
                ViewBag.PreOrderAll = oa;
                var ob = DB.PreOrders.Where(x => x.Draw == Draw.待审核).Count();//待审核
                ViewBag.PreOrderWaitDraw = ob;
                var oc = DB.PreOrders.Where(x => x.Draw == Draw.通过).Where(x => x.IsFinish == IsFinish.未完成).Count();//审核待完成
                ViewBag.PreOrderDraw = oc;
                var od = DB.PreOrders.Where(x => x.IsFinish == IsFinish.已完成).Count();//已完成订单
                ViewBag.PreOrderFinish = od;
                var oda = (double)od / oa;
                ViewBag.oda = Math.Round(oda, 2) * 100;//已完成
                var oba = (double)ob / oa;
                ViewBag.oba = Math.Round(oba, 2) * 100;//待审核
                var oca = (double)oc / oa;
                ViewBag.oca = Math.Round(oca, 2) * 100;//已审核待完成
                //管理员Index页面helpful总单数显示
                var a = DB.HelpfulPreOrders.Count();//总单数
                ViewBag.HelpfulAll = a;
                var b = DB.HelpfulPreOrders.Where(x => x.Draw == Draw.待审核).Count();//待审核
                ViewBag.HelpfulWaitDraw = b;
                var c = DB.HelpfulPreOrders.Where(x => x.Draw == Draw.通过).Where(x=>x.IsFinish==IsFinish.未完成).Count();//审核待完成
                ViewBag.HelpfulDraw = c;
                var d = DB.HelpfulPreOrders.Where(x => x.IsFinish == IsFinish.已完成).Count();//已完成订单
                ViewBag.HelpfulFinish = d;
                var da = (double)d / a;
                ViewBag.da = Math.Round(da, 2) * 100;//已完成
                var ba = (double)b / a;
                ViewBag.ba = Math.Round(ba, 2) * 100;//待审核
                var ca = (double)c / a;
                ViewBag.ca = Math.Round(ca, 2) * 100;//已审核待完成
            }
            else
            {
                //用户Index页面中显示订单数
                var oa = DB.PreOrders.Where(x=>x.UserId==UserCurrent.Id).Count();//总单数
                ViewBag.PreOrderAll = oa;
                var ob = DB.PreOrders.Where(x=>x.UserId==UserCurrent.Id).Where(x => x.Draw == Draw.待审核).Count();//待审核
                ViewBag.PreOrderWaitDraw = ob;
                var oc = DB.PreOrders.Where(x => x.UserId == UserCurrent.Id).Where(x => x.Draw == Draw.通过).Where(x=>x.IsFinish==IsFinish.未完成).Count();//审核待完成
                ViewBag.PreOrderDraw = oc;
                var od = DB.PreOrders.Where(x => x.UserId == UserCurrent.Id).Where(x => x.IsFinish == IsFinish.已完成).Count();//已完成订单
                ViewBag.PreOrderFinish = od;
                var oda = (double)od / oa;
                ViewBag.oda = Math.Round(oda, 2) * 100;//已完成
                var oba = (double)ob / oa;
                ViewBag.oba = Math.Round(oba, 2) * 100;//待审核
                var oca = (double)oc / oa;
                ViewBag.oca = Math.Round(oca, 2) * 100;//已审核待完成

                //用户Index页面Helpful订单展示
                
                var a = DB.HelpfulPreOrders.Where(x => x.UserId == UserCurrent.Id).Count();
                ViewBag.HelpfulAll = a;
                var b = DB.HelpfulPreOrders.Where(x => x.UserId == UserCurrent.Id).Where(x => x.Draw == Draw.待审核).Count();//待审核
                ViewBag.HelpfulWaitDraw = b;
                var c = DB.HelpfulPreOrders.Where(x => x.UserId == UserCurrent.Id).Where(x => x.Draw == Draw.通过).Where(x=>x.IsFinish==IsFinish.未完成).Count();//审核待完成
                ViewBag.HelpfulDraw = c;
                var d = DB.HelpfulPreOrders.Where(x => x.UserId == UserCurrent.Id).Where(x => x.IsFinish == IsFinish.已完成).Count();//已完成订单
                ViewBag.HelpfulFinish = d;
                var da = (double)d / a;
                ViewBag.da = Math.Round(da, 2) * 100;//已完成
                var ba = (double)b / a;
                ViewBag.ba = Math.Round(ba, 2) * 100;//待审核
                var ca = (double)c / a;
                ViewBag.ca = Math.Round(ca, 2) * 100;//已审核待完成
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
            return View(orders);
        }
        [HttpPost]
        public IActionResult UserSearch(string key)
        {
            var orders = DB.PreOrders
               .Where(x=>x.UserId==UserCurrent.Id)
               .Where(x => x.PreOrderNumber == key||x.ShopName.Contains(key)||x.TrueOrderNumber==key)
               .ToList();
            if (orders.Count() == 0)
            {
                return Content("error");
            }
            else
            {
                return Content("success");
            }
        }
    }
}
