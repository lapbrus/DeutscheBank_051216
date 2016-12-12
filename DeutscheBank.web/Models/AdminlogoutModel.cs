using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeutscheBank.web.Models
{
    public class AdminLogoutModel
    {

        [Required]
        [StringLength(3, ErrorMessage = "yes or no")]
        public string LogoutAdmin { get; set; }

    }
}




