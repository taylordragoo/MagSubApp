﻿using System.ComponentModel.DataAnnotations;

namespace MagSubApp.Models
{
    public class Webappmaster
    {
        public int Id { get; set; }
        [StringLength(20, MinimumLength = 2)]
        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }
        [StringLength(20, MinimumLength = 2)]
        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Please enter email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please select a subscription")]
        public int SubID { get; set; }
    }
}
