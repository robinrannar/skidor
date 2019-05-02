using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Skidor4.Data;
using System.Web.Mvc;

namespace Skidor4.Authentication
{
    public class AuthenticationRole : AuthorizeAttribute
    {
        private readonly string[] userAssignedRole;
        private DbPersonOperation db = new DbPersonOperation();

        public AuthenticationRole(params string[] roles)
        {
            this.userAssignedRole = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            foreach (var roles in userAssignedRole)
            {
                authorize = db.CheckUserRole(httpContext.User.Identity.Name, roles);
                if (authorize)
                    return authorize;
            }
            return authorize;
        }
    }
}