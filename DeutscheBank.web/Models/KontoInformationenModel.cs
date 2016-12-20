using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeutscheBank.web.Models
{
    public class KontoInformationenModel
    {
        public string Bank { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public bool HatKonto { get; set; }
        public int ID { get; set; }
    }
}