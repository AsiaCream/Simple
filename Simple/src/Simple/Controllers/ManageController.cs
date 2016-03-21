using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Simple.Controllers
{
    public class ManageController : Controller
    {
        [HttpGet]
        public IActionResult ShopOrderAmount()
        {
            return View();
        }
        [HttpGet]
        public IActionResult OneToOne()
        {
            return View();
        }
        [HttpGet]
        public IActionResult OneToMore()
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
        public IActionResult CollectOrder()
        {
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
    }
}
