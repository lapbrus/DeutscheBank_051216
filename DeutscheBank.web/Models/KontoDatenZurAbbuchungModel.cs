using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeutscheBank.web.Models
{
    public class KontoDatenZurAbbuchungModel
    {
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public string Bank { get; set; }
        public int ID { get; set; }
    }
}