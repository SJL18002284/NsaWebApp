using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NsaWebApp.Models
{
    public class VolunteerModel
    {
        [Key]
        public string VolunteerID { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
                            ErrorMessage = "Please enter a valid email address")]
        public string userEmail { get; set; }

        [Required(ErrorMessage = "Full Name cannot be empty")]
        public string fullName { get; set; }

        [Required(ErrorMessage = "Contact Number cannot be empty")]
        public string contactNo { get; set; }
    }
}