using IdentityOwinMVC;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace IdentityOwinMVC
{
    //Call configuration method with IAppBuilder object
    //Set up scheme authentification
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/account/login")
            };
            app.UseCookieAuthentication(options);

        }
    }
}