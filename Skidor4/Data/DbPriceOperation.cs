using Skidor4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skidor4.Data
{
    public class DbPriceOperation
    {
        db4Entities dbcalls = new db4Entities();


        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT HÄMTA FRÅN DATABASEN
        //metod för att hämta pris
        public List<prices> GetPrices()
        {
            return dbcalls.prices.ToList();
        }

        public prices GetPrice(int id)
        {
            var cl = dbcalls.chiplink.Where(x => x.person_id == id).OrderByDescending(x => x.enddate).FirstOrDefault();

            return dbcalls.prices.FirstOrDefault(x => x.pk_id == cl.price_id);
        }

        public prices GetPriceByPriceID(int id)
        {
            return dbcalls.prices.FirstOrDefault(x => x.pk_id == id);
        }

        #endregion
        // <----------------------------------------------------->
    }
}