using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skidor4.Models
{
    // Klassen gör så att man visar namnet på svenska istället för det som står i databasen
    [MetadataType(typeof(StationsMetaData))]
    public partial class stations
    {
    }
    public class StationsMetaData
    {
        [Display(Name = "Stationsnamn")]
        public string name { get; set; }

    }
}