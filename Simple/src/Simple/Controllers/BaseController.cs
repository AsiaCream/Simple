using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Http;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Simple.Models;

namespace Simple.Controllers
{
    public class BaseController : Controller
    {
        [FromServices]
        public SimpleContext DB { get; set; }
        [FromServices]
        public SignInManager<User> signInManager { get; set; }
        [FromServices]
        public UserManager<User> userManager { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var UserCurrent= DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
                ViewBag.UserCurrent = UserCurrent;
            }
        }
        /*public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            UserCurrent();
            return base.OnActionExecutionAsync(context, next);
        }*/
        /*public virtual User UserCurrent()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                User UserCurrent = DB.Users.Where(x => x.UserName == HttpContext.User.Identity.Name).SingleOrDefault();
                ViewBag.UserCurrent = UserCurrent;       
            return UserCurrent;
            }
            return UserCurrent;
        }*/
    }
   
}
