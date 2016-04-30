using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityOwinMVC.Security
{
    public class MyIdentityUser : IdentityUser
    {
        //IdentifyUser base has basic info : UserName,Email, PasswordHash
        //We had some properties to the basic model
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Bio { get; set; }
    }
}