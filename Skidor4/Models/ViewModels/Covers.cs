using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skidor4.Models
{
    // Klassen gör så att man visar namnet på svenska istället för det som står i databasen
    [MetadataType(typeof(CoversMetaData))]
    public partial class covers
    {

    }
    public class CoversMetaData
    {
        [Display(Name = "Chipnummer")]
        public string noChip { get; set; }

        [Display(Name = "Tid")]
        public string time { get; set; }

        [Display(Name = "Aktivt chip")]
        public string isActive { get; set; }

    }
}