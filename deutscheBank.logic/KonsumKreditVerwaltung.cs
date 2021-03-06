﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deutscheBank.logic
{
    public class KonsumKreditVerwaltung
    {

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
        #region Kreditrahmen

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
        /// Lädt die FinanzielleSituation für die übergebene ID
        /// </summary>
        /// <param name="id">die id der zu ladenden FinanzielleSituation</param>
        /// <returns>die FinanzielleSituation für die übergebene ID</returns>
        public static FinanzielleSituation FinanzielleSituationLaden(int id)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - FinanzielleSituationLaden");
            Debug.Indent();

            FinanzielleSituation finanzielleSituation = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    finanzielleSituation = context.AlleFinanzielleSituationsArten.Where(x => x.ID == id).FirstOrDefault();
                    Debug.WriteLine("FinanzielleSituation geladen!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in FinanzielleSituationLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return finanzielleSituation;
        }

        #endregion

        #region finanzielle Situation
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


        #endregion

        #region Persoenlich Daten


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

        public static List<Ort> OrteLaden()
        {
            Debug.WriteLine("KonsumKreditVerwaltung - OrteLaden");
            Debug.Indent();

            List<Ort> alleOrte = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleOrte = context.AlleOrte.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in OrteLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleOrte;
        }
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

        //public static Kunde KundeLaden(int iD_Kunde)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
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
                using (var context = new dbKreditEntities())
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


        public static bool PersönlicheDatenSpeichern(int? idTitel, string geschlecht, DateTime geburtsDatum, string vorname, string nachname, int idFamilienstand, int idIdentifikationsart, string identifikationsNummer, string idStaatsbuergerschaft, int idWohnart, int idKunde)
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
        #endregion

        #region Arbeitgeber

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

        #endregion

        #region Kontaktdaten

        public static KontaktDaten KontaktDatenLaden(int id)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - KontakdatenLaden");
            Debug.Indent();
            KontaktDaten kontaktDaten = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    kontaktDaten = context.AlleKontaktDaten.Where(x => x.ID == id).FirstOrDefault();
                    Debug.WriteLine("KontaktDaten geladen");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontaktDatenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Indent();
                Debugger.Break();
            }
            Debug.Unindent();
            return kontaktDaten;

        }

        public static bool KontaktDatenSpeichern(int id, int ort, string strasse, string hausNummer, string eMail, string telefonNummer)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - KonataktDatenSpeichern");
            Debug.Indent();
            bool erfolgreich = false;
            try
            {
                using (var context = new dbKreditEntities())
                {
                    ///Speichere zum Kunden die Angaben
                    ///
                    //  Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == id).FirstOrDefault();
                    if (aktKunde != null)
                    {
                        if (aktKunde.KontaktDaten == null)  // ist in den "KontaktDaten" schon vorhanen                                   
                            aktKunde.KontaktDaten = new KontaktDaten();

                        aktKunde.KontaktDaten.ID = id;
                        aktKunde.KontaktDaten.FKOrt = ort;
                        aktKunde.KontaktDaten.Strasse = strasse;
                        aktKunde.KontaktDaten.Hausnummer = hausNummer;
                        aktKunde.KontaktDaten.EMail = eMail;
                        aktKunde.KontaktDaten.Telefonnummer = telefonNummer;

                    }
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Kontakt-Daten gespeichert");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontaktDatenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();


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
                        .Include("Arbeitgeber.BeschaeftigungsArt")
                        .Include("Arbeitgeber.Branche")
                        .Include("Familienstand")
                        .Include("FinanzielleSituation")
                        .Include("IdentifikationsArt")
                        .Include("KontaktDaten")
                        .Include("KontoDaten")
                        .Include("AlleKredite")
                        // .Include("Schulabschluss")
                        .Include("Titel")
                        //  .Include("TitelNachstehend")
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

        public static Ort KundenOrtLaden(int id)
        {
            Debug.WriteLine("KonsumKreditVerwaltung - KundenOrtLaden");
            Debug.Indent();

            Ort Ort = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    Ort = context.AlleOrte.Where(x => x.ID == id).FirstOrDefault();
                    Debug.WriteLine("KundenOrt geladen!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KundenOrtLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return Ort;
        }

        #endregion

        #region KontoInformation

        /// <summary>
        /// Hier wird das statisch hinzugefügte Model "KontoIdentifizierungsMoeglichkeit"
        /// Als Basis genommen. Davon wird eine Liste erzeugt, die statisch 3 Einträge
        /// beinhaltet. Da es nicht notwendig ist diese ändern zu können, wird sie eben 
        /// statisch produziert.
        /// </summary>
        /// <returns>Eine Liste aus Abfragemöglichkeiten</returns>
        public static List<KontoIdentifizierungsMoeglichkeit> KontoIdentifizierungsMoeglichkeitenLaden()
        {
            List<KontoIdentifizierungsMoeglichkeit> alleKontoIdentifizierungsMoeglichkeitenAngaben = null;

            try
            {
                List<KontoIdentifizierungsMoeglichkeit> temporareKontoIdentifizierungsListe = new List<KontoIdentifizierungsMoeglichkeit>()
                {
                    new KontoIdentifizierungsMoeglichkeit() { ID = 1, Bezeichnung = "Vorhandenes Deutsche Bank AG Konto." },
                    new KontoIdentifizierungsMoeglichkeit() { ID = 2, Bezeichnung = "Neues Konto bei Deutsche Bank AG anlegen." },
                    new KontoIdentifizierungsMoeglichkeit() { ID = 3, Bezeichnung = "Anderes Konto verwenden." },
                    new KontoIdentifizierungsMoeglichkeit() { ID = 4, Bezeichnung = "Kreditkarte verwenden." }
                };

                alleKontoIdentifizierungsMoeglichkeitenAngaben = temporareKontoIdentifizierungsListe;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontoIdentifizierungsMoeglichkeitenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return alleKontoIdentifizierungsMoeglichkeitenAngaben;
        }

        /// <summary>
        /// Speichert zu einer übergebenene ID_Kunde seine Kontoinformationen ab.
        /// </summary>
        /// <param name="bic"></param>
        /// <param name="iban"></param>
        /// <param name="bankInstitut"></param>
        /// <param name="istDeutscheBankKunde"></param>
        /// <param name="idKunde"></param>
        /// <param name="datenSatzVorhanden">bei TRUE wird ein vorhandener Datensatz bearbeitet/ bei FALSE ein neuer generiert</param>
        /// <returns></returns>
        public static bool KontoInformationenSpeichern(string bic, string iban, string bankInstitut, bool istDeutscheBankKunde, int idKunde)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KontoInformationenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        if (aktKunde.KontoDaten == null)
                            aktKunde.KontoDaten = new KontoDaten();

                        /// Weise Daten den aktuellen Kunden zu (Somit kann man vorhandene Daten auch ändern)
                        aktKunde.KontoDaten.ID = idKunde;
                        aktKunde.KontoDaten.BIC = bic;
                        aktKunde.KontoDaten.IBAN = iban;
                        aktKunde.KontoDaten.Bank = bankInstitut;
                        aktKunde.KontoDaten.HatKonto = istDeutscheBankKunde;
                    }

                    /// Speichere KontoDaten (Änderungen) in die Datenbank
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Kontoinformationen gespeichert!");
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
        /// Liefert ALLE Kontodaten aus der Datenbank
        /// </summary>
        /// <param name="kundenID">ID des vorhandenen Kunden</param>
        /// <returns>Kontodaten bei Fehler NULL</returns>
        public static List<KontoDaten> KontoDatenLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KontoDatenLaden");
            Debug.Indent();

            List<KontoDaten> alleKontoDateneBL = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleKontoDateneBL = context.AlleKontoDaten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontoDatenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            Debug.Unindent();

            return alleKontoDateneBL;
        }

        /// <summary>
        /// Dieses Objekt wird zum erzeugen einer Zufallszahl benötigt.
        /// Da dies mit einem Algorithmus bezogen auf die Uhrzeit basiert,
        /// muss ich den "Random" als Membervariable initialisieren.
        /// </summary>
        static Random zufall = new Random();

        /// <summary>
        /// Erzeugt ein neues Bankkonto mittels Liste und gibt diese Liste zurück
        /// An der Stelle "0" BIC
        /// An der Stelle "1" IBAN
        /// </summary>
        /// <returns>Ein neues Bankkonto</returns>
        public static List<string> NeuesBankKontoErzeugen()
        {
            Debug.Indent();
            Debug.WriteLine("NeuesBankKontoErzeugen");

            /// Da für dieses Beispiel, wir immer nur von der selben Bank ausgehen,
            /// bleibt die Banknummer (IBAN) immer derselbe.
            const string bic = "BARCDEHAXXX";

            /// In dieser 2 dimensionalen Liste, werden die BIC´s und die IBAN´s gespeichert.
            List<string> bicUndIban = new List<string>();

            /* 
              __________________________________________________________________________________________________________
              Bestandteile des  |  Kurz-         |  Formatierung und Vergaben                          |  Beispiel
              IBAN-Standards    |  bezeichnung   |                                                     |
              __________________________________________________________________________________________________________
              Ländercode        |  LL            |   Konstant "DE"                                     |    DE
              ----------------------------------------------------------------------------------------------------------
              Prüfziffer        |  PZ            |   2-stellig, Modulus 97-10 (ISO 7064)	             |    21
              ----------------------------------------------------------------------------------------------------------
              Bankleitzahl      |  BLZ           |   Konstant 8-stellig, Bankidentifikation            |    30120400
                                |                |   entsprechend deutschem                            |
                                |                |   Bankleitzahlenverzeichnis                         |
              ----------------------------------------------------------------------------------------------------------
              Kontonummer       |  KTO           |   Konstant 10-stellig (ggf. mit vorangestellten     |    15228
                                                     Nullen) Kunden-Kontonummer
          */

            /// Dieser Teil des IBAN`s bleibt immmer gleich
            /// es werden lediglich die 10 Kundenstellen erzeugt.
            string iban = "AT78 0202 16217";

            try
            {
                int leerzeichen = 0;

                for (int i = 0; i <= 12; i++)
                {
                    Debug.Indent();
                    Debug.WriteLine(leerzeichen + " % 5 =" + leerzeichen % 5);

                    if (leerzeichen % 5 == 0)
                    {
                        iban += " ";
                    }
                    else
                    {
                        iban += zufall.Next(0, 10).ToString();
                    }

                    leerzeichen++;
                    Debug.Unindent();
                }

                bicUndIban.Add(bic);
                bicUndIban.Add(iban);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in NeuesBankKontoErzeugen");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return bicUndIban;
        }

        /// <summary>
        /// Überprüft ob das erzeugte Bankkonto schon in der 
        /// Datenbank vorhanden ist, ist dies der Fall
        /// wird ein neues Bankkonto erzeugt
        /// </summary>
        /// <returns>Bei Erfolg das neue Bankkonto / andernfalls null</returns>
        public static List<string> BankKontoErzeugen()
        {
            Debug.Indent();
            Debug.WriteLine("BankKontoErzeugen");

            List<string> neuesBankKonto = null;
            bool erfolgreich = false;

            try
            {
                do
                {
                    neuesBankKonto = NeuesBankKontoErzeugen();

                    int zaehler = 0;

                    if (KontoDatenLaden().Count != 0)
                    {
                        foreach (var item in KontoDatenLaden())
                        {
                            if (item.IBAN == neuesBankKonto[1] && zaehler != KontoDatenLaden().Count)
                            {
                                NeuesBankKontoErzeugen();
                                zaehler++;
                            }
                            else
                            {
                                erfolgreich = true;
                            }
                        }
                    }
                    else
                    {
                        erfolgreich = true;
                    }
                } while (!erfolgreich);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in NeuesBankKontoErzeugen");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }



            return neuesBankKonto;
        }

        /// <summary>
        /// Erstüberprüfung der eingegebenen Textkette, die überprüft, ob
        /// etwaige Leerzeichen vorhanden sind (die könnten an jeder Stelle stehen)
        /// und fügt stattdesen einen Leerstring "" => null ein. Was bewirkt, dass das 
        /// Leerzeichen gelöscht wird und das danachkommende Zeichen (Buchstabe/Zahl)
        /// in der Zeichenkette, was auch als Liste von Caracter bekannt ist, automatisch auf.
        /// </summary>
        /// <param name="eingabeIBAN"></param>
        /// <returns>Textkette ohne Leerzzeichen</returns>
        public static string FilterAufVorhandeneLeerzeichen(string eingabeIBAN)
        {
            string temporareKontoIdentifizierungsListe = "";
            if (eingabeIBAN != null)
            {
                for (int i = 0; i < eingabeIBAN.Length; i++)
                {
                    if (eingabeIBAN[i] == ' ')
                        temporareKontoIdentifizierungsListe += "";
                    else
                        temporareKontoIdentifizierungsListe += eingabeIBAN[i];
                }

                eingabeIBAN = temporareKontoIdentifizierungsListe;

            }

            return eingabeIBAN;
        }

        /// <summary>
        /// Filtert eine Texteingabe und fügt an jeder 4ten Stelle ein Leerzeichen " " ein. 
        /// </summary>
        /// <param name="eingabeIBAN">Die Texteingabe die gefiltert werden soll.</param>
        /// <returns>überarbeitete Texteingabe</returns>
        public static string LeerzeichenEinfuegen(string eingabeIBAN)
        {
            string temporareKontoIdentifizierungsListe = "";
            int leerzeichen = 1;

            if (eingabeIBAN != null)
            {
                for (int j = 0; j < eingabeIBAN.Length; j++)
                {
                    if (leerzeichen % 5 == 0)
                    {
                        for (int i = 0; i < (eingabeIBAN.Length); i++)
                        {
                            if (i == (leerzeichen - 1))
                            {
                                temporareKontoIdentifizierungsListe += " ";
                                temporareKontoIdentifizierungsListe += eingabeIBAN[i];
                            }
                            else
                            {
                                temporareKontoIdentifizierungsListe += eingabeIBAN[i];
                            }
                        }
                        eingabeIBAN = temporareKontoIdentifizierungsListe;
                        temporareKontoIdentifizierungsListe = "";
                    }
                    leerzeichen++;
                }
            }

            return eingabeIBAN;
        }

        /// <summary>
        /// Liefert die aktuellen, KontoDaten des Kunden
        /// </summary>
        /// <param name="kundenID">ID des vorhandenen Kunden</param>
        /// <returns>Kontodaten bei Fehler NULL</returns>
        public static KontoDaten KontoDatenLaden(int idKunde)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KontaktDatenLaden");
            Debug.Indent();

            KontoDaten aktKontoDaten = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    aktKontoDaten = context.AlleKontoDaten.Where(x => x.ID == idKunde).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung KontaktDatenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
                Debug.Unindent();
            }

            return aktKontoDaten;
        }


        public static bool KreditKontoInformationenSpeichern(string inhaber, string kartenNummer, string gueltigBis, string cvcNummer, int idKunde)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KreditkartenformationenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    //Speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        if (aktKunde.KundenKreditkarte == null)
                            aktKunde.KundenKreditkarte = new KundenKreditkarte();
                    }
                    /// Weise Daten den aktuellen Kunden zu (Somit kann man vorhandene Daten auch ändern)
                    aktKunde.KundenKreditkarte.ID = idKunde;
                    aktKunde.KundenKreditkarte.Inhaber = inhaber;
                    aktKunde.KundenKreditkarte.KartenNummer = kartenNummer;
                    aktKunde.KundenKreditkarte.Gueltigkeit = gueltigBis;
                    aktKunde.KundenKreditkarte.CVC_Nummer = cvcNummer;



                    /// Speichere KontoDaten (Änderungen) in die Datenbank
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Kreditkarteninformationen gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KreditkartenInformationenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return erfolgreich;
        }





        #endregion

        #region AntragBezahltPruefen
        /// <summary>
        /// Liefert den aktuellen Stand ob 
        /// die geforderte Gebuehr bezahlt wurde
        /// </summary>
        /// <param name="ID">ID des vorhandenen Kunden</param>
        /// <returns>Kontodaten bei Fehler NULL</returns>
        public static AntragBezahlt AntragsKonditionLaden(int idKunde)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - AntragsDatenLaden");
            Debug.Indent();

            AntragBezahlt aktAntragBezahlt = null;
            try
            {
                using (var context = new dbKreditEntities())
                {
                    aktAntragBezahlt = context.AlleAntragBezahltKonditionen.Where(x => x.ID == idKunde).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung AntragKonditionLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
                Debug.Unindent();
            }

            return aktAntragBezahlt;
        }


        public static bool AntragsKonditionSpeichern(bool bezahlt, int idKunde)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - AntragKonditionSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    //Speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        if (aktKunde.AntragBezahlt == null)
                            aktKunde.AntragBezahlt = new AntragBezahlt();
                    }
                    /// Weise Daten den aktuellen Kunden zu (Somit kann man vorhandene Daten auch ändern)
                    aktKunde.AntragBezahlt.ID = idKunde;
                    aktKunde.AntragBezahlt.Bezahlt = bezahlt;


                    /// Speichere KontoDaten (Änderungen) in die Datenbank
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} AntragsKondition gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in AntragskonditionSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return erfolgreich;
        }

        public static bool KontoInformationenZurAbbuchungSpeichern(string bic, string iban, string bank, int idKunde)
        {
             
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KontoInformationenZurAbbuchungSpeichern");
            Debug.Indent();

            bool erfolgreich = false;
            List<KontoDatenZurAbbuchung> alleKontoDateneZurAbbuchungBL = null;

            try
            {
                using (var context = new dbKreditEntities())
                {
                    alleKontoDateneZurAbbuchungBL = context.AlleKontoDatenZurAbbuchung.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontoInformationenZurAbbuchungSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

                Debug.Unindent();
                return erfolgreich;
        }
                

             
    }


    #endregion


    #region Zusammenfassung

    #endregion

    // keine Eintraege in die Datenbank benötigt 
    public class KontoIdentifizierungsMoeglichkeit
        {
            public int ID { get; set; }

            public string Bezeichnung { get; set; }
        }


    }



    









