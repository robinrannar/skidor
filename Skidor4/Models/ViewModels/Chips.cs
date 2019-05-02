using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skidor4.Models
{
    // Klassen gör så att man visar namnet på svenska istället för det som står i databasen
    [MetadataType(typeof(ChipMetaData))]
    public partial class chips
    {
    }
    public class ChipMetaData
    {
        [Display(Name = "Chipnummer")]
        public int pk_id { get; set; }

        [Display(Name = "Betalt")]
        public string paid { get; set; }
    }
}