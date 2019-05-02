using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skidor4.Models.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Användarnamn")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ett användarnamn")]
        public string alias { get; set; }

        [Display(Name = "Lösenord")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ditt lösenord")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public permissions Role { get; set; } = new permissions();
    }
}