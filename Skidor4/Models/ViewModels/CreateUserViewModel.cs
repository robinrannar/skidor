using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skidor4.Data;
using System.ComponentModel.DataAnnotations;

namespace Skidor4.Models.ViewModels
{
    public class CreateUserViewModel
    {
        public persons Person  { get; set; }
        public chips Chip { get; set; }
        public permissions Permission { get; set; }
        public List<permissions> Permissions { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Du måste välja roll")]
        [Display(Name = "Roll")]
        public int chosenPermission { get; set; }
    }
}