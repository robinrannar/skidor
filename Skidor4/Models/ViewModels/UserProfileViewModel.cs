using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skidor4.Data;

namespace Skidor4.Models.ViewModels
{
    public class UserProfileViewModel
    {
        DbChipOperation dbchips = new DbChipOperation();

        public persons person;
        public List<chiplink> chips;
        public chiplink chip;
        public string onlyDate;


        public UserProfileViewModel(persons p)
        {
            persons person = new persons();
            List<chips> chips = new List<chips>();
            chips chip = new chips();

            CombineUserAndChips(p);
        }

        public void CombineUserAndChips(persons p)
        {
            chips = dbchips.GetAllChipsByPersonID(p.pk_id);

            foreach (var item in chips)
            {
                if (item.enddate < DateTime.Today)
                {
                    item.isActive = false;
                }
                else
                {
                    item.isActive = true;
                }
                onlyDate = ((DateTime)item.enddate).ToString("dd/MM/yyyy");
            }

            chip = chips.OrderBy(x => x.enddate).FirstOrDefault();

            person = p;
        }
    }
}