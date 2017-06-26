using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADE_ManagementSystem.Models.User
{
    public class UserViewModel : AspNetUser
    {
        public string UserRole { get; set; }
    }
}