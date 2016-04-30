﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityOwinMVC.Security
{
    //represent basic role entity
    public class MyIdentityRole : IdentityRole
    {
        public MyIdentityRole()
        {

        }

        public MyIdentityRole(string roleName)
            : base(roleName)
        {

        }

        public MyIdentityRole(string roleName, string description)
            : base(roleName)
        {
            this.Description = description;
        }


        public string Description { get; set; }
    }
}