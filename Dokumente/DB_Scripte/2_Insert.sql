use dbKreditRechner
go

-- Land in tblLand eintragen
insert into tblland
select [Spalte 0], [Spalte 1] from L�nderneu
where [Spalte 0] <> ''
GO

-- OrtPLZ in tblPLZOrt eintragen
insert into tblort 
select plz, 'AUT',ort from  [LandOrt].dbo.tblOrt

-- Lookup Tabellen mit Werten bef�llen
insert into [dbo].[tblBeschaeftigungsart]
values
('Angestellter'),
('Arbeiter'),
('Pensionist'),
('Arbeitslos')


insert into tblbranche values
('Land- und Forstwirtschaft'),
('Fischerei'),
('Bergbau'),
('Verarbeitendes Gewerbe/Herstellung von Waren'),
('Energieversorgung'),
('Baugewerbe/Bau'),
('Handel'),
('Verkehr'),
('Gastgewerbe/Beherbergung und Gastronomie'),
('Information und Kommunikation'),
('Grundst�cks- und Wohnungswesen'),
('Gesundheits- und Sozialwesen'),
('�ffentliche Verwaltung'),
('Dienstleistungen')

insert into tblFamilienstand
values('ledig'),('Verheiratet'),('Verwitwet'),('Geschieden');

insert into tblIdentifikationsArt
values ('Reisepass'),('Personalausweis');

 insert into tblSchulAbschluss
 values ('Pflichtschule'),('Lehre'),('Mittlere Schule'),('Matura'),
 ('Fachhochschule/Universit�t');
 
  insert into tblWohnart
 values('Miete'),('Eigentum'),('Genossenschaft'),('Wohngemeinschaft');

 insert into tblTitel
values ('DI'),('Dipl.Ta.'),('Dipl.Vw.'),('Dir.'),('Dkfm.'),('Dr.'),
('Ing.'),('Mag.'),('Prof.'),('Univ.-Prof.'),('Gen.Dir.'),('Gen.Manager'),
('DI(FH)'),('DDr.'),('Dipl.HTL-Ing.'),('Mag.(FH)'),('Dipl.Kffr.'),
('MBA'),('Dipl.P�d.'),('MA'),('MMag.'),('MSc'),('Dipl.Oek.'),('BA'),
('Bakk.'),('Bakk.(FH)'),('BSc'),('PhD');


