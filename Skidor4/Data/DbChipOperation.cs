using Skidor4.Models;
using Skidor4.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skidor4.Data
{
    public class DbChipOperation
    {
        db4Entities dbcalls = new db4Entities();

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT HÄMTA FRÅN DATABASEN
        //metod för att hämta alla chip i databasen
        public List<chiplink> GetChips()
        {
            return dbcalls.chiplink.ToList();
        }
        //Metod för att hämta pris för specifikt chipid
        public chiplink GetPriceByChipId(int id)
        {
            return dbcalls.chiplink.FirstOrDefault(p => p.chip_id == id);
        }
        //metod för att hämta ett specifik chips ur databasen
        public chips getChipById(int id)
        {
            var result = dbcalls.chips.FirstOrDefault(i => i.pk_id == id);
            return result;
        }
        public List<chips> unLinkedChips()
        {
            List<chips> unusedchips = new List<chips>();
            var allchip = dbcalls.chips.ToList();
            var linkchips = dbcalls.chiplink.Select(x => x.chip_id).ToList();
            foreach (chips c in allchip)
            {
                if (linkchips.Contains(c.pk_id))
                {
                    //do nothing
                }
                else
                {
                    unusedchips.Add(c);
                }
            }
            return unusedchips;
        }
        public List<chips> LinkedChips()
        {
            List<chips> usedchips = new List<chips>();
            var allchip = dbcalls.chips.ToList();
            var linkchips = dbcalls.chiplink.Select(x => x.chip_id).ToList();
            foreach (chips c in allchip)
            {
                if (linkchips.Contains(c.pk_id))
                {
                    usedchips.Add(c);
                }
                else
                {
                    //do nothing
                }
                
            }
            return usedchips;
        }
        //metod för att hämta ett specifik chips ur databasen
        public chiplink getChipByNullableId(int? id)
        {
            var result = dbcalls.chiplink.FirstOrDefault(i => i.chip_id == id);
            return result;
        }
        //Hämtar om chip som registreras är aktiva eller ej
        public bool GetChipsById(covers c)
        {
            var chip = dbcalls.chiplink.Where(x => x.chip_id == c.fk_chip_id).OrderByDescending(x => x.enddate).FirstOrDefault();
            DateTime? startDate = chip.startdate;
            DateTime? endDate = chip.enddate;
            DateTime? regTime = c.time;


            if (regTime >= startDate && regTime <= endDate)
            {
                chip.isActive = true;
            }
            else
            {
                chip.isActive = false;
            }
            return chip.isActive;
        }

        //hämtar alla chips som hör till en specifik person
        public List<chiplink> GetAllChipsByPersonID(int id)
        {
            return dbcalls.chiplink.Where(p => p.person_id == id).ToList();
        }

        //Metod för att hämta pris för specifikt chipid
        public chiplink GetChipLink(int personId)
        {
            return dbcalls.chiplink.Where(x => x.person_id == personId).OrderByDescending(x => x.enddate).FirstOrDefault();
        }

        //metod för att hämta scoreboard beroende på dag
        public List<chiplink> GetChipByDateCondition(string date)
        {
            DateTime d = DateTime.Today;
            DateTime seasonStart = new DateTime(2017, 9, 1, 0, 0, 0);
            List<chiplink> sList = new List<chiplink>();
            scoreboard s = new scoreboard();


            if (date == "Idag" || date == null)
            {
                sList = dbcalls.chiplink.Where(x => x.startdate >= d).ToList();
            }

            else if (date == "Denna veckan")
            {
                d = d.AddDays(-7);
                sList = dbcalls.chiplink.Where(x => x.startdate >= d).ToList();
            }
            else if (date == "Denna säsongen")
            {
                sList = dbcalls.chiplink.Where(x => x.startdate >= seasonStart).ToList();
            }

            return sList;
        }

        //metod för att hämta sålda chip
        public Tuple<List<chiplink>, List<chiplink>, List<chiplink>> GetSoldChipsByDate()
        {
            DateTime d = DateTime.Today;
            DateTime seasonStart = new DateTime(2017, 9, 1, 0, 0, 0);
            List<int> idList = new List<int>();

            var listDay = dbcalls.chiplink.Where(x => x.startdate >= d).ToList();   
            List<chiplink> dayList = new List<chiplink>();

            foreach (var p in listDay)
            {
                if (!idList.Contains(p.chip_id))
                {
                    dayList.Add(p);
                    idList.Add(p.chip_id);
                }
            }

            idList.Clear();
            d = d.AddDays(-7);
            List<chiplink> monthList = new List<chiplink>();
            var listMonth = dbcalls.chiplink.Where(x => x.startdate >= d).ToList();

            foreach (var p in listMonth)
            {
                if (!idList.Contains(p.chip_id))
                {
                    monthList.Add(p);
                    idList.Add(p.chip_id);
                }
            }

            idList.Clear();
            List<chiplink> seasonList = new List<chiplink>();
            var listSeason = dbcalls.chiplink.Where(x => x.startdate >= seasonStart).ToList();

            foreach (var p in listSeason)
            {
                if (!idList.Contains(p.chip_id))
                {
                    seasonList.Add(p);
                    idList.Add(p.chip_id);
                }
            }

            Tuple<List<chiplink>, List<chiplink>, List<chiplink>> soldChipDateList = new Tuple<List<chiplink>, List<chiplink>, List<chiplink>>(dayList, monthList, seasonList);

            return soldChipDateList;
        }
        #endregion
        // <----------------------------------------------------->

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT LÄGGA TILL I DATABASEN
        //metod för att updatera chip i databasen
        public EditChipViewModel UpdateChip(EditChipViewModel chip)
        {
            var c = dbcalls.chiplink.Single(x => x.chip_id == chip.Chips.pk_id && x.person_id == x.persons.pk_id);
            c.startdate = chip.startdate;
            c.enddate = chip.enddate;
            c.chips.paid = chip.paid;
            c.persons.firstname = chip.chiplink.persons.firstname;
            c.persons.lastname = chip.chiplink.persons.lastname;
            chip.chiplink.price_id = chip.SelectedPriceCategory;


            dbcalls.SaveChanges();

            return chip;
        }

        //Metod för att länka person till chip
        public EditChipViewModel SaleChip(EditChipViewModel newchip)
        {
            chiplink newlink = new chiplink();
            persons newperson = new persons();
            newlink.chip_id = newchip.SelectedUnUsedChip;
            newlink.price_id = newchip.SelectedPriceCategory;
            newlink.startdate = newchip.startdate;
            newlink.enddate = newchip.enddate;
            newperson.alias = newchip.SelectedUnUsedChip.ToString();
            newperson.password = newchip.password;
            newperson.zipcode = Convert.ToInt32(newchip.password);
            newperson.fk_permission_id = 8;

            dbcalls.persons.Add(newperson);
            dbcalls.chiplink.Add(newlink);
            dbcalls.SaveChanges();

            return newchip;
        }
        public EditChipViewModel LinkPersonAndChip(EditChipViewModel newchip)
        {
            chiplink newlink = new chiplink();
            newlink.chip_id = newchip.SelectedUsedChip;
            newlink.person_id = newchip.chiplink.person_id;
            newlink.price_id = newchip.SelectedPriceCategory;
            newlink.startdate = newchip.startdate;
            newlink.enddate = newchip.enddate;

            dbcalls.chiplink.Add(newlink);
            dbcalls.SaveChanges();

            return newchip;
        }

        //Metod för att skapa nytt chip.
        public chips NewChip()
        {
            chips newchips = new chips();

            dbcalls.chips.Add(newchips);
            dbcalls.SaveChanges();

            return newchips;
        }

        //Metod för att skapa nytt chip.
        public chiplink ConnectChipAndUser(chips c, int id)
        {
            var cLink = dbcalls.chiplink.Where(x => x.chip_id == c.pk_id).OrderByDescending(x => x.enddate).FirstOrDefault();

            if (cLink != null)
            {
                cLink.person_id = id;
                dbcalls.SaveChanges();
            }
       
            return cLink;
        }
        #endregion
        // <----------------------------------------------------->
    }
}