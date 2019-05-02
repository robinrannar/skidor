using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skidor4.Models;
using Skidor4.Data;
using System.Web.Security;
using System.Reflection;
using Skidor4.Models.ViewModels;
using Skidor4.Authentication;

namespace Skidor4.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        DbChipOperation dbChip = new DbChipOperation();
        DbPriceOperation dbPrice = new DbPriceOperation();
        DbPersonOperation dbpersons = new DbPersonOperation();
        DbScoreboardOperation dbscoreboards = new DbScoreboardOperation();
        Methods method = new Methods();

        //Kalla på metoder här om du inte vet var lägg dom annars längst ner så städas det upp senare.

        // <----------------------------------------------------->
        #region ENDAST HTTPGET METODER I DENNA REGION
        //Hämtar användare för uppdatering
        [HttpGet]
        public ActionResult EditUser()
        {
            var sId = Request.Cookies["UserInfo"].Value;
            var id = sId.Replace("Id=", "");
            int idTest = int.Parse(id);

            var personToEdit = dbpersons.getPersonById(idTest);
            
            return View(personToEdit);
        }
        //Hämtar statistic och skriver ut
        [HttpGet]
        public ActionResult UserStatistics(int Id)
        {
            var userstatistics = dbscoreboards.ScoreboardDetails(Id);
            return View(userstatistics);
        }
        //Hämtar persnlig scoreboard och skriver ut
        [HttpGet]
        public ActionResult Scoreboard()
        {
            var sId = Request.Cookies["UserInfo"].Value;
            var id = sId.Replace("Id=", "");
            int idTest = int.Parse(id);

            var person = dbpersons.getPersonById(idTest);
            var chipList = dbChip.GetAllChipsByPersonID(idTest);

            List<scoreboard> scoreList = new List<scoreboard>();

            foreach (var i in chipList)
            {
                var scores = dbscoreboards.GetPersonalScoreboard(i.chip_id);

                foreach (var b in scores)
                {
                    scoreList.Add(b);
                }
            }
            

            return View(scoreList);
        }

        // öppnar chiphantering
        [HttpGet]
        public ActionResult ConnectChip()
        {
            chips chip = new chips();

            return View(chip);
        }
        #endregion
        // <----------------------------------------------------->

        // <----------------------------------------------------->
        #region ENDAST HTTPPOST METODER I DENNA REGION
        //Uppdaterar användare
        [HttpPost]
        public ActionResult EditUser(persons p)
        {
            var personToEdit = dbpersons.UpdatePerson(p);

            return RedirectToAction("Profile");

        }
        #endregion
        // <----------------------------------------------------->

        // <----------------------------------------------------->
        #region ENDAST ÖVRIGA METODER I DENNA REGION
        // öppnar användarprofil
        public new ActionResult Profile()
        {
            var sId = Request.Cookies["UserInfo"].Value;
            var id = sId.Replace("Id=", "");
            int idTest = int.Parse(id);

            var user = dbpersons.getPersonById(idTest);

            UserProfileViewModel vm = new UserProfileViewModel(user);

            if (vm == null)
                return HttpNotFound();

            return View(vm);
        }

        // kopplar chip till användare
        [HttpPost]
        public ActionResult ConnectChip(chips c)
        {
            var sId = Request.Cookies["UserInfo"].Value;
            var id = sId.Replace("Id=", "");
            int idTest = int.Parse(id);

            dbChip.ConnectChipAndUser(c, idTest);

            return RedirectToAction("Profile");
        }
        #endregion
        // <----------------------------------------------------->
    }
}