using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Skidor4.Authentication;
using Skidor4.Models;
using Skidor4.Models.ViewModels;
using Skidor4.Data;

namespace Skidor4.Controllers
{
    [AuthenticationRole("Manager")]
    public class ManagerController : Controller
    {
        DbPersonOperation dbpersons = new DbPersonOperation();
        DbPermissionOperation dbPermission = new DbPermissionOperation();
        //Metod för att hämta AdminLayout

        //hämtar användare till lista, inkl filtrering
        public ActionResult Users(string searchInput)
        {
            if (searchInput == null)
            {
                return View(dbpersons.GetPersons());
            }
            else
            {
                return View(dbpersons.GetPersonsByEmailSearch(searchInput));
            }

        }
        //metod för att filtrera sökreultat(om det hinns med ska den lyftas ut)
        public JsonResult GetSearchingData(string SearchBy, string SearchValue)
        {
            List<persons> PersonList = new List<persons>();
            if (SearchBy == "ID")
            {
                try
                {
                    int Id = Convert.ToInt32(SearchValue);
                    PersonList = dbpersons.GetPersons().Where(x => x.pk_id == Id || SearchValue == null).ToList();
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} är inget ID", SearchValue);
                }
                return Json(PersonList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                PersonList = dbpersons.GetPersons().Where(x => x.lastname.StartsWith(SearchValue) || SearchValue == null).ToList();
                return Json(PersonList, JsonRequestBehavior.AllowGet);
            }
        }

        //Underlag för registering
        [HttpGet]
        public ActionResult Registration()
        {
            var permissionList = dbPermission.GetPermissions();
            var viewModel = new CreateUserViewModel() { Permissions = permissionList };

            return View(viewModel);
        }
        //Metod för att väljer vilken person som ska redigeras
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var personToEdit = dbpersons.getPersonById(id);
            var permissionList = dbPermission.GetPermissions();
            var role = dbpersons.GetRole(personToEdit);
            var viewModel = new CreateUserViewModel
            {
                Person = personToEdit,
                Permission = role,
                Permissions = permissionList
            };

            return View(viewModel);
        }
        public ActionResult Statistics()
        {
            string date = "";
            ManagerStatisticsViewModel vm = new ManagerStatisticsViewModel(date);

            return View(vm);
        }

        public ActionResult FilterTracks()
        {
            string date = "";
            ManagerStatisticsViewModel vm = new ManagerStatisticsViewModel(date);

            return PartialView("FilterTrackStatisticsVM", vm);
        }

        public ActionResult TracksByCondition(string date)
        {
            ManagerStatisticsViewModel vm = new ManagerStatisticsViewModel(date);

            return PartialView("FilteredTrackStatisticsVM", vm);
        }

        public ActionResult FilterCards()
        {
            string date = "";
            ManagerStatisticsViewModel vm = new ManagerStatisticsViewModel(date);

            return PartialView("FilterCardStatisticsVM", vm);
        }

        public ActionResult CardsByCondition(string date)
        {
            ManagerStatisticsViewModel vm = new ManagerStatisticsViewModel(date);

            return PartialView("FilteredCardStatisticsVM", vm);
        }

        [HttpPost]
        public ActionResult GetTracks(string date)
        {
            if (date == null)
            {
                date = "Idag";
            }

            TrackDiagramVM vm = new TrackDiagramVM();
            var list = vm.SetTracks(date);
            string serialized = JsonConvert.SerializeObject(list);

            return Json(serialized);
        }

        [HttpPost]
        public ActionResult GetCards(string date)
        {
            if (date == null)
            {
                date = "Idag";
            }

            TrackDiagramVM vm = new TrackDiagramVM();
            var list = vm.SetCardList(date);
            string serialized = JsonConvert.SerializeObject(list);

            return Json(serialized);
        }

        //Metod för att registrerar en ny person i systemet
        [HttpPost]
        public ActionResult Registration(CreateUserViewModel newperson)
        {
            if (ModelState.IsValid)
            {
                persons p = new persons();
                p = newperson.Person;
                int selectedPermission = newperson.chosenPermission;
                p.fk_permission_id = selectedPermission;
                dbpersons.Addpersons(p);
            }
            ModelState.Clear();
            return RedirectToAction("Users");
        }
        //Metod för att uppdatera personen
        [HttpPost]
        public ActionResult Edit(CreateUserViewModel p)
        {
            var personToEdit = dbpersons.UpdatePersonAdmin(p);
            var permissionList = dbPermission.GetPermissions();

            return RedirectToAction("Users");


        }

    }
}