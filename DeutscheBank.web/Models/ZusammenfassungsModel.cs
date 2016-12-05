using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeutscheBank.web.Models
{
    public class ZusammenfassungsModel
    {
        public KreditRahmenModel KreditRahmen { get; set; }
        public FinanzielleSituationModel FinanzielleSituation { get; set; }
        public PersoenlicheDatenModel PersönlicheDaten { get; set; }
        public ArbeitgeberModel Arbeitgeber { get; set; }
        public KontaktDatenModel KontaktDaten { get; set; }
        public KontoInformationenModel KontoInformationen { get; set; }

        public int ID_Kunde { get; set; }
    }
}