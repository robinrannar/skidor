using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skidor4.Models
{
    // Klassen gör så att man visar namnet på svenska istället för det som står i databasen
    [MetadataType(typeof(TracksMetaData))]
    public partial class Tracks
    {

    }
    public class TracksMetaData
    {
        [Display(Name = "Spårnamn")]
        public string name { get; set; }

        [Display(Name = "Längd")]
        public string lenght { get; set; }

        [Display(Name = "Pistads")]
        public string pist { get; set; }
    }

}
