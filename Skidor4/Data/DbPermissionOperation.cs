using Skidor4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skidor4.Data
{
    public class DbPermissionOperation
    {
        db4Entities dbcalls = new db4Entities();

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT HÄMTA FRÅN DATABASEN
        //metod för att hämta roller
        public List<permissions> GetPermissions()
        {
            return dbcalls.permissions.ToList();
        }
        #endregion
        // <----------------------------------------------------->
    }
}
    
