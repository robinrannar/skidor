using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Skidor4.Data;
using Skidor4.Models;
using Skidor4.Models.ViewModels;

namespace Skidor4.Controllers
{
    //kalla på metoderna här, skapa inga nya
    public class HomeController : Controller
    {
        DbPersonOperation dbpersons = new DbPersonOperation();
        DbTrackOperation dbtracks = new DbTrackOperation();
        DbScoreboardOperation dbscores = new DbScoreboardOperation();
        DbChipOperation dbChips = new DbChipOperation();
        DbPermissionOperation dbPermission = new DbPermissionOperation();

        //Metod för att visa Index
        public ActionResult Index()
        {
            int idTest = 0;

            if (Response.Cookies.AllKeys.Contains("UserInfo"))
            {
                var sId = Request.Cookies["UserInfo"].Value;
                var id = sId.Replace("Id=", "");
                idTest = int.Parse(id);
            }
            else
            {
                idTest = 0;
            }
            //var sId = Request.Cookies["UserInfo"].Value;
            //var id = sId.Replace("Id=", "");
            //int idTest = int.Parse(id);
            var track = dbtracks.GetTracks();
            var persons = dbpersons.GetPublicPersons();

            var viewModel = new PublicHomeViewModel("Idag", "Alla spår", idTest) { Tracks = track, Persons = persons };
       
            return View(viewModel);
        }
        //Metod för att komma till spår
        public ActionResult Track()
        {
            var track = dbtracks.GetTracks();
            return View(track.ToList());
        }
        //Metod för att logga in
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                if (dbpersons.UserCheck(login.alias, login.password))
                {
                    var person = dbpersons.GetPersonByAliasAndPass(login.alias, login.password);
                    login.Role = dbpersons.GetRole(person);

                    FormsAuthentication.SetAuthCookie(person.alias, false);

                    if (person != null)
                    {
                        SetCookie(login);
                        return RedirectToAction("Index");
                    }

                }
                else
                    ModelState.AddModelError("", "Var vänlig att kontrollera dina användaruppgifter");
            }
            return RedirectToAction("Index", "Home");
        }
        //Metod för att logga ut
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        //Metod för att sätta cookie
        public void SetCookie(LoginViewModel lvm)
        {
            persons p = dbpersons.GetPersonByAliasAndPass(lvm.alias, lvm.password);

            HttpCookie myCookie = new HttpCookie("UserInfo");
            myCookie["Id"] = p.pk_id.ToString();
            myCookie.Expires = DateTime.Now.AddDays(1d);
            Response.Cookies.Add(myCookie);
        }

        //Metod för att registrera sig
        [HttpGet]
        public ActionResult Register()
        {
            PublicHomeViewModel vm = new PublicHomeViewModel("Idag", "Alla spår", 0);
            vm.GetPerson();
            return View(vm);
        }

        //Metod för att registrera sig
        [HttpPost]
        public ActionResult Register(CreateUserViewModel newperson)
        {
            if (ModelState.IsValid)
            {
                persons p = new persons();
                p = newperson.Person;
                p.fk_permission_id = 8;
                dbpersons.Addpersons(p);
            }
            ModelState.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult FilterScoreboard()
        {
            int idTest = 0;

            if (Response.Cookies.AllKeys.Contains("UserInfo"))
            {
                var sId = Request.Cookies["UserInfo"].Value;
                var id = sId.Replace("Id=", "");
                idTest = int.Parse(id);
            }
            else
            {
                idTest = 0;
            }
            //var sId = Request.Cookies["UserInfo"].Value;
            //var id = sId.Replace("Id=", "");
            //int idTest = int.Parse(id);
            PublicHomeViewModel vm = new PublicHomeViewModel("Idag", "Alla spår", idTest);
         
            return PartialView("FilterScoreboardPV", vm);
        }

        public ActionResult ScoreboardByCondition(string date, string track, string filter)
        {
            int idTest = 0;

            if (HttpContext.Request.Cookies["UserInfo"] != null)
            {
                var sId = Request.Cookies["UserInfo"].Value;
                var id = sId.Replace("Id=", "");
                idTest = int.Parse(id);
            }

            var tracks = dbtracks.GetTracks();
            string alltracks = "";

            foreach (var t in tracks)
            {
                alltracks += t.name;
            }

            if(!alltracks.Contains(track))
            {
                track = "Alla spår";
            }

            PublicHomeViewModel vm = new PublicHomeViewModel(date, track, idTest); 
           
            return PartialView("FilteredScoreboardPV", vm);
        }

        [HttpGet]
        public ActionResult ShowMap(string name)
        {
            int idTest = 0;

            if (Response.Cookies.AllKeys.Contains("UserInfo"))
            {
                var sId = Request.Cookies["UserInfo"].Value;
                var id = sId.Replace("Id=", "");
                idTest = int.Parse(id);
            }
            
            if (name == null)
            {
                name = "Alla spår";
            }

            PublicHomeViewModel vm = new PublicHomeViewModel("Idag", name, idTest);
            vm.mapImgUrl = vm.GetMap(name);

            return PartialView("TrackMapPV", vm);
        }
    }
}