using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADE_ManagementSystem.Global
{
    public class ProjectConstants
    {
        public class AccessLevel
        {
            public const int None = 0;
            public const int View = 1;
            public const int Create = 2;
            public const int Edit = 3;
            public const int Full = 4;
        }

        public const string UserProfileLink = "/Images/Users/dp"; // to use:

        public const string DefaultUserProfileLink = "/Images/Users/defaultpic.png"; 

        
    }
}