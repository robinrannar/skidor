using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Skidor4.Data;
using Skidor4.Models;

namespace Skidor4.Models.MyRoleProvider
{
    public class SiteRole : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            db4Entities dbcalls = new db4Entities();
            string data = dbcalls.persons.Where(x => x.alias == username).FirstOrDefault().permissions.rolename;
            string[] result = { data };
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        //Checks if an user is authorized or not
        public override bool IsUserInRole(string username, string roleName)
        {
            db4Entities dbcalls = new db4Entities();
            var data = dbcalls.persons.Where(x => x.alias == username && x.permissions.rolename == roleName).FirstOrDefault();

            if (data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
