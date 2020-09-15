using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MagSubApp.Models
{
    public partial class Webappmaster
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Please enter email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please select a subscription")]
        public int MagSub { get; set; }
    }
}
