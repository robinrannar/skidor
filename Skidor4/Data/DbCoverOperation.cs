using Skidor4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skidor4.Data
{
    public class DbCoverOperation
    {
        db4Entities dbcalls = new db4Entities();
        DbChipOperation dbchip = new DbChipOperation();

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT HÄMTA FRÅN DATABASEN
        //metod för att hämta statistik
        public List<covers> getOwnStatistics(int id)
        {
            var result = dbcalls.covers.Where(x => x.fk_chip_id == id).ToList();
            return result;
        }
        //Hämta person utifrån chipId
        public List<covers> GetCoversByChip(int id)
        {
            var result = dbcalls.covers.Where(i => i.fk_chip_id == id).ToList();

            return result;
        }
        //metod för att hämta tjuvåkare
        public Tuple<List<covers>, List<covers>, List<covers>> GetNoChipRegistrations()
        {
            DateTime d = DateTime.Today;
            DateTime seasonStart = new DateTime(2017, 9, 1, 0, 0, 0);

            List<covers> dayList = new List<covers>();
            var dList = dbcalls.covers.Where(x => x.time >= d).ToList();

            foreach (covers c in dList)
            {
                if (c.fk_chip_id == null)
                {
                    dayList.Add(c);
                }
            }

            d = d.AddDays(-7);
            List<covers> monthList = new List<covers>();
            var mList = dbcalls.covers.Where(x => x.time >= d).ToList();

            foreach (covers c in mList)
            {
                if (c.fk_chip_id == null)
                {
                    monthList.Add(c);
                } 
            }

            d = d.AddDays(-7);
            List<covers> seasonList = new List<covers>();
            var sList = dbcalls.covers.Where(x => x.time >= seasonStart).ToList();

            foreach (covers c in sList)
            {
                if (c.fk_chip_id == null)
                {
                    seasonList.Add(c);
                }
            }

            Tuple<List<covers>, List<covers>, List<covers>> coverDateList = new Tuple<List<covers>, List<covers>, List<covers>>(dayList, monthList, seasonList);

            return coverDateList;
        }
        //metod för att hämta alla personer i databasen
        public List<covers> GetStationRegistrations()
        {
            List<covers> lc = new List<covers>();
            lc = dbcalls.covers.ToList();

            foreach (covers c in lc)
            {
                if (c.fk_chip_id == null)
                {
                    c.noChip = "Ej reg";
                }
                else
                {
                    c.noChip = c.fk_chip_id.ToString();
                    c.isActive = dbchip.GetChipsById(c);

                }
            }
            return lc;
        }
        //metod för att lägga till personer i databasen
        public covers AddCovers(covers c)
        {
            dbcalls.covers.Add(c);
            dbcalls.SaveChanges();

            return c;
        }
        #endregion
        // <----------------------------------------------------->
    }
}