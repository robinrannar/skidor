using Skidor4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skidor4.Data
{
    public class DbEquipmentOperation
    {
        db4Entities dbcalls = new db4Entities();

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT HÄMTA FRÅN DATABASEN
        //metod för att hämta lista av equipment i databasen
        public List<equipment> GetAllEquipment()
        {
            return dbcalls.equipment.ToList();
        }
        //metod för att hämta equipment i databasen
        public equipment GetSpecificEquipment(int id)
        {
            return dbcalls.equipment.FirstOrDefault(x => x.pk_id == id);

        }
        #endregion
        // <----------------------------------------------------->

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT LÄGGA TILL I DATABASEN
        //metod för att uppdatera equipment i databasen
        public equipment UpdateEquipment(equipment eq)
        {
            var e = dbcalls.equipment.Single(x => x.pk_id == eq.pk_id);

            e.name = eq.name;
            e.fk_chip_id = eq.fk_chip_id;

            dbcalls.SaveChanges();

            return eq;
        }
        //Metod för att skapa nytt equipment
        public equipment NewEquipment(equipment e)
        {
            dbcalls.equipment.Add(e);
            dbcalls.SaveChanges();

            return e;
        }
        #endregion
        // <----------------------------------------------------->
    }
}