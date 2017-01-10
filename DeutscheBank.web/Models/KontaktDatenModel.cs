using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace DeutscheBank.web.Models
{
    public class KontaktDatenModel 
    {
        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Ort")]
        public int ID_Ort { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Kunde")]
        public int ID_Kunde { get; set; }


        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Strasse")]
        public string Strasse { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Hausnummer")]
        public string HausNummer { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Email")]
        public string EMail { get; set; }

       [Required(ErrorMessage = "Pflichtfeld")]
       [Display(Name = "Telefonnummer")]
       public string TelefonNummer { get; set; }

        
        public List<AlleOrteModel> AlleOrte { get; set; }
    }
}