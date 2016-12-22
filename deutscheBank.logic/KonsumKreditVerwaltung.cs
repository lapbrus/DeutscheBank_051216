using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deutscheBank.logic
{
    public class KonsumKreditVerwaltung
    {

        #region Erledigt

        ///<summary>
        ///Erzeugt einen "lleren dummy Kunden
        ///zu dem in Folge alle Konsumkredit Daten
        ///verknüpft werden können.
        ///</summary>
        ///<returns>einen leeren Kunden wenn erfolgreich, ansonsten null</returns>
        public static Kunde ErzeugeKunde()
        {
            Debug.WriteLine("KonsumKreditVerwaltung - ErzeugeKunde");
            Debug.Indent();

            Kunde neuerKunde = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    neuerKunde = new logic.Kunde()
                    {
                        Vorname = "anonym",
                        Nachname = "anonym",
                        Geschlecht = "m"

                    };
                    context.AlleKunden.Add(neuerKunde);

                    int anzahlZeilenBetroffen = context.SaveChanges();
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Kunden angelegt!");

                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in ErzeugeKunde");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return neuerKunde;

        }

        /// <summary>
        /// Lädt den Kreditrahmen für die übergebene ID
        /// </summary>
        /// <param name="id">die id des zu ladenden Kreditrahmens</param>
        /// <returns>der Kreditwunsch für die übergebene ID</returns>
        public static Kredit KreditRahmenLaden(int id)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - KreditRahmenLaden");
            Debug.Indent();

            Kredit wunsch = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    wunsch = context.AlleKredite.Where(x => x.ID == id).FirstOrDefault();
                    Debug.WriteLine("KreditRahmen geladen!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KreditRahmenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return wunsch;
        }


        ///<summary>
        ///Speichert zu einer übergebene ID_Kunde den Wunschkredit und dessen Laufzeit ab
        ///</summary>
        ///<param name="GewuenschterKredit" >die Höhe des gewünschten Kredits</parameter>
        ///<param name="GewuenschteLaufzeit">die Laufzeit des gewünschten Kredits</param> 
        ///<param name="idKunde">ID des Kunden zu dem die Angaben gespeichert werden</param> 
        ///<returns>true wenn Eintragung gespeichert werden konnte, der Kunde existiert ansonsten false </returns>
        ///


        public static bool KreditRahmenSpeichern(double kreditBetrag, int laufzeit, int idKunde)
        {
            Debug.WriteLine("Konsumkreditverwaltung - Kreditrahmen speichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {

                        Kredit neuerKreditWunsch = new Kredit()
                        {
                            GewuenschterKredit = (decimal)kreditBetrag,
                            GewuenschteLaufzeit = (sbyte)laufzeit,
                            FKKunde = (int)idKunde
                        };
                        context.AlleKredite.Add(neuerKreditWunsch);
                    }

                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen } KreditRahmen gespeichert!");
                }


            }
            catch (Exception ex)
            {

                Debug.WriteLine("Fehler in KreditRahmenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();

            }
            Debug.Unindent();
            return erfolgreich;
        }

        /// <summary>
        /// Speichert die Daten aus der Finanziellen Situation zu einem Kunden
        /// </summary>
        /// <param name="nettoEinkommen">das Netto Einkommen des Kunden</param>
        /// <param name="ratenVerpflichtungen">Raten Verpflichtungen des Kunden</param>
        /// <param name="wohnKosten">Die Wohnkosten des Kunden</param>
        /// <param name="einkünfteAlimenteUnterhalt">Einkünfte aus Alimente und Unterhalt</param>
        /// <param name="unterhaltsZahlungen">Zahlungen für Alimente und Unterhalt</param>
        /// <param name="idKunde">dei ID des Kunden</param>
        /// <returns>true wenn die Finanzielle Situation erfolgreich gespeicehrt werden konnte, ansonsten false</returns>
        public static bool FinanzielleSituationSpeichern(double nettoEinkommen, double ratenVerpflichtungen, double wohnKosten, double einkünfteAlimenteUnterhalt, double unterhaltsZahlungen, int idKunde)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - Finanzielle Situation Speichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    ///speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();


                    if (aktKunde != null)
                    {
                        FinanzielleSituation neueFinanzielleSituation = new FinanzielleSituation()
                        {

                            MonatsEinkommenNetto = (decimal)nettoEinkommen,
                            Raten = (decimal)ratenVerpflichtungen,
                            Unterhalt = (decimal)unterhaltsZahlungen,
                            SonstigeEinkommen = (decimal)einkünfteAlimenteUnterhalt,
                            Wohnkosten = (decimal)wohnKosten,
                            ID = idKunde,

                        };
                        context.AlleFinanzielleSituationsArten.Add(neueFinanzielleSituation);

                    }
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} FinanzielleSituation gespeichert!");



                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Fehler in FinanzielleSituation");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();

            }

            Debug.Unindent();
            return erfolgreich;


        }

        /// <summary>
        /// Liefert alle Branchen zurück
        /// </summary>
        /// <returns>alle Branchen oder null bei einem Fehler</returns>
        public static List<Branche> BranchenLaden()
        {
            Debug.WriteLine("KonsumKreditVerwaltung - BranchenLaden");
            Debug.Indent();

            List<Branche> alleBranchen = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleBranchen = context.AlleBranchen.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in BranchenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleBranchen;
        }

        //public static bool PersönlicheDatenSpeichern(int? iD_Titel,  DateTime geburtsDatum, string nachname,  int iD_Familienstand, int iD_Identifikationsart, string identifikationsNummer, string iD_Staatsbuergerschaft, int iD_Wohnart1, int iD_Wohnart2, int iD_Kunde)
        //{
        //    throw new NotImplementedException();
        //}

        ///<summary>
        ///Liefert alle Beschaeftigungsarten zurück
        ///</summary>
        ///<returns>alle Beschaeftigungsarten oder null bei einem Fehler</returns>
        public static List<Beschaeftigungsart> BeschaeftigungsArtenLaden()
        {

            Debug.WriteLine("Konsumkreditverwaltung - Beschaeftigungsarten");
            Debug.Indent();

            List<Beschaeftigungsart> alleBeschaeftigungsArten = null;


            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleBeschaeftigungsArten = context.AlleBeschaeftigungsArten.ToList();
                }
            }
            catch (Exception Ex)
            {
                Debug.WriteLine("Fehler in Beschaeftigungsart");
                Debug.Indent();
                Debug.WriteLine(Ex.Message);
                Debug.Unindent();
                Debugger.Break();

            }


            Debug.Unindent();
            return (alleBeschaeftigungsArten);

        }

        /// <summary>
        /// Liefert alle Schulabschlüsse zurück
        /// </summary>
        /// <returns>alle Schulabschlüsse oder null bei einem Fehler</returns>
        //public static List<Schulabschluss> BildungsAngabenLaden()
        //{
        //    Debug.WriteLine("KonsumKreditVerwaltung - BildungsAngabenLaden");
        //    Debug.Indent();

        //    List<Schulabschluss> alleAbschlussArten = null;

        //    try
        //    {
        //        using (var context = new dbKreditEntities())
        //        {
        //            alleAbschlussArten = context.AlleSchulabschlussArten.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Fehler in BildungsAngabenLaden");
        //        Debug.Indent();
        //        Debug.WriteLine(ex.Message);
        //        Debug.Unindent();
        //        Debugger.Break();
        //    }

        //    Debug.Unindent();
        //    return alleAbschlussArten;
        //}

        /// <summary>
        /// Liefert alle FamilienStand zurück
        /// </summary>
        /// <returns>alle FamilienStand oder null bei einem Fehler</returns>
        public static List<FamilienStand> FamilienStandArtenLaden()
        {
            Debug.WriteLine("KonsumKreditVerwaltung - FamilienStandArtenLaden");
            Debug.Indent();

            List<FamilienStand> alleFamilienStandArten = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleFamilienStandArten = context.AlleFamilienStandArten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in FamilienStandArtenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleFamilienStandArten;
        }

        /// <summary>
        /// Liefert alle Länder zurück
        /// </summary>
        /// <returns>alle Länder oder null bei einem Fehler</returns>
        public static List<Land> LaenderLaden()
        {
            Debug.WriteLine("KonsumKreditVerwaltung - LaenderLaden");
            Debug.Indent();

            List<Land> alleLänder = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleLänder = context.AlleLaender.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in LaenderLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleLänder;
        }

        /// <summary>
        /// Liefert alle Wohnarten zurück
        /// </summary>
        /// <returns>alle Wohnarten oder null bei einem Fehler</returns>
        public static List<Wohnart> WohnartenLaden()
        {
            Debug.WriteLine("KonsumKreditVerwaltung - WohnartenLaden");
            Debug.Indent();

            List<Wohnart> alleWohnarten = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleWohnarten = context.AlleWohnarten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in WohnartenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleWohnarten;
        }

        /// <summary>
        /// Liefert alle IdentifikatikonsArt zurück
        /// </summary>
        /// <returns>alle IdentifikatikonsArt oder null bei einem Fehler</returns>
        public static List<IdentifikationsArt> IdentifikiationsAngabenLaden()
        {
            Debug.WriteLine("KonsumKreditVerwaltung - IdentifikiationsAngabenLaden");
            Debug.Indent();

            List<IdentifikationsArt> alleIdentifikationsArten = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleIdentifikationsArten = context.AlleIdentifikationsArten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in IdentifikiationsAngabenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleIdentifikationsArten;
        }

        /// <summary>
        /// Liefert alle Titel zurück
        /// </summary>
        /// <returns>alle Titel oder null bei einem Fehler</returns>
        public static List<Titel> TitelLaden()
        {
            Debug.WriteLine("KonsumKreditVerwaltung - TitelLaden");
            Debug.Indent();

            List<Titel> alleTitel = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleTitel = context.AlleTitel.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in TitelLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleTitel;
        }
        /// <summary>
        /// Liefert alle TitelNachstehend zurück
        /// </summary>
        /// <returns>alle TitelNachstehend oder null bei einem Fehler</returns>

        #endregion




        /// <summary>
        /// Speichert die Daten für die übergebene idKunde
        /// </summary>
        /// <param name="idTitel">der Titel des Kunden</param>
        /// <param name="geschlecht">das Geschlecht des Kunden</param>
        /// <param name="geburtsDatum">das Geburtsdatum des Kunden</param>
        /// <param name="vorname">der Vorname des Kunden</param>
        /// <param name="nachname">der Nachname des Kunden</param>
        /// <param name="idFamilienstand">der Familienstand des Kunden</param>
        /// <param name="idIdentifikationsart">die Identifikations des Kunden</param>
        /// <param name="identifikationsNummer">der Identifikations-Nummer des Kunden</param>
        /// <param name="idStaatsbuergerschaft">die Staatsbürgerschaft des Kunden</param>
        /// <param name="idWohnart">die Wohnart des Kunden</param>
        /// <param name="idKunde">die ID des Kunden</param>
        /// <returns>true wenn das Anpassen der Werte erfolgreich war, ansonsten false</returns>

        /// <summary>
        /// Lädt den Kunden für die übergebene ID
        /// </summary>
        /// <param name="id">die id des zu ladenden Kunden</param>
        /// <returns>der Kunde für die übergebene ID</returns>
        public static Kunde PersoenlicheDatenLaden(int id)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - PersönlicheDatenLaden");
            Debug.Indent();

            Kunde persönlicheDaten = null;

            try
            {
                using (var context = new dbKreditEntities() )
                {
                    persönlicheDaten = context.AlleKunden.Where(x => x.ID == id).FirstOrDefault();
                    Debug.WriteLine("PersönlicheDatenLaden geladen!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in PersönlicheDatenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return persönlicheDaten;
        }
        

        public static bool PersönlicheDatenSpeichern(int? idTitel, string geschlecht, DateTime geburtsDatum, string vorname, string nachname,  int idFamilienstand, int idIdentifikationsart, string identifikationsNummer, string idStaatsbuergerschaft, int idWohnart, int idKunde)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - PersönlicheDatenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditEntities())
                {

                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        aktKunde.Vorname = vorname;
                        aktKunde.Nachname = nachname;
                        aktKunde.FKFamilienstand = idFamilienstand;
                        aktKunde.FKStaatsangehoerigkeit = idStaatsbuergerschaft;
                        aktKunde.FKTitel = idTitel;
                        aktKunde.FKIdentifikationsArt = idIdentifikationsart;
                        aktKunde.IdentifikationsNummer = identifikationsNummer;
                        aktKunde.Geschlecht = geschlecht;
                        aktKunde.FKWohnart = idWohnart;
                    }

                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} PersönlicheDaten gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in PersönlicheDatenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();

                Debug.Unindent();
            }

            Debug.Unindent();
            return erfolgreich;
        }

        /// <summary>
        /// Lädt den Kreditrahmen für die übergebene ID
        /// </summary>
        /// <param name="id">die id des zu ladenden Kreditrahmens</param>
        /// <returns>der Kreditwunsch für die übergebene ID</returns>
        public static Arbeitgeber ArbeitgeberAngabenLaden(int id)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - ArbeitgeberAngabenLaden");
            Debug.Indent();

            Arbeitgeber arbeitGeber = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    arbeitGeber = context.AlleArbeitgeber.Where(x => x.ID == id).FirstOrDefault();
                    Debug.WriteLine("ArbeitgeberAngaben geladen!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in ArbeitgeberAngabenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return arbeitGeber;
        }



        /// <summary>
        /// Speichert die Angaben des Arbeitsgebers zu einem Kunden
        /// </summary>
        /// <param name="firmenName">der Firmenname des Arbeitgeber des Kunden</param>
        /// <param name="idBeschaeftigungsArt">die Beschaeftigungsart des Arbeitgeber des Kunden</param>
        /// <param name="idBranche">die Branche des Arbeitgeber des Kunden</param>
        /// <param name="beschaeftigtSeit"> BeschaeftigtSeit Wert des Kunden</param>
        /// <param name="idKunde">die ID des Kunden</param>
        /// <returns>true wenn das Speichern erfolgreich war, ansonsten false</returns>
        public static bool ArbeitgeberAngabenSpeichern(string firmenName, int idBeschaeftigungsArt, int idBranche, string beschaeftigtSeit, int idKunde)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - ArbeitgeberAngabenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditEntities())
                {

                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        Arbeitgeber neuerArbeitgeber = new Arbeitgeber()
                        {
                            BeschaeftigtSeit = DateTime.Parse(beschaeftigtSeit),
                            FKBranche = idBranche,
                            FKBeschaeftigungsArt = idBeschaeftigungsArt,
                            Firma = firmenName
                        };
                        aktKunde.Arbeitgeber = neuerArbeitgeber;
                    }

                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} ArbeitgeberDaten gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in ArbeitgeberAngabenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return erfolgreich;
        }

        public static KontoDaten KontoInformationenLaden(int id)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - KontoInformationenLaden");
            Debug.Indent();

            KontoDaten kontoDaten = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    kontoDaten = context.AlleKontoDaten.Where(x => x.ID == id).FirstOrDefault();
                    Debug.WriteLine("KontoInformationen geladen!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontoInformationenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return kontoDaten;
        }

        public static bool KontoInformationenSpeichern(string bankName, string iban, string bic, bool neuesKonto, int idKunde)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - KontoInformationenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditEntities())
                {

                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        KontoDaten kontoDaten = context.AlleKontoDaten.FirstOrDefault(x => x.ID == idKunde);

                        if (kontoDaten == null)
                        {
                            kontoDaten = new KontoDaten();
                            context.AlleKontoDaten.Add(kontoDaten);
                        }
                        kontoDaten.Bank = bankName;
                        kontoDaten.IBAN = iban;
                        kontoDaten.BIC = bic;
                        kontoDaten.HatKonto = !neuesKonto;
                        kontoDaten.ID = idKunde;
                    }

                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Konto-Daten gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontoInformationenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return erfolgreich;
        }


        /// <summary>
        /// Lädt zu einer übergebenen ID alle Informationen zu diesem Kunden aus der DB
        /// </summary>
        /// <param name="iKunde">die ID des zu landenden Kunden</param>
        /// <returns>alle Daten aus der DB zu diesem Kunden</returns>
        public static Kunde KundeLaden(int idKunde)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - KundeLaden");
            Debug.Indent();

            Kunde aktuellerKunde = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    aktuellerKunde = context.AlleKunden
                        .Include("Arbeitgeber")
                        .Include("Arbeitgeber.Beschaeftigungsart")
                        .Include("Arbeitgeber.Branche")
                        .Include("FamilienStand")
                        .Include("FinanzielleSituation")
                        .Include("IdentifikationsArt")
                        .Include("KontaktDaten")
                        .Include("KontoDaten")
                        .Include("Kredit")
                   //     .Include("Schulabschluss")
                        .Include("Titel")
                   //     .Include("TitelNachstehend")
                        .Include("Wohnart")
                        .Include("Land")
                        .Where(x => x.ID == idKunde).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KundeLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return aktuellerKunde;
        }

    }


}





