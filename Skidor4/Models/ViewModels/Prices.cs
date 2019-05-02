using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skidor4.Models
{
    // Klassen gör så att man visar namnet på svenska istället för det som står i databasen
    [MetadataType(typeof(PricesMetaData))]
    public partial class prices
    {
        public List<prices> getprices { get; set; }

        public class PricesMetaData
        {
            [Display(Name = "Kortnamn")]
            public string name { get; set; }
        }
    }
}