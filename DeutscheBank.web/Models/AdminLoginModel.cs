using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeutscheBank.web.Models
{
    public class AdminLoginModel
    {

        [Required]
        [StringLength(50, ErrorMessage = "max. 50 Zeichen")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "max. 50 Zeichen")]
        public string AdminKennwort { get; set; }


    }
}