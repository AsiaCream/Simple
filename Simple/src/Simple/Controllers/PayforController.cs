using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Simple.Models;

namespace Simple.Controllers
{
    public class PayforController : BaseController
    {
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
