using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeutscheBank.web.Models
{
    public class KreditKartenModel 
    {
        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Inhaber: Bitte tragen Sie Ihren Namen ein so wie es auf die Karte aufgeduckt ist")]
        public string Inhaber { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Kartennummer: ")]
        [CreditCard(ErrorMessage = "Falsche KartenNummer")]
        public string KartenNummer { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Gültig bis: mm/yy")]
        public string GueltigBis { get; set; }

        [Required(ErrorMessage = "Pflichtfeld")]
        [Display(Name = "CVC Nummer: Siehe 3 - stellige Nummer auf der Rűckseite Ihrer Kreditkarte")]
        public string CVC_Nummer { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }

    }
}