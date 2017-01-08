using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeutscheBank.web.Models
{
    public class KontaktDatenModel 
    {
        public int ID { get; set; }
        public int FKOrt { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Strasse")]
        public string Strasse { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Hausnummer")]
        public string HausNummer { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Email")]
        public string Mail { get; set; }

       [Required(ErrorMessage = "Pflichtfeld")]
       [Display(Name = "Telefonnummer")]
       public string TelefonNummer { get; set; }

        public List<LandsModel> AlleLaender { get; set; }
        public List<OrtsModel> AlleOrte { get; set; }

    }
}