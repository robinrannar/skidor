using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skidor4.Models;
using Skidor4.Data;
using System.Web.Security;
using Skidor4.Models.ViewModels;
using Skidor4.Authentication;

namespace Skidor4.Controllers
{
    [AuthenticationRole("Admin")]
    public class AdminController : Controller
    {
        DbPersonOperation dbpersons = new DbPersonOperation();
        DbChipOperation dbchips = new DbChipOperation();    
        DbPriceOperation dbprices = new DbPriceOperation();
        DbCoverOperation dbcovers = new DbCoverOperation();
        DbEquipmentOperation dbequipment = new DbEquipmentOperation();
        DbTrackOperation dbtrack = new DbTrackOperation();
        DbPermissionOperation dbPermission = new DbPermissionOperation();
        Methods method = new Methods();


        //Kalla på metoder här om du inte vet var lägg dom annars längst ner så städas det upp senare.

        // <----------------------------------------------------->
        #region ENDAST HTTPGET METODER I DENNA REGION


        //Metod som hämtar vilket equipment som skall redigeras
        [HttpGet]
        public ActionResult EditEquipment(int id)
        {
            var equipmentToEdit = dbequipment.GetSpecificEquipment(id);

            return View(equipmentToEdit);
        }

        //Metod som tar in ett chip id att jobba med.
        [HttpGet]
        public ActionResult EditChips(int id)
        {            
            var chipToEdit = dbchips.getChipById(id);
            var personToEdit = dbpersons.GetPersonByChip(id);
            var priceByChipId = dbchips.GetPriceByChipId(id); 
            var getCategories = dbprices.GetPrices();
            var viewModel = new EditChipViewModel() { Chips = chipToEdit,
                                                      chiplink = personToEdit,
                                                      chip = priceByChipId
                                                    };
            viewModel.PriceCategories = dbprices.GetPrices().Select(x => new SelectListItem { Value = x.pk_id.ToString(),
                                                                                      Text = x.name.ToString() }).ToList();

            return View(viewModel);
        }

        // Metod för att hämta alla chip ur databasen
        [HttpGet]
        public ActionResult Chips()
        {
            var chip = dbchips.GetChips();
            return View(chip.ToList());
        }
        //Metod för att  visa stationensregistrationer
        [HttpGet]
        public ActionResult StationRegistration()
        {
            var stationRegistrations = dbcovers.GetStationRegistrations();

            return View(stationRegistrations);
        }

        //Metod för att hämta AdminLayout
        [HttpGet]
        public ActionResult Equipment()
        {
            var equipment = dbequipment.GetAllEquipment();

            return View(equipment.ToList());
        }

        [HttpGet]
        public ActionResult EquipmentRegistration()
        {
            equipment newEquipment = new equipment();

            return View(newEquipment);
        }
        //Underlag för covers
        [HttpGet]
        public ActionResult AddStationRegistration()
        {
            covers c = new covers();
            return View(c);
        }
        #endregion
        // <----------------------------------------------------->

        // <----------------------------------------------------->
        #region ENDAST HTTPPOST METODER I DENNA REGION
       
        //Metod för att redigera chip
        [HttpPost]
        public ActionResult EditChips(EditChipViewModel c)
        {
            var chipToEdit = dbchips.UpdateChip(c);
            if (chipToEdit == null) return RedirectToAction("Chips");

            var getCategories = dbprices.GetPrices();
            var viewModel = new EditChipViewModel();

            viewModel.PriceCategories = dbprices.GetPrices().Select(x => new SelectListItem
                                                                                    {
                                                                                        Value = x.pk_id.ToString(),
                                                                                        Text = x.name.ToString()
                                                                                    }).ToList();
                                                                                   
            return View(viewModel);
        }

        //Metod för att redigera equipment
        [HttpPost]
        public ActionResult EditEquipment(equipment eq)
        {
            var eqToEdit = dbequipment.UpdateEquipment(eq);

            if (eqToEdit == null) return RedirectToAction("Equipment");

            return View(eqToEdit);
        }

        //Metod för att registrer ett nytt redskap i systemet
        [HttpPost]
        public ActionResult EquipmentRegistration(equipment eq)
        {
            if (ModelState.IsValid)
            {
                chips chip = new chips();
                chip = dbchips.NewChip();
           
                eq.fk_chip_id = chip.pk_id;
                dbequipment.NewEquipment(eq);
            }
            ModelState.Clear();
            return RedirectToAction("Equipment");
        }

        //Metod för att registrerar en cover och nytt score
        [HttpPost]
        public ActionResult AddStationRegistration(covers c)
        {
            if (ModelState.IsValid)
            {
                var time = c.time;
                int id = (int)c.fk_chip_id;

                dbcovers.AddCovers(c);

                var userstatistics = dbcovers.getOwnStatistics(id);
                dbtrack.setTrackTimes(userstatistics, (DateTime)time);
            }
            ModelState.Clear();

            return RedirectToAction("StationRegistration");
        }
        #endregion
        // <----------------------------------------------------->

        // <----------------------------------------------------->
        #region ENDAST ÖVRIGA METODER I DENNA REGION

        #endregion
        // <----------------------------------------------------->
    }
}