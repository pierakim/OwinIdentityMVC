﻿using System;

namespace IdentityOwinMVC.Models
{
    public class ChangeProfile
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Bio { get; set; }
    }
}