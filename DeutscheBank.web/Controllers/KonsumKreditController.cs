using deutscheBank.freigabe;
using deutscheBank.logic;
using DeutscheBank.web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeutscheBank.web.Controllers
{
    public class KonsumKreditController : Controller
    {
        [HttpGet]
        public ActionResult KreditRahmen()
        {
            Debug.WriteLine("GET - KonsumKredit - KreditRahmen");
            KreditRahmenModel model = new KreditRahmenModel()
            {
                GewünschterBetrag = 50000, // default Werte
                Laufzeit = 12 // default Werte

            };
            int id = -1;
            if (Request.Cookies["idkunde"] != null && int.TryParse(Request.Cookies["idKunde"].Value, out id))
            {
                // lade aus Datenbank
                Kredit wunsch = KonsumKreditVerwaltung.KreditRahmenLaden(id);
                model.GewünschterBetrag = (int)wunsch.GewuenschterKredit;
                model.Laufzeit = wunsch.GewuenschteLaufzeit;
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KreditRahmen(KreditRahmenModel model)
        {
            Debug.WriteLine("POST - KonsumKredit - KreditRahmen");

            if (ModelState.IsValid)
            {
                /// speichere Daten über BusinessLogic
                Kunde neuerKunde = KonsumKreditVerwaltung.ErzeugeKunde();

                if (neuerKunde != null && KonsumKreditVerwaltung.KreditRahmenSpeichern(model.GewünschterBetrag, model.Laufzeit, neuerKunde.ID))
                {
                    /// ich benötige für alle weiteren Schritte die ID
                    /// des angelegten Kunden. Damit ich diese bei der nächsten Action
                    /// habe, speichere ich sie für diesen Zweck in ein Cookie
                    Response.Cookies.Add(new HttpCookie("idKunde", neuerKunde.ID.ToString()));

                    /// gehe zum nächsten Schritt
                    return RedirectToAction("FinanzielleSituation");
                }
            }

            /// falls der ModelState NICHT valid ist, bleibe hier und
            /// gib die Daten (falls vorhanden) wieder auf das UI
            return View(model);
        }



        [HttpGet]
        public ActionResult FinanzielleSituation()
        {
            Debug.WriteLine("GET - KonsumKredit - FinanzielleSituation");

            FinanzielleSituationModel model = new FinanzielleSituationModel()
            {
                ID_Kunde = int.Parse(Request.Cookies["idKunde"].Value)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinanzielleSituation(FinanzielleSituationModel model)
        {
            Debug.WriteLine("POST - KonsumKredit - FinanzielleSituation");

            if (ModelState.IsValid)
            {
                /// speichere Daten über BusinessLogic
                if (KonsumKreditVerwaltung.FinanzielleSituationSpeichern(
                                                model.NettoEinkommen,
                                                model.RatenVerpflichtungen,
                                                model.WohnKosten,
                                                model.EinkuenfteAlimente,
                                                model.UnterhaltsZahlungen,
                                                model.ID_Kunde))
                {
                    return RedirectToAction("PersönlicheDaten");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult PersönlicheDaten()
        {
            Debug.WriteLine("GET - KonsumKredit - PersönlicheDaten");

            List<FamilienStandModel> alleFamilienStandArten = new List<FamilienStandModel>();
            List<IdentifikationsModel> alleIdentifikationsAngaben = new List<IdentifikationsModel>();
            List<StaatsbuergerschaftsModel> alleStaatsbuergerschaftsAngaben = new List<StaatsbuergerschaftsModel>();
            List<TitelModel> alleTitelAngaben = new List<TitelModel>();
            List<WohnartModel> alleWohnartAngaben = new List<WohnartModel>();

            /// Lade Daten aus Logic

            foreach (var familienStand in KonsumKreditVerwaltung.FamilienStandArtenLaden())
            {
                alleFamilienStandArten.Add(new FamilienStandModel()
                {
                    ID = familienStand.ID.ToString(),
                    Bezeichnung = familienStand.Bezeichnung
                });
            }
            foreach (var identifikationsAngabe in KonsumKreditVerwaltung.IdentifikiationsAngabenLaden())
            {
                alleIdentifikationsAngaben.Add(new IdentifikationsModel()
                {
                    ID = identifikationsAngabe.ID.ToString(),
                    Bezeichnung = identifikationsAngabe.Bezeichnung
                });
            }
            foreach (var land in KonsumKreditVerwaltung.LaenderLaden())
            {
                alleStaatsbuergerschaftsAngaben.Add(new StaatsbuergerschaftsModel()
                {
                    ID = land.ID,
                    Bezeichnung = land.Bezeichnung
                });
            }
            foreach (var titel in KonsumKreditVerwaltung.TitelLaden())
            {
                alleTitelAngaben.Add(new TitelModel()
                {
                    ID = titel.ID.ToString(),
                    Bezeichnung = titel.Bezeichnung
                });
            }
            foreach (var wohnart in KonsumKreditVerwaltung.WohnartenLaden())
            {
                alleWohnartAngaben.Add(new WohnartModel()
                {
                    ID = wohnart.ID.ToString(),
                    Bezeichnung = wohnart.Bezeichnung
                });
            }

            PersoenlicheDatenModel model = new PersoenlicheDatenModel()
            {
                AlleFamilienStandArten = alleFamilienStandArten,
                AlleIdentifikationsAngaben = alleIdentifikationsAngaben,
                AlleStaatsbuergerschaftsAngaben = alleStaatsbuergerschaftsAngaben,
                AlleTitelAngaben = alleTitelAngaben,
                AlleWohnartAngaben = alleWohnartAngaben,
                ID_Kunde = int.Parse(Request.Cookies["idKunde"].Value)
            };

            Kunde kunde = KonsumKreditVerwaltung.PersoenlicheDatenLaden(model.ID_Kunde);
            if (kunde != null)
            {
                model.Geschlecht = !string.IsNullOrEmpty(kunde.Geschlecht) && kunde.Geschlecht == "m" ? DeutscheBank.web.Models.Geschlecht.Männlich : DeutscheBank.web.Models.Geschlecht.Weiblich;
                model.Vorname = kunde.Vorname;
                model.Nachname = kunde.Nachname;
                model.ID_Titel = kunde.FKTitel.HasValue ? kunde.FKTitel.Value : 0;
                //model.ID_TitelNachstehend = kunde.FKTitelNachstehend.HasValue ? kunde.FKTitelNachstehend.Value : 0;
                //model.GeburtsDatum = DateTime.Now;
                model.ID_Staatsbuergerschaft = kunde.FKStaatsangehoerigkeit;
                model.ID_FamilienStand = kunde.FKFamilienstand.HasValue ? kunde.FKFamilienstand.Value : 0;
                model.ID_Wohnart = kunde.FKWohnart.HasValue ? kunde.FKWohnart.Value : 0;
                //model.ID_Bildung = kunde.FKSchulabschluss.HasValue ? kunde.FKSchulabschluss.Value : 0;
                model.ID_Identifikationsart = kunde.FKIdentifikationsArt.HasValue ? kunde.FKIdentifikationsArt.Value : 0;
                model.IdentifikationsNummer = kunde.IdentifikationsNummer;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersönlicheDaten(PersoenlicheDatenModel model)
        {
            Debug.WriteLine("POST - KonsumKredit - PersönlicheDaten");

            if (ModelState.IsValid)
            {

                /// speichere Daten über BusinessLogic
                if (KonsumKreditVerwaltung.PersönlicheDatenSpeichern(
                                             model.ID_Titel,
                                             model.Geschlecht == Geschlecht.Männlich ? "m" : "w",
                                             model.GeburtsDatum,
                                             model.Vorname,
                                             model.Nachname,
                                             model.ID_FamilienStand,
                                             model.ID_Identifikationsart,
                                             model.IdentifikationsNummer,
                                             model.ID_Staatsbuergerschaft,
                                             model.ID_Wohnart,
                                             model.ID_Kunde))
                {
                    return RedirectToAction("Arbeitgeber");
                }


            }

            return View();
        }

        [HttpGet]
        public ActionResult Arbeitgeber()
        {
            Debug.WriteLine("GET - KonsumKredit - Arbeitgeber");

            List<BeschaeftigungsArtModel> alleBeschaeftigungsArten = new List<BeschaeftigungsArtModel>();
            List<BrancheModel> alleBranchen = new List<BrancheModel>();

            foreach (var branche in KonsumKreditVerwaltung.BranchenLaden())
            {
                alleBranchen.Add(new BrancheModel()
                {
                    ID = branche.ID.ToString(),
                    Bezeichnung = branche.Bezeichnung
                });
            }

            foreach (var beschaeftigungsArt in KonsumKreditVerwaltung.BeschaeftigungsArtenLaden())
            {
                alleBeschaeftigungsArten.Add(new BeschaeftigungsArtModel()
                {
                    ID = beschaeftigungsArt.ID.ToString(),
                    Bezeichnung = beschaeftigungsArt.Bezeichnung
                });
            }

            ArbeitgeberModel model = new ArbeitgeberModel()
            {
                AlleBeschaeftigungsArten = alleBeschaeftigungsArten,
                AlleBranchen = alleBranchen,
                ID_Kunde = int.Parse(Request.Cookies["idKunde"].Value)
            };

            Arbeitgeber arbeitgeberDaten = KonsumKreditVerwaltung.ArbeitgeberAngabenLaden(model.ID_Kunde);
            if (arbeitgeberDaten != null)
            {
                model.BeschaeftigtSeit = arbeitgeberDaten.BeschaeftigtSeit.Value.ToString("MM.yyyy");
                model.FirmenName = arbeitgeberDaten.Firma;
                model.ID_BeschaeftigungsArt = arbeitgeberDaten.FKBeschaeftigungsArt; ;
                model.ID_Branche = arbeitgeberDaten.FKBranche.Value;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Arbeitgeber(ArbeitgeberModel model)
        {
            Debug.WriteLine("POST - KonsumKredit - Arbeitgeber");

            if (ModelState.IsValid)
            {
                /// speichere Daten über BusinessLogic
                if (KonsumKreditVerwaltung.ArbeitgeberAngabenSpeichern(
                                                model.FirmenName,
                                                model.ID_BeschaeftigungsArt,
                                                model.ID_Branche,
                                                model.BeschaeftigtSeit,
                                                model.ID_Kunde))
                {
                    return RedirectToAction("KontoInformationen");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult KontoInformationen()
        {
            Debug.WriteLine("GET - KonsumKredit - KontoInformationen");


            DeutscheBank.web.Models.KontoInformationenModel model = new KontoInformationenModel()
            {
                ID = int.Parse(Request.Cookies["idkunde"].Value)

                
            };

            KontoDaten daten = KonsumKreditVerwaltung.KontoInformationenLaden(model.ID);
            if (daten != null)
            {
                model.Bank = daten.Bank;
                model.BIC = daten.BIC;
                model.IBAN = daten.IBAN;
                model.HatKonto = !daten.HatKonto; 
            }
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontoInformationen(KontoInformationenModel model)
        {
            Debug.WriteLine("POST - KonsumKredit - KontoInformationen");

            if (ModelState.IsValid)
            {
                /// speichere Daten über BusinessLogic
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(
                                                model.Bank,
                                                model.IBAN,
                                                model.BIC,
                                                model.HatKonto,
                                                model.ID))
                {
                    return RedirectToAction("Kontaktdaten");
                }
            }

            return View();
        }

        
        // NEW:

        [HttpGet]
        public ActionResult KontaktDaten()
        {
            Debug.WriteLine("Get - KonsumKredit - KontaktDaten");

            DeutscheBank.web.Models.KontaktDatenModel model = new KontaktDatenModel()
            {
                ID_PLZ = int.Parse(Request.Cookies["idKunde"].Value)

            };
            KontaktDaten daten = deutscheBank.logic.KonsumKreditVerwaltung.KontaktDatenLaden(model.ID_PLZ);
            if (daten != null)
            {
                model.Strasse = daten.Strasse;
                model.HausNummer = daten.Hausnummer;
                model.TelefonNummer = daten.Telefonnummer;
                model.Mail = daten.EMail;
                
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontaktDaten(KontaktDatenModel model)
        {
            Debug.WriteLine("POST - Konsumkredit - Kontaktdaten");
          
                if (ModelState.IsValid)
                {
                /// speichere Daten über BusinessLogic
                if (KonsumKreditVerwaltung.KontaktDatenSpeichern(
                                                model.ID_PLZ,
                                                model.Strasse,
                                                model.HausNummer,
                                                model.Mail,
                                                model.TelefonNummer))                             
                    {
                        return RedirectToAction("Zusammenfassung");
                    }
                }

                return View();
            }





        // ---ENDNEW

        [HttpGet]
        public ActionResult Zusammenfassung()
        {
            Debug.WriteLine("GET - KonsumKredit - Zusammenfassung");

            /// ermittle für diese Kunden_ID
            /// alle gespeicherten Daten (ACHTUNG! das sind viele ....)
            /// gib Sie alle in das ZusammenfassungsModel (bzw. die UNTER-Modelle) 
            /// hinein.
            ZusammenfassungModel model = new ZusammenfassungModel();
            model.ID_Kunde = int.Parse(Request.Cookies["idKunde"].Value);

            /// lädt ALLE Daten zu diesem Kunden (also auch die angehängten/referenzierten
            /// Entities) aus der DB
            Kunde aktKunde = KonsumKreditVerwaltung.KundeLaden(model.ID_Kunde);

            model.GewünschterBetrag = (double) aktKunde.Kredit.ToList()[aktKunde.Kredit.ToList().Count-1].GewuenschterKredit;
          //  model.Laufzeit = (double)aktKunde.Kredit.ToString()Count-1].;

            model.NettoEinkommen = (double)aktKunde.FinanzielleSituation.MonatsEinkommenNetto.Value;
            model.Wohnkosten = (double)aktKunde.FinanzielleSituation.Wohnkosten.Value;
            model.EinkuenfteAlimente = (double)aktKunde.FinanzielleSituation.SonstigeEinkommen;
            model.UnterhaltsZahlungen = (double)aktKunde.FinanzielleSituation.Unterhalt.Value;
            model.RatenVerpflichtungen = (double)aktKunde.FinanzielleSituation.Raten.Value;

            model.Geschlecht = aktKunde.Geschlecht == "m" ? "Herr" : "Frau";
            model.Vorname = aktKunde.Vorname;
            model.Nachname = aktKunde.Nachname;
            model.Titel = aktKunde.Titel?.Bezeichnung;
         //   model.Titel = aktKunde.TitelNachstehend?.Bezeichnung;
            model.GeburtsDatum = DateTime.Now;
            model.Staatsbuergerschaft = aktKunde.Land.Bezeichnung;
            model.AnzahlUnterhaltspflichtigeKinder = -1;
            model.Familienstand= aktKunde.Familienstand?.Bezeichnung;
            model.Wohnart = aktKunde.Wohnart?.Bezeichnung;
            //  model.Schulabschluss = aktKunde.Schulabschluss?.Bezeichnung;
            model.Identifikationsart = aktKunde.IdentifikationsArt?.Bezeichnung;
            model.IdentifikationsNummer = aktKunde.IdentifikationsNummer;

            model.FirmenName = aktKunde.Arbeitgeber?.Firma;
            model.BeschaeftigungsArt = aktKunde.Arbeitgeber?.Beschaeftigungsart.Bezeichnung;
            model.Branche = aktKunde.Arbeitgeber?.Branche?.Bezeichnung;
            model.BeschaeftigtSeit = aktKunde.Arbeitgeber?.BeschaeftigtSeit.Value.ToShortDateString();

            model.Strasse = aktKunde.KontaktDaten?.Strasse;
            model.HausNummer = aktKunde.KontaktDaten?.Hausnummer;
            model.Ort = aktKunde.KontaktDaten?.tblOrt.PLZ;
            model.Mail = aktKunde.KontaktDaten?.EMail;
            model.TelefonNummer = aktKunde.KontaktDaten?.Telefonnummer;

      //     model.HatKonto = (bool)aktKunde.KontoDaten?.HatKonto;
            model.Bank = aktKunde.KontoDaten?.Bank;
            model.IBAN = aktKunde.KontoDaten?.IBAN;
            model.BIC = aktKunde.KontoDaten?.BIC;

            /// gib model an die View
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bestätigung(int id, bool? bestätigt)
        {
            if (bestätigt.HasValue && bestätigt.Value)
            {
                Debug.WriteLine("POST - KonsumKredit - Bestätigung");
                Debug.Indent();


                //int idKunde = int.Parse(Request.Cookies["idKunde"].Value);
                Kunde aktKunde = KonsumKreditVerwaltung.KundeLaden(id);
                Response.Cookies.Remove("idKunde");

                bool istFreigegeben = KreditFreigabe.FreigabeErteilt(
                                                          aktKunde.Geschlecht,
                                                            aktKunde.Vorname,
                                                            aktKunde.Nachname,
                                                            aktKunde.Familienstand.Bezeichnung,
                                                            (double)aktKunde.FinanzielleSituation.MonatsEinkommenNetto,
                                                            (double)aktKunde.FinanzielleSituation.Wohnkosten,
                                                            (double)aktKunde.FinanzielleSituation.SonstigeEinkommen,
                                                            (double)aktKunde.FinanzielleSituation.Unterhalt,
                                                            (double)aktKunde.FinanzielleSituation.Raten);

                /// Rüfe Service/DLL auf und prüfe auf Kreditfreigabe
                Debug.WriteLine($"Kreditfreigabe {(istFreigegeben ? "" : "nicht")}erteilt!");

                Debug.Unindent();
                return RedirectToAction("Index", "Freigabe", new { erfolgreich = istFreigegeben });

            }
            else
            {
                return RedirectToAction("Zusammenfassung");
            }
        }


    }

}


