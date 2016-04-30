using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using IdentityOwinMVC.Models;
using IdentityOwinMVC.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace IdentityOwinMVC.Controllers
{

    public class AccountController : Controller
    {
        //Two member variable,
        private UserManager<MyIdentityUser> userManager;
        private RoleManager<MyIdentityRole> roleManager;

        //Creation of an instance of our custom DbContext class
        //passe it to the constructor of UserStore and RoleStore classes
        //UserStore and RoleStore perform database storage and retrieval tasks
        public AccountController()
        {
            var db = new MyIdentityDbContext();

            var userStore = new UserStore<MyIdentityUser>(db);
            userManager = new UserManager<MyIdentityUser>(userStore);

            var roleStore = new RoleStore<MyIdentityRole>(db);
            roleManager = new RoleManager<MyIdentityRole>(roleStore);

        }

        /**********/
        /*REGISTER*/
        /**********/

        public ActionResult Register()
        {
            return View();
        }


        //creation of an instance of MyIdentityUser
        //Set properties to the corresponding properties of the Register model class
        //Creation of the new user account with Create() method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new MyIdentityUser();

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.BirthDate = model.BirthDate;
                user.Bio = model.Bio;

                IdentityResult result = userManager.Create(user, model.Password);

                //redirection to login page
                if (result.Succeeded)
                {
                    //Pour l'exemple on donne les droits admin au nouvel utilisateur
                    userManager.AddToRole(user.Id, "Administrator");
                    return RedirectToAction("Login","Account");
                }
                else
                {
                    ModelState.AddModelError("UserName", "Error while creating the user!");
                }
            }
            return View(model);
        }


        /*******/
        /*LOGIN*/
        /*******/

        //User tries to access a URL without loggin ? redirection to login page.
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //validate whether user name and password are correct.
        //Done by finding a user with a specified user name and password using UserManager object
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                MyIdentityUser user = userManager.Find(model.UserName, model.Password);
                if (user != null)
                {
                    //if OK
                    //Retrieve an authentication manager and store in variable authenticationManager
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = model.RememberMe;
                    authenticationManager.SignIn(props, identity);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }


        /****************/
        /*ChangePassword*/
        /****************/

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = userManager.FindByName(HttpContext.User.Identity.Name);
                IdentityResult result = userManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Error while changing the password.");
                }
            }
            return View(model);
        }


        /****************/
        /*ChangeProfile**/
        /****************/

        [Authorize]
        public ActionResult ChangeProfile()
        {
            MyIdentityUser user = userManager.FindByName(HttpContext.User.Identity.Name);
            ChangeProfile model = new ChangeProfile();
            model.FullName = user.FullName;
            model.BirthDate = user.BirthDate;
            model.Bio = user.Bio;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfile(ChangeProfile model)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = userManager.FindByName(HttpContext.User.Identity.Name);
                user.FullName = model.FullName;
                user.BirthDate = model.BirthDate;
                user.Bio = model.Bio;
                IdentityResult result = userManager.Update(user);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Profile updated successfully.";
                }
                else
                {
                    ModelState.AddModelError("", "Error while saving profile.");
                }
            }
            return View(model);
        }


        /*********/
        /*LogOut**/
        /*********/

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}