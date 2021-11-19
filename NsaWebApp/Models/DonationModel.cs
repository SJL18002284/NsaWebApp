using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NsaWebApp.Models
{
    public class DonationModel
    {
        public string donationID { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        public string doneeName { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                            ErrorMessage = "Please enter a valid email address")]
        public string doneeEmail { get; set; }

        [Required(ErrorMessage = "Contact Number cannot be empty")]
        public string doneeNo { get; set; }

        [Required(ErrorMessage = "Donation Details cannot be empty")]
        public string donationDetails { get; set; }

        [Required(ErrorMessage = "Request Date cannot be empty")]
        public string donationDate { get; set; }
    }
}