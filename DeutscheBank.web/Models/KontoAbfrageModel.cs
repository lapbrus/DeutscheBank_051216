using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeutscheBank.web.Models
{
    public class KontoAbfrageModel
    {
        [Required(ErrorMessage = "Bitte Ihre Angaben zum Konto machen, mittels dem Sie den Kredit abwickeln möchten.")]
        [Display(Name = "Kontaktabfrage")]
        public int ID_KontoAbfrage { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }

        public List<KontoAbfrageArtModel> AlleKontoAbfrageMoeglichkeitenAngaben { get; set; }



    }
}