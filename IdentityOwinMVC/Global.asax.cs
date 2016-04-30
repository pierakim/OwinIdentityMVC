using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IdentityOwinMVC.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityOwinMVC
{
    //contains code for responding to application-level events raised by ASP.NET or by HttpModules
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            MyIdentityDbContext db = new MyIdentityDbContext();
            RoleStore<MyIdentityRole> roleStore = new RoleStore<MyIdentityRole>(db);
            RoleManager<MyIdentityRole> roleManager = new RoleManager<MyIdentityRole>(roleStore);

            if (!roleManager.RoleExists("Administrator"))
            {
                MyIdentityRole newRole = new MyIdentityRole("Administrator", "Administrators can add, edit and delete data.");
                roleManager.Create(newRole);
            }

            if (!roleManager.RoleExists("Operator"))
            {
                MyIdentityRole newRole = new MyIdentityRole("Operator", "Operators can only add or edit data.");
                roleManager.Create(newRole);
            }

        }
    }
}
