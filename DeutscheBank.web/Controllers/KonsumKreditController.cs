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
        #region KreditRahmen

  

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

        #endregion

        #region FinanzielleSituation
       
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

        #endregion

        #region PersönlicheDaten
        
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

        #endregion

        #region Arbeitgeber
        
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
                    return RedirectToAction("KontoVerfuegbar");
                }
            }
            return View();
        }

        #endregion

        #region KontoInformationen




        /// <summary>
        /// Hier wird mittels Dropdown abgefragt, ob der Kunde:
        ///     - ein vorhandenes Deutsche Ban AG - Konto verwenden möchte
        ///     - ein neues Konto bei der Deutschen Bank AG anlegen möchte
        ///       und dieses dann für den Konsum-Kredit vewenden möchte
        ///     - oder ein anderes Konto einer anderen Bank verwenden möchte
        /// </summary>
        /// <returns>Je nach Entscheidung zum richtigen Controller / anderfalls das "model" in der akt. View</returns>
        [HttpGet]
        public ActionResult KontoVerfuegbar()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            List<KontoAbfrageMoeglichkeitModel> alleKontoAbfrageMoeglichkeitenWeb = new List<KontoAbfrageMoeglichkeitModel>();

            /// Lade alle Kontoabfrage-Möglichkeiten aus der Businesslogic in 
            /// die Liste "alleKontoAbfrageMoeglichkeitenWeb".
            foreach (var kontoAbfrageMoeglichkeitWeb in KonsumKreditVerwaltung.KontoAbfrageMoeglichkeitenLaden())
            {
                alleKontoAbfrageMoeglichkeitenWeb.Add(new KontoAbfrageMoeglichkeitModel()
                {
                    ID = kontoAbfrageMoeglichkeitWeb.ID.ToString(),
                    Bezeichnung = kontoAbfrageMoeglichkeitWeb.Bezeichnung
                    });
            }

            /// Erzeuge das Model und weise der Liste alle wichtigen Details zu (ID´s, Bezeichnungen)
            KontoAbfrageModel model = new KontoAbfrageModel()
            {
                AlleKontoAbfrageMoeglichkeitenAngaben = alleKontoAbfrageMoeglichkeitenWeb,
                KundenID = int.Parse(Request.Cookies["idKunde"].Value)
            };
           
            Debug.Unindent();

            return View(model);
        }

        [HttpPost]
        public ActionResult KontoVerfuegbar(KontoAbfrageModel model)
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontoInformation");
            Debug.Indent();

            int auswahl = model.ID_KontoAbfrage;

            if (ModelState.IsValid)
            {
                if (auswahl == 1)
                {
                    Debug.WriteLine("Auswahl des Kontos:");
                    Debug.Indent();
                    Debug.WriteLine("ID: " + model.ID_KontoAbfrage);
                    Debug.WriteLine("Auswahl: \"Vorhandenes Konto der Deutschen Bank AG\"");
                    Debug.Unindent();

                    return RedirectToAction("HatDeutscheBankKontoInformation");
                }
                else if (auswahl == 2)
                {
                    Debug.WriteLine("Auswahl des Kontos:");
                    Debug.Indent();
                    Debug.WriteLine("ID: " + model.ID_KontoAbfrage);
                    Debug.WriteLine("Auswahl: \"Neues Konto bei Deutsche Bank AG anlegen.\"");
                    Debug.Unindent();

                    return RedirectToAction("NeuesDeutscheBankKontoInformation");
                }
                else if (auswahl == 3)
                {
                    Debug.WriteLine("Auswahl des Kontos:");
                    Debug.Indent();
                    Debug.WriteLine("ID: " + model.ID_KontoAbfrage);
                    Debug.WriteLine("Auswahl: \"Anderes Konto verwenden.\"");
                    Debug.Unindent();

                    return RedirectToAction("AndereKontoInformation");
                }
                else
                {
                    Debug.WriteLine("Irgendetwas ist Schief gegangen....");
                    Debug.Indent();
                    Debug.WriteLine("ID: " + model.ID_KontoAbfrage);
                    Debug.Unindent();
                    Debugger.Break();
                }
            }


            Debug.Unindent();

            return View(model);
        }

        /// <summary>
        /// Hier darf der User die Daten seines Vorhandenen Deutsche Bank AG - Kontos
        /// angeben, Das Eingabefeld Bank wird sichtbar für den User da gestellt, aber
        /// nicht veränderbar sein (Label)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HatDeutscheBankKontoInformation()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - HatDeutscheBankKontoInformation");
            Debug.Unindent();

            DeutscheBankKontoInformationModel model = new DeutscheBankKontoInformationModel()
            {
                KundenID = int.Parse(Request.Cookies["idKunde"].Value),
                BankInstitut = "Deutsche Bank AG",
                IstDeutscheBankKunde = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HatDeutscheBankKontoInformation(DeutscheBankKontoInformationModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - HatDeutscheBankKontoInformation");
            Debug.Unindent();

            model.IBAN = KonsumKreditVerwaltung.FilterAufVorhandeneLeerzeichen(model.IBAN);
            model.IBAN = KonsumKreditVerwaltung.LeerzeichenEinfuegen(model.IBAN);

            if (ModelState.IsValid)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(model.BIC,
                                                                        model.IBAN,
                                                                        "Deutsche Bank AG",
                                                                        true,
                                                                        model.KundenID
                                                                        ))
                {
                    return RedirectToAction("KontaktDaten");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Hier bekommt der User einen Vorschlag für sein neues 
        /// Deutsche Bank AG - Kontos und kann dieses mit "erstellen" 
        /// bestätigen.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NeuesDeutscheBankKontoInformation()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            List<string> bicUndIban = new List<string>();

            bicUndIban = KonsumKreditVerwaltung.BankKontoErzeugen();

            DeutscheBankKontoInformationModel model = new DeutscheBankKontoInformationModel()
            {
                BIC = bicUndIban[0],
                IBAN = bicUndIban[1],
                IstDeutscheBankKunde = true,
                BankInstitut = "Deutsche Bank AG",
                KundenID = int.Parse(Request.Cookies["kundenID"].Value)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NeuesDeutscheBankKontoInformation(DeutscheBankKontoInformationModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            if (ModelState.IsValid)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(model.BIC,
                                                                        model.IBAN,
                                                                        "Deutsche Bank AG",
                                                                        true,
                                                                        model.KundenID
                                                                        ))
                {
                    return RedirectToAction("KontaktDaten");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Hier kann der User die ganzen benötigten Konto-Informationene selbst eingeben,
        /// muss allerdings seine Bank angeben.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AndereKontoInformation()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            KontoInformationenModel model = new KontoInformationenModel()
            {
                ID = int.Parse(Request.Cookies["kundenID"].Value)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AndereKontoInformation(KontoInformationenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            if (ModelState.IsValid)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(model.BIC,
                                                                        model.IBAN,
                                                                        "Deutsche Bank AG",
                                                                        true,
                                                                        model.ID
                                                                        ))
                {
                    return RedirectToAction("KontaktDaten");
                }
            }

            return View(model);
        }
        #endregion

        #region KontaktDaten

        /// <summary>
        ///  Ladet alle benötigten Daten für die Lookuptabellen aus der Datenbank! 
        ///  (Ortschaften)
        /// </summary>
        /// <param name="model">ein Modell von Kontaktdaten</param>
        /// <returns>Das Kontaktdatenmodell mit allen Lookupdaten</returns>
        private KontaktDatenModel KontaktDatenLookup(KontaktDatenModel model)
        {
            List<AlleOrteModel> alleOrte = new List<AlleOrteModel>();

            /// Lade Daten aus Logic

            foreach (var ort in KonsumKreditVerwaltung.OrteLaden())
            {
                alleOrte.Add(new AlleOrteModel()
                {
                    ID = ort.ID.ToString(),
                    Bezeichnung = ort.Bezeichnung
                });
            }

            model.AlleOrte = alleOrte;
            model.ID_Kunde = int.Parse(Request.Cookies["idKunde"].Value);

            return model;
        }

        [HttpGet]
        public ActionResult KontaktDaten()
        {
            Debug.WriteLine("Get - KonsumKredit - KontaktDaten");

            KontaktDatenModel model = new KontaktDatenModel();
            model = KontaktDatenLookup(model);
            
            

            KontaktDaten kontaktDaten = KonsumKreditVerwaltung.KontaktDatenLaden(model.ID_Kunde);
            if (kontaktDaten != null)
            {
                model.ID_Ort = kontaktDaten.FKOrt;
                model.Strasse = kontaktDaten.Strasse;
                model.HausNummer = kontaktDaten.Hausnummer;
                model.EMail = kontaktDaten.EMail;
                model.TelefonNummer = kontaktDaten.Telefonnummer;

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
                if (KonsumKreditVerwaltung.KontaktDatenSpeichern
                (
                                                model.ID_Kunde,
                                                model.ID_Ort,
                                                model.Strasse,
                                                model.HausNummer,
                                                model.EMail,
                                                model.TelefonNummer))
                {
                    return RedirectToAction("Zusammenfassung");
                }
            }
            model = KontaktDatenLookup(model);


            return View(model);
        }


        #endregion
        
        #region Zusammenfassung

   
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
            Ort aktOrt = KonsumKreditVerwaltung.KundenOrtLaden(model.ID_Kunde);

            model.GewünschterBetrag = (double) aktKunde.AlleKredite.ToList()[aktKunde.AlleKredite.ToList().Count-1].GewuenschterKredit;
            model.Laufzeit = (int)aktKunde.AlleKredite.ToList()[aktKunde.AlleKredite.ToList().Count-1].GewuenschteLaufzeit;

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
            model.Ort = aktOrt.Bezeichnung;
            model.PLZ = aktOrt.PLZ;
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

        #endregion
    }

}


