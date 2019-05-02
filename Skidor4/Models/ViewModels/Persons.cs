using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skidor4.Models
{
    [MetadataType(typeof(PersonMetaData))]
    public partial class persons
    {
    }
    public class PersonMetaData
    {
        [Display(Name = "Förnamn")]
        public string firstname { get; set; }

        [Display(Name = "Efternamn")]
        public string lastname { get; set; }

        [Display(Name = "Adress")]
        public string adress { get; set; }

        [Display(Name = "Postnummer  ")]
        public string zipcode { get; set; }

        [Display(Name = "Lösenord")]
        public string password { get; set; }

        [Display(Name = "Stad")]
        public string city { get; set; }

        [Display(Name = "Epostadress")]
        public string email { get; set; }

        [Display(Name = "Kön")]
        public string gender { get; set; }

        [Display(Name = "Publikprofil")]
        public string @public { get; set; }

    }
}