using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Skidor4.Controllers;
using System.Web.Mvc;
using Skidor4.Models;
using Skidor4.Data;


namespace Skidor4.Data
{
    public class Methods : Controller
    {
        db4Entities dbCalls = new db4Entities();

        //Metod för att hämta den satta Cookien
        public int GetCookieID()
        {
            var sId = Request.Cookies["UserInfo"].Value;
            var id = sId.Replace("Id=", "");
            int idTest = int.Parse(id);

            return idTest;
        }

        public List<covers> SetCoverListNewDates(List<covers> list, DateTime date)
        {
            List<covers> newList = new List<covers>();
            string newDate = date.ToShortDateString();
            

            foreach (covers c in list)
            {
                var eq = dbCalls.equipment.FirstOrDefault(x => x.fk_chip_id == c.fk_chip_id);

                if (eq != null)
                {
                    c.isEquipment = true;
                }
                else
                {
                    c.isEquipment = false;
                }

                c.timeTest = (DateTime)c.time;
                c.onlyDate = c.timeTest.ToShortDateString();
                newList.Add(c);
            }
            return newList;
        }

        public string GetSpeed(double sum, string length)
        {
            string speed = "";
            speed = Math.Round(((Convert.ToDouble(length) / (sum)) * 3.6), 1).ToString();

            return speed;
        }
    }
}