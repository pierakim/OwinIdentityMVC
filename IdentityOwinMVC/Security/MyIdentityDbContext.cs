using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityOwinMVC.Security
{
    //specifie un type generique de MyIdentityUser.
    //
    public class MyIdentityDbContext : IdentityDbContext<MyIdentityUser>
    {
        //Use DefaultConnection with localDb for IdentityUser
        //Connection string in Web.config
        public MyIdentityDbContext()
            : base("DefaultConnection")
        {

        }
    }
}