using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skidor4.Data;

namespace Skidor4.Models.ViewModels
{
    public class EditChipViewModel
    {
        public chips Chips { get; set; }
        public prices Price { get; set; }
        public persons Person { get; set; }
        public chiplink chiplink { get; set; }
        public chiplink chip { get; set; }
        public IEnumerable<SelectListItem> PriceCategories { get; set; }
        public int SelectedPriceCategory { get; set; }
        public IEnumerable<SelectListItem> UnUsedChips { get; set; }
        public int SelectedUnUsedChip { get; set; }
        public IEnumerable<SelectListItem> UsedChips { get; set; }
        public int SelectedUsedChip { get; set; }
        public string password { get; set; }
        public bool paid { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public persons chipPerson { get; set; }
    }
}