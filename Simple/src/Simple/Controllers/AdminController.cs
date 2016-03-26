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
                DB.SaveChanges();
                return RedirectToAction("JoinShopType", "Admin");
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
        public IActionResult HelpfulWaitDraw()
        {
            var ret = DB.HelpfulPreOrders.Where(x => x.Draw == Draw.待审核).OrderBy(x => x.Id).ToList();
            return View(ret);
        }
    }
}
