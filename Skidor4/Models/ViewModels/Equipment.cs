using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skidor4.Models
{
    [MetadataType(typeof(EquipmentMetaData))]
    public partial class equipment
    {
    }
    public class EquipmentMetaData
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ett namn")]
        [Display(Name = "Namn")]
        public string name { get; set; }
    }
}