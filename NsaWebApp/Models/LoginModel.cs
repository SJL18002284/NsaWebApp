using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NsaWebApp.Models
{
    public class LoginModel
    {
        [Key]
        public string userID { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
                            ErrorMessage = "Please enter a valid email address")]
        public string userEmail { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string userPassword { get; set; }

        public string loginDateTime { get; set; }
    }
}