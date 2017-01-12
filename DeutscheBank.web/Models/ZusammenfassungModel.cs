using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeutscheBank.web.Models
{
    public class ZusammenfassungModel
    {
        
            #region Allgemein
            public int ID_Kunde { get; set; }
            #endregion

            #region KreditRahmen
            public double GewünschterBetrag { get; set; }
            public int Laufzeit { get; set; }
            #endregion

            #region Finanzielle Situation
            public double NettoEinkommen { get; set; }

            public double Wohnkosten { get; set; }

            public double EinkuenfteAlimente { get; set; }

            public double UnterhaltsZahlungen { get; set; }

            public double RatenVerpflichtungen { get; set; }
            #endregion

            #region Persönliche Daten
            public string Geschlecht { get; set; }

            public string Vorname { get; set; }

            public string Nachname { get; set; }

            public string Titel { get; set; }

            public DateTime GeburtsDatum { get; set; }

            public string Staatsbuergerschaft { get; set; }

            public int AnzahlUnterhaltspflichtigeKinder { get; set; }

            public string Familienstand { get; set; }

            public string Wohnart { get; set; }

            public string Identifikationsart { get; set; }

            public string IdentifikationsNummer { get; set; }
            #endregion

            #region Arbeitgeber
            public string FirmenName { get; set; }

            public string BeschaeftigungsArt { get; set; }

            public string Branche { get; set; }

            public string BeschaeftigtSeit { get; set; }
            #endregion

            #region KontaktDaten
            public string Strasse { get; set; }
            public string HausNummer { get; set; }
            public string Ort { get; set; }
            public string PLZ { get; set; }
            public string Mail { get; set; }
            public string TelefonNummer { get; set; }
            #endregion

            #region KontoInformationen
            public bool HatKonto { get; set; }

            public string Bank { get; set; }

            public string IBAN { get; set; }

            public string BIC { get; set; }
            #endregion
        
















    }
}