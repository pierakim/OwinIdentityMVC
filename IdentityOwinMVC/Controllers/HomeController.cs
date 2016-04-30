using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IdentityOwinMVC.Models;
using IdentityOwinMVC.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityOwinMVC.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            MyIdentityDbContext db = new MyIdentityDbContext();

            UserStore<MyIdentityUser> userStore = new UserStore<MyIdentityUser>(db);
            UserManager<MyIdentityUser> userManager = new UserManager<MyIdentityUser>(userStore);

            MyIdentityUser user = userManager.FindByName(HttpContext.User.Identity.Name);

            DbDataEntities dbTest = new DbDataEntities();
            List<Customer> model = null;

            if (userManager.IsInRole(user.Id, "Administrator"))
            {
                model = dbTest.Customers.ToList();
            }

            if (userManager.IsInRole(user.Id, "Operator"))
            {
                model = dbTest.Customers.Where(c => c.Country == "USA").ToList();
            }

            ViewBag.FullName = user.FullName;

            return View(model);
        }
    }
}