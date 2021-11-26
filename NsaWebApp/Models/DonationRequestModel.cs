﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NsaWebApp.Models
{
    public class DonationRequestModel
    {
        //variables of the request
        public string requestID { get; set; }

        [Required(ErrorMessage = "Request Date cannot be empty")]
        public string requestDate { get; set; }

        [Required(ErrorMessage = "Cause Name cannot be empty")]
        public string causeName { get; set; }

        [Required(ErrorMessage = "Organizer Name cannot be empty")]
        public string organizerName { get; set; }

        [Required(ErrorMessage = "Organizer Email cannot be empty")]
        //[RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
        //                    ErrorMessage = "Please enter a valid email address")]
        public string organizerEmail { get; set; }

        [Required(ErrorMessage = "Organizer ContactNo cannot be empty")]
        public string organizerContactNo { get; set; }

        [Required(ErrorMessage = "Request Details cannot be empty")]
        public string requestDetails { get; set; }

        public string userID { get; set; }
    }
}