using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeutscheBank.web.Models
{
    public class FinanzielleSituationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pflichtfeld")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl eingeben")]
        [Range(500, 10000, ErrorMessage = "Der Wert muss zwischen 500 und 10000 sein")]
        [Display(Name = "Netto Einkommen 14x jährlich")]
        public double NettoEinkommen { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Pflichtfeld")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte Zahl eingeben")]
        [Range(0,1000, ErrorMessage = "Der Wert muss zwischen 0 und 1000 sein")]
        [Display(Name = "Wohnkosten(Miete. Heizun,Strom)")]
        public double WohnKosten { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Pflichtfeld")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte Zahl eingeben")]
        [Range(0, 1000, ErrorMessage = "Der Wert muss zwischen 0 und 1000 sein")]
        [Display(Name = "Einkünfte Alimente")]
        public double EinkuenfteAlimente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Pflichtfeld")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl eingeben")]
        [Range(0, 10000, ErrorMessage = "Wert muss zwischen 0 und 10000 liegen")]
        [Display(Name = "Zahlungen für Unterhalt, Alimente usw.")]
        public double UnterhaltsZahlungen { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Pflichtfeld")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl eingeben")]
        [Range(0, 10000, ErrorMessage = "Wert muss zwischen 0 und 10000 liegen")]
        [Display(Name = "Bestehende RatenVerpflichtungen usw.")]
        public double RatenVerpflichtungen { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        [Required]
        public int ID_Kunde { get; set; }

    }
}