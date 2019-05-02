using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skidor4.Models;
using Skidor4.Data;
using Skidor4.Models.ViewModels;
using Skidor4.Authentication;

namespace Skidor4.Controllers
{
    [AuthenticationRole("Salesman")]
    public class SalesmanController : Controller
    {
        DbChipOperation dbchips = new DbChipOperation();
        DbPersonOperation dbpersons = new DbPersonOperation();
        DbPriceOperation dbprices = new DbPriceOperation();

        // GET: Salesman
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Sales()
        {

            List<SelectListItem> pCategories = dbprices.GetPrices().Select(x => new SelectListItem
            {
                Value = x.pk_id.ToString(),
                Text = x.name.ToString()
            }).ToList();

            List<SelectListItem> unSelectedChips = dbchips.unLinkedChips().Select(x => new SelectListItem
            {
                Value = x.pk_id.ToString(),
                Text = x.pk_id.ToString()
            }).ToList();

            List<SelectListItem> SelectedChips = dbchips.LinkedChips().Select(x => new SelectListItem
            {
                Value = x.pk_id.ToString(),
                Text = x.pk_id.ToString()
            }).ToList();
            var sVM = new EditChipViewModel() { PriceCategories =  pCategories, UnUsedChips = unSelectedChips};

            return View(sVM);
        }

        [HttpGet]
        public ActionResult UpdateSales()
        {
            List<SelectListItem> pCategories = dbprices.GetPrices().Select(x => new SelectListItem
            {
                Value = x.pk_id.ToString(),
                Text = x.name.ToString()
            }).ToList();

            List<SelectListItem> SelectedChips = dbchips.LinkedChips().Select(x => new SelectListItem
            {
                Value = x.pk_id.ToString(),
                Text = x.pk_id.ToString()
            }).ToList();
            var uVM = new EditChipViewModel() { PriceCategories = pCategories, UsedChips = SelectedChips };

            return View(uVM);
        }

        [HttpPost]
        public ActionResult Sales(EditChipViewModel newchip)
        {
            if (ModelState.IsValid)
            {
                EditChipViewModel newperson = new EditChipViewModel();
               newperson = dbchips.SaleChip(newchip);
            }
            ModelState.Clear();
            return RedirectToAction("Sales");
        }

        public ActionResult GetPersonId(int id)
        {
            List<SelectListItem> pCategories = dbprices.GetPrices().Select(x => new SelectListItem
            {
                Value = x.pk_id.ToString(),
                Text = x.name.ToString()
            }).ToList();

            List<SelectListItem> SelectedChips = dbchips.LinkedChips().Select(x => new SelectListItem
            {
                Value = x.pk_id.ToString(),
                Text = x.pk_id.ToString()
            }).ToList();
            var uVM = new EditChipViewModel() { PriceCategories = pCategories, UsedChips = SelectedChips };

            var p = dbpersons.GetPersonByChip(id);
            uVM.chipPerson = p.persons;

            return View("UpdateSales", uVM);
        }

        [HttpPost]
        public ActionResult UpdateSales(EditChipViewModel newchip)
        {
            if (ModelState.IsValid)
            {
                EditChipViewModel newperson = new EditChipViewModel();
                newperson = dbchips.LinkPersonAndChip(newchip);
            }
            ModelState.Clear();
            return RedirectToAction("UpdateSales");
        }
    }
}