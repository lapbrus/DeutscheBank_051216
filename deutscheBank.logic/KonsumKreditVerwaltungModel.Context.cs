﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbKreditEntities : DbContext
    {
        public dbKreditEntities()
            : base("name=dbKreditEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Arbeitgeber> AlleArbeitgeber { get; set; }
        public DbSet<Beschaeftigungsart> AlleBeschaeftigungsArten { get; set; }
        public DbSet<Branche> AlleBranchen { get; set; }
        public DbSet<Einstellungen> tblEinstellungen { get; set; }
        public DbSet<FinanzielleSituation> AlleFinanzielleSituationsArten { get; set; }
        public DbSet<IdentifikationsArt> AlleIdentifikationsArten { get; set; }
        public DbSet<KontaktDaten> AlleKontaktDaten { get; set; }
        public DbSet<KontoDaten> AlleKontoDaten { get; set; }
        public DbSet<Kredit> AlleKredite { get; set; }
        public DbSet<Kunde> AlleKunden { get; set; }
        public DbSet<Land> AlleLaender { get; set; }
        public DbSet<login> tbllogin { get; set; }
        public DbSet<Ort> AlleOrte { get; set; }
        public DbSet<Schulabschluss> AlleSchulabschlussArten { get; set; }
        public DbSet<Titel> AlleTitel { get; set; }
        public DbSet<Wohnart> AlleWohnarten { get; set; }
        public DbSet<FamilienStand> AlleFamilienStandArten { get; set; }
    }
}
