using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeutscheBank.web.Models
{
    public class DeutscheBankKontoInformationModel
    {
      
        [Required(ErrorMessage = "Bitte Ihren IBAN angeben.")]
        [StringLength(30, ErrorMessage = "Maximal 30 Zeichen")]
        [Display(Name = "BIC")]
        public string BIC { get; set; }

        
        [Required(ErrorMessage = "Bitte Ihre Bankleitzahl angeben.")]
        [StringLength(30, ErrorMessage = "Maximal 30 Zeichen")]
        [Display(Name = "IBAN")]
        public string IBAN { get; set; }

      
        [StringLength(100, ErrorMessage = "Maximal 100 Zeichen.")]
        [Display(Name = "Bankinstitut")]
        public string BankInstitut { get; set; }

        /// <summary>
        /// Diese Information wird im Controller gesetzt!!!
        /// </summary>
       
        public bool IstDeutscheBankKunde { get; set; }

       
        [Required]
        public int KundenID { get; set; }


    }
}