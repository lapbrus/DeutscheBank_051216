//------------------------------------------------------------------------------
// <auto-generated>
//    Dieser Code wurde aus einer Vorlage generiert.
//
//    Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten Ihrer Anwendung.
//    Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace deutscheBank.logic
{
    using System;
    using System.Collections.Generic;
    
    public partial class KundenKreditkarte
    {
        public int ID { get; set; }
        public string Inhaber { get; set; }
        public string KartenNummer { get; set; }
        public string Gueltigkeit { get; set; }
        public string CVC_Nummer { get; set; }
    
        public virtual Kunde Kunde { get; set; }
    }
}
