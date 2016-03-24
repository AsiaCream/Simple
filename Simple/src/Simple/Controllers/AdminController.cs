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
        public IActionResult EditJoinShopType(int id,FindType findtype)
        {
            var old= DB.FindTypes.Where(x => x.Id == id).SingleOrDefault();
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
    }
}
