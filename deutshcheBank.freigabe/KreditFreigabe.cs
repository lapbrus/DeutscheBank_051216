using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace deutscheBank.freigabe
{ 
    public class KreditFreigabe
{
    /// <summary>
    /// Hier wird entschieden ob auf Grund der eingebenen Daten, eine Freigabe für das 
    /// Kredit erteilt wird 
    /// <param name="Vorname">der Vorname des Antragsstgellers</param>
    /// <param name="Nachname">der Nachname des Antragsstgellers</param>
    /// <param name="monatsEinkommen">der Monatseinkommen des Antragsstgellers</param>
    /// <param name="wohnKosten">die Wohnkosten des Antragsstgellers</param>
    /// <param name="einkünfteAlimente">die Einkünfte aus Alimente des Antragsstgellers</param>
    /// <param name="ausgabenAlimente">die Ausgaben für Alimente des Antragsstgellers</param>
    /// <param name="ratenZahlungen">die monatlichen Zahlungen für Raten des Antragsstgellers</param>
    /// </summary>
    /// <returns></returns>

    public static bool FreigabeErteilt(
        string geschlecht,
        string vorname,
        string nachname,
        string familienStand,
        double monatsEinkommen,
        double wohnKosten,
        double einkuenfteAlimente,
        double ausgabenAlimente,
        double ratenZahlungen)
    {
        Debug.WriteLine("KreditFreigabe - FreigabeErteilt");
        Debug.Indent();
        bool freigabe = false;

        if (string.IsNullOrEmpty(vorname))
            throw new ArgumentNullException(nameof(vorname));
        if (string.IsNullOrEmpty(nachname))
            throw new ArgumentNullException(nameof(nachname));
        if (monatsEinkommen <= 0 || monatsEinkommen > 50000)
            throw new ArgumentException($"Ungültigter Wert für {nameof(monatsEinkommen)}");
        if (wohnKosten <= 0 || wohnKosten > 10000)
            throw new ArgumentException($"Ungültigter Wert für {nameof(wohnKosten)}");
        if (einkuenfteAlimente <= 0 || einkuenfteAlimente > 10000)
            throw new ArgumentException($"Ungültigter Wert für {nameof(einkuenfteAlimente)}");
        if (ausgabenAlimente <= 0 || ausgabenAlimente > 10000)
            throw new ArgumentException($"Ungültigter Wert für {nameof(ausgabenAlimente)}");
        if (ratenZahlungen <= 0 || ratenZahlungen > 10000)
            throw new ArgumentException($"Ungültigter Wert für {nameof(ratenZahlungen)}");

        double verfügbarerBetrag = monatsEinkommen + einkuenfteAlimente - wohnKosten - einkuenfteAlimente - ausgabenAlimente - ratenZahlungen;
        double verhältnisWohkostenVerfügbarerBetrag = wohnKosten / verfügbarerBetrag;

        switch (familienStand)
        {
            case "ledig":
            case "verwitwet":
                switch (geschlecht)
                {
                    case "m":
                        freigabe = verfügbarerBetrag > wohnKosten * 2;
                        break;
                    case "w":
                        freigabe = verfügbarerBetrag > wohnKosten * 1.8;
                        break;
                    default:
                        throw new ArgumentException($"Ungültiger Wert für {nameof(geschlecht)}!\n\nNur 'm' oder 'w' erlaubt.");
                }

                break;
            case "in Partnerschaft":
            case "verheiratet":
                if (verhältnisWohkostenVerfügbarerBetrag < 0.5)
                {
                    freigabe = verfügbarerBetrag > wohnKosten * 2.5;
                }
                else
                {
                    freigabe = verfügbarerBetrag > wohnKosten * 3.5;
                }
                break;
            default:
                throw new ArgumentException($"Ungültiger Wert für {nameof(familienStand)}!\n\nNur 'ledig', 'verwitwet', 'in Partnerschaft', 'verheiratet' erlaubt.");
        }

        Debug.Unindent();
        return freigabe;


    }


}
}