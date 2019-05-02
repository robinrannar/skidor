using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skidor4.Models
{
    // Klassen gör så att man visar namnet på svenska istället för det som står i databasen
    [MetadataType(typeof(PersonMetaData))]
    public partial class persons
    {
    }
    public class PersonMetaData
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ett förnamn")]
        [Display(Name = "Förnamn")]
        public string firstname { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ett efternamn")]
        [Display(Name = "Efternamn")]
        public string lastname { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange en gatuadress")]
        [Display(Name = "Adress")]
        public string adress { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ett postnummer")]
        [Display(Name = "Postnummer  ")]
        public string zipcode { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ett användar-ID")]
        [Display(Name = "Användar-ID")]
        public string alias { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ett lösenord")]
        [Display(Name = "Lösenord")]
        public string password { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange en stad")]
        [Display(Name = "Stad")]
        public string city { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange en epostadress")]
        [Display(Name = "Epostadress")]
        public string email { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste välja kön")]
        [Display(Name = "Kön")]
        public string gender { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste välja profil")]
        [Display(Name = "Publikprofil")]
        public string @public { get; set; }

    }
}