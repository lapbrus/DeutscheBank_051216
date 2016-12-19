-- DML
insert - eintragen
update - aktualisieren
delete - löschen

use dbTeilnehmer

-- eintragen von DS
-- mit values trägt man EINEN DS ein
insert into tblklasse (idklasse,raumnr, sitzplätze)
values('IN20','512',20)

-- man kann die Spaltennamen weglassen, wenn man alle Felder
-- in der Reihenfolge befüllt, wie sie in der Tabelle angegeben sind. 
insert into tblgerät
values
('IN19','Drucker','12345'),
('IN19','Projektor','34455'),
('IN20','Dönerbratgerät','D4566')

-- eintragen von DS aus einer Tabelle
insert into tblTeilnehmer(FKKlasse, Vorname, Nachname, PLZ)
select 'IN20', au_fname, au_lname, left(zip,4)
from pubs.dbo.authors

-- Funktionen left, right
select au_fname, 
left(au_fname, 4) as Kürzel, right(au_fname,2)
from pubs.dbo.authors

select left('Hallo',2)
-- Datenbankzugriff auf andere DB erfolgt mit
-- DBNamen.Schemaname.Objektname

-- löschen von DS
-- löscht ALLE DS der Tabelle
delete from tblteilnehmer

-- delete kann auch eingeschränkt mit der where Klausel
delete from tblTeilnehmer
where idteilnehmer > 2016

-- update ändert bestehende DS
update tblTeilnehmer set
	Feld = Wert,
	Feld = Wert
where .... 

-- ohne where Klausel werden ALLE DS geändert
update tblTeilnehmer set
	istmaab = 0

-- mit where Klausel
update tblTeilnehmer set
	vorname = '',
	nachname = '',
	einstieg = NULL, 
	grösse = 205
where fkklasse = 'IN20'



select * from tblTeilnehmer





select * from tblgerät
select * from tblklasse
select * from tblteilnehmer

--- 

use dbteilnehmer
go
select * from tblKlasse
select * from tblTrainer
select * from tblGerät
select * from tblTrainerKlasse
select * from tblTeilnehmer

-- tragen Sie einen neuen TN Fritz Müller in der TE19
insert into tblteilnehmer(vorname, Nachname, FKklasse)
values('Fritz','Müller','TE19')

-- erhöhen Sie das Gehalt von Frau Jungwirth um 30%
update tbltrainer set
	gehalt = gehalt * 1.3
where trnn = 'jungwirth'

-- Geben Sie alle Klassen von Herrn Sippl aus
select trnn, fkklasse
from tbltrainer
	join tblTrainerKlasse on IDTrainer = FKTrainer
where trnn = 'sippl'

-- Welche TN der IN19 kommen aus Wien, sortiert nach Nachname
select nachname
from tblTeilnehmer 
	join tblklasse on IDKlasse = FKKlasse
where ort = 'wien' and idklasse = 'in19'
order by nachname

-- Welche Nicht-MAAB TN der IN19 sind größer als 170cm?
select fkklasse, istmaab, vorname, nachname, grösse
from tblTeilnehmer 
where IstMAAB = 0 and fkklasse = 'in19' and grösse > 170

-- Welche Geräte befinden sich in der IN19?
select *
from tblgerät
where fkklasse = 'in19'

-- Hat Herr Grübel Herrn Sippl als Trainer?
if exists(select nachname, trnn
			from tblTeilnehmer 
				join tblklasse on IDKlasse = fkklasse
				join tblTrainerKlasse as tk on idklasse = tk.fkklasse
				join tblTrainer on IDTrainer = FKTrainer
			where nachname = 'grübel' and trnn = 'sippl') 
	select 'ja' as Erg
else
	select 'nein' as Erg
go

select case 
when geschlecht = 'm' then 'Herr' 
when geschlecht = 'k' then 'Kind'
else 'Frau' end as Anrede, 
vorname, nachname
from tblteilnehmer



-- einfachere Schreibweise wenn der Operator ein = ist
select case geschlecht
when 'm' then 'Herr' 
when 'k' then 'Kind'
else 'Frau' end as Anrede, 
vorname, nachname
from tblteilnehmer

select trnn, 
case 
when gehalt > 1700 then 'gut' 
when gehalt < 900 then 'pleite'
else 'schlecht' end as Info
from tbltrainer

-- Wie alt sind die Männer in der IN19?
select geburtsdatum, nachname, 
year(getdate()) - year(geburtsdatum) as TNAlter,
DATEDIFF(day,geburtsdatum,getdate())/365.0 as AlterTN
from tblteilnehmer
where geschlecht = 'm' and fkklasse = 'in19'

-- /neuer abschnitt
use pubs
go

/************ ZEICHENKETTENFUNKTIONEN ***********************/
--	UPPER
--	
--	Gibt den uebergebenen Character in Grossbuchstaben zurueck
--	
--	Parameter:			[Character or Binary data | kann eine Konstante, Variable oder Spalte sein]
--						muss implizit in ein Varcharacter konvertierbar sein ansonsten muss man es mittels CAST oder CONVERT explizit konvertieren
--	Rueckgabetyp:		VARCHAR or NVARCHAR

--	ohne UPPER
SELECT 'google ist gross'

--	mit UPPER
SELECT UPPER('google') + ' ist ' + UPPER('gross')

--	mit UPPER auf Wert einer Spalte
SELECT UPPER(fname) FROM employee

--	LOWER
--	
--	Gibt den uebergebenen Character in Kleinbuchstaben zurueck
--	
--	Parameter:			[Character or Binary data | kann eine Konstante, Variable oder Spalte sein]
--						muss implizit in ein Varcharacter konvertierbar sein ansonsten muss man es mittels CAST oder CONVERT explizit konvertieren
--	Rueckgabetyp:		VARCHAR or NVARCHAR

--	ohne LOWER
SELECT 'MICROSOFT ist BOESE'

--	mit LOWER
SELECT LOWER('MICROSOFT') + ' ist ' + LOWER('BOESE')

--	mit LOWER auf Wert einer Spalte
SELECT LOWER(fname) FROM employee


------------------------------------------------------------------------------


--									S U B S T R I N G

 -- Substring erlaubt dem Benutzer einen bestimmten Teil eines Datensatzes auszulesen.
 -- Deklaration: select Substring(Datentyp, Start, Länge)
select SUBSTRING(au_lname,2,3) as Teil_des_Nachnamens -- Startet in diesem Fall beim 2. Zeichen und gibt die folgenden 3 Zeichen aus 
from authors
where au_lname = 'White'

--------------------------------------------------------------------------------


-- **** ascii()

-- Gibt den ASCII-Codewert des ersten Zeichens eines Zeichenausdrucks zurück.

-- ASCII ( character_expression )

-- character_expression Ist ein Ausdruck vom Datentyp char oder varchar

use pubs
go



select ASCII('A')

select ASCII('k')

select ASCII('1')

select ASCII('?')


-- Beispiel aus der PUBS datenbank:

-- Einzelne suche

select ASCII(au_lname), au_lname
	from authors
where au_lname = 'Bennet'



-- Ganze spalte

select ASCII(fname), fname
	from employee
where job_id = 13



-------------------------------------


select ASCII('Google.at')  -- Nur der erste Buchstabe wird bewertet


select ASCII('Hallo Welt, DO, PR, ST!') -- Nur der erste Buchstabe wird bewertet



--#####################################################################################





--#####################################################################################


-- **** char()

-- Wandelt einen int-ASCII-Code in ein Zeichen um.

-- CHAR ( integer_expression )


-- integer_expression   Eine ganze Zahl zwischen 0 und 255. NULL wird zurückgegeben, 
--                      wenn der ganzzahlige Ausdruck außerhalb dieses Bereichs liegt.


-- char- oder varchar-Daten können ein Zeichen enthalten oder aus einer Zeichenfolge
-- mit maximal 8.000 Zeichen (char-Daten) bzw. 2^31 Zeichen (varchar-Daten) bestehen.


-- Char funktion ( ascii code )
-- Der Charakter gibt mit der Zahl den Ascii Wert raus


SELECT CHAR(116);


SELECT CHAR(84);


SELECT CHAR(65);



-- Beispiel aus der PUBS datenbank:


select fname + ' ' + CHAR(86) + '. ' + lname
from employee





--Charindex

--Charindex ist case sensitive, Die funktion gibt die erste Stelle des Gesuchten Wortes zurück
--Rückgabetyp: int
select CHARINDEX('Moon',pub_name)
from publishers

select *
from publishers


select CHARINDEX('Test','Das ist ein Test Text!')












/************ DATUMSFUNKTIONEN ***********************/
--	GETUTCDATE
--	
--	Gibt das aktuelle Datum und Uhrzeit des DB-Systems vobei der Zeitzonenoffset nicht einbezogen wird(UTC-Zeit)
--	Heisst wenn man in der Zeitzone +2 ist werden die 2 Stunden abgezogen. Aus 12 Uhr wird 10
--	
--	Parameter:			----
--	Rueckgabetyp:		DATETIME

--	Aktuelle Zeit (mit Zeitzonenoffset)
SELECT GETDATE()

--	Aktuelle Zeit in UTC-Zeit (ohne Zeitzonenoffset)
SELECT GETUTCDATE()

----------------------------------------------------------------------------------

--									D A T E N A M E

-- Gibt das gewählte Format (year, quater, month, week, day,...) eines Datum Datentypes zurück.
select DATENAME(WEEKDAY, ord_date) as Wochentag  -- Der Wochentag wird von ord_date definiert und zurückgegeben.
from sales
where title_id = 'PC8888'

select DATENAME(MONTH, '06 05.1989') as Wochentag  -- Der Wochentag wird von ord_date definiert und zurückgegeben.
from sales
where title_id = 'PC8888'


-- ('End of month')
-- Eomonth():   Gibt den letzten Tag des Monats, der das angegebene Datum enthält,
--				mit einem optionalen Versatz zurück
-- (zB. um Taggeld zu berechnen)

select eomonth('12/1/2011') as Result;

select eomonth(hire_date)
	from employee



-- dateadd():   Wenn der angegeben Tag des Monats größer ist als der des Nachfolge-
--				monats, dann wird das Folgemonat mit dem maximal möglichen Monat an-
--				gegeben, .zB. 31.8.2011 um 1 Monat zu erhöhen wäre dann 30.9.2011

select dateadd(month, 1, '2011/30/8')

select dateadd(day, 1, '2011/30/8')

select dateadd(month, 1, '2011/31/8')

select dateadd(year, 1, '2011/31/8')

Select dateadd(quarter, 1, '2011/31/8')

select dateadd(month, 1, '2016/31/01')

select dateadd(month, 1, '31.1.2016')


update employee set
	hire_date = DATEADD(month, 1, hire_date)
where fname = 'Paolo'


select *
from employee 
where fname = 'Paolo'


-- DATEPART
--Rückgabetyp: int
--Gibt einen Teil von einem Datum zurück.

select ord_date
from sales

select DATEPART(yyyy,ord_date) as Jahr, DATEPART(mm,ord_date) as Monat,DATEPART(dd,ord_date)as Tag ,DATEPART(dy,ord_date) as TagvomJahr
from sales


/*
year		yy, yyyy
quarter		qq, q
month		mm, m
dayofyear	dy, y
day			dd, d
week		wk, ww
weekday		dw, w
hour		hh
minute		mi, n
second		ss, s
*/



select DATEPART(yyyy,ord_date)
from sales

select DATEPART(mm,ord_date)
from sales

select DATEPART(dy,ord_date)
from sales






/************ MATH.FUNKTIONEN ***********************/
--	POWER
--	
--	Gibt die Potenz von den angegebenen Parametern zurueck
--
--	Parameter:			[FLOAT | muss implizit in ein FLOAT konvertierbar sein], [Numerischer Datenyp | mit Ausnahme von BIT]
--	RueckgabeTyp		FLOAT

--	POWER mit INTEGER und INTEGER
SELECT POWER(5,3)

--	POWER mit FLOAT und FLOAT
SELECT POWER(13.96, 3.4)

--	mit POWER auf Wert einer Spalte
SELECT POWER(price, 2) FROM titles


****
SQRT	 ==>		Gibt die Quadratwurzel des angegebenen float-Werts zurück.	
****
		   Syntax:	 SQRT ( float_expression )
 float_expression:	 Ein Ausdruck vom Typ float od. von einem Typ, der implizit in float 
						 konvertiert werden kann.
	  Rückgabetyp:	 float
----------------------------------------------------------------------------------------


******
SQUARE	 ==>		Gibt das Quadrat des angegebenen float-Werts zurück.
******
		   Syntax:	 SQUARE ( float_expression )
 float_expression:	 Ein Ausdruck vom Typ float od. von einem Typ, der implizit in float 
						 konvertiert werden kann.
	  Rückgabetyp:	 float
----------------------------------------------------------------------------------------


Beispiele:
----------
*/
use pubs go


--(1)--
select price as Buchpreis, square(price) as [Buchpreis zum Quadrat]
from titles

--(2)--
select price as Buchpreis, sqrt(price) as [Wurzel aus Buchpreis]
from titles


--(3)--
select price as Buchpreis, square(price) as [Buchpreis zum Quadrat], 
						   sqrt(square(price)) as [Wurzel aus quadriertem Buchpreis],
						   sqrt(price) as [Wurzel aus Buchpreis] 
from titles

select square(5), sqrt(25)



--------------------------------------------------------------------------------

--							R O U N D

-- Rundet den angegebenen Wert-Datentyp
-- Deklatation: select Round(Wert-Datentyp, Länge auf der gerundet werden soll (wie Excel), [0]) 0 == optional und es wird gerundet. Wird 0 weggelassen wird der Wert abgeschnitten
select ROUND(price,1) , price 
from titles
order by price desc

select ROUND(price,-1) , price 
from titles
order by price desc
---------------------------------------------------------------------------------

/************ SYSTEMFUNKTIONEN ***********************/
--	ISNUMERIC
--	
--	Prueft ob der Angegebene Parameter ein gueltiger Nummerischer Datentyp ist
--	Gueltige Nummerische Datentypen sind(INT, BIGINT, DECIMAL, SMALLINT, TINYINT, FLOAT, MONEY, SMALLMONEY, REAL, NUMERIC)
--	Gibt 1 zurueck wenn der Ausdruck ein Nummerischer Datentyp ist ansonsten wird 0 zurueckgegeben
--	
--	Parameter:			Irgendein Ausdruck
--	Rueckgabetyp:		INT

--	ISNUMERIC mit String
SELECT ISNUMERIC('Bin ich nummerisch?')

--	ISNUMERIC mit FLOAT
SELECT ISNUMERIC(17.1)

--	ISNUMERIC mit GETDATE()
SELECT ISNUMERIC(GETDATE())

--	ISNUMERIC mit INT
SELECT ISNUMERIC(2)

------------------------------------------------------------------------------

--							R O W C O U N T

--Gibt die Anzahl der Zeilen zurück, auf die sich die letzte Anweisung
-- ausgewirkt hat. Beträgt die Anzahl der Zeilen mehr als 2 Milliarden, 
-- verwenden Sie ROWCOUNT_BIG.
-- Hat einen Rückgabetypen des Typen int (Integer)
insert into jobs(job_desc, min_lvl, max_lvl) 
values
('Programmierer', 10 , 20)

select @@ROWCOUNT






--Username

       --Returns a database user name from a specified identification number.

       -- Gibt den Datenbank Usernamen einer bestimmten ID zurueck

       select USER_NAME(1) as Erster ,USER_NAME(2) as Zweiter,USER_NAME(3) as Dritter,USER_NAME(4) as Vierter,
                    USER_NAME(5) as Fuenfter,USER_NAME(6) as Sechster,USER_NAME(7) as Siebter 
       go

       select USER_NAME() as CurrentUser



	   --Is Null

       
       -- ISNULL ( check_expression , replacement_value )
       --***Argumente***
       --check_expression
       --Der Ausdruck, der auf NULL überprüft werden soll. check_expression kann einen beliebigen Typ aufweisen.
       --replacement_value
       --Der Ausdruck, der zurückgegeben werden soll, wenn check_expression NULL ist. 
       --Der Datentyp von replacement_value muss implizit in den Typ von check_expresssion konvertiert werden können.
             
             --**** INT Beispiel
             select * from titles
             where price is null 

             select ISNULL(price,0.00)
              from titles
                    -- Auf gut deutsch , dort wo der Gesuchte Datensatz NULL ist , wird stattdessen 0.00 angezeigt 

             -- Text Beispiel
       select * from titles 
       where notes is null

       select isnull(notes,'Nicht Vorhanden') from titles
                    -- Hier wird statt NUll  der Beispieltext('Nicht Vorhanden') angezeigt 

                    -- Also wie man sieht verhindert man damit die Ausgabe von NULL ,
                    -- ändert jedoch NICHT dauerhaft den Datzensatz
                    -- Der Datentyp der Ersatzanzeige muss dem des gesuchten Datensatzes entsprechen 
                    -- Der typ des gesuchten muss mit der gewuenschten ersatzausgabe übereinstimmen

					
-- N E U
use pubs
go

/************ ZEICHENKETTENFUNKTIONEN ***********************/
--	UPPER
--	
--	Gibt den uebergebenen Character in Grossbuchstaben zurueck
--	
--	Parameter:			[Character or Binary data | kann eine Konstante, Variable oder Spalte sein]
--						muss implizit in ein Varcharacter konvertierbar sein ansonsten muss man es mittels CAST oder CONVERT explizit konvertieren
--	Rueckgabetyp:		VARCHAR or NVARCHAR

--	ohne UPPER
SELECT 'google ist gross'

--	mit UPPER
SELECT UPPER('google') + ' ist ' + UPPER('gross')

--	mit UPPER auf Wert einer Spalte
SELECT UPPER(fname) FROM employee

--	LOWER
--	
--	Gibt den uebergebenen Character in Kleinbuchstaben zurueck
--	
--	Parameter:			[Character or Binary data | kann eine Konstante, Variable oder Spalte sein]
--						muss implizit in ein Varcharacter konvertierbar sein ansonsten muss man es mittels CAST oder CONVERT explizit konvertieren
--	Rueckgabetyp:		VARCHAR or NVARCHAR

--	ohne LOWER
SELECT 'MICROSOFT ist BOESE'

--	mit LOWER
SELECT LOWER('MICROSOFT') + ' ist ' + LOWER('BOESE')

--	mit LOWER auf Wert einer Spalte
SELECT LOWER(fname) FROM employee


------------------------------------------------------------------------------


--									S U B S T R I N G

 -- Substring erlaubt dem Benutzer einen bestimmten Teil eines Datensatzes auszulesen.
 -- Deklaration: select Substring(Datentyp, Start, Länge)
select SUBSTRING(au_lname,2,3) as Teil_des_Nachnamens -- Startet in diesem Fall beim 2. Zeichen und gibt die folgenden 3 Zeichen aus 
from authors
where au_lname = 'White'

--------------------------------------------------------------------------------


-- **** ascii()

-- Gibt den ASCII-Codewert des ersten Zeichens eines Zeichenausdrucks zurück.

-- ASCII ( character_expression )

-- character_expression Ist ein Ausdruck vom Datentyp char oder varchar

use pubs
go



select ASCII('A')

select ASCII('k')

select ASCII('1')

select ASCII('?')


-- Beispiel aus der PUBS datenbank:

-- Einzelne suche

select ASCII(au_lname), au_lname
	from authors
where au_lname = 'Bennet'



-- Ganze spalte

select ASCII(fname), fname
	from employee
where job_id = 13



-------------------------------------


select ASCII('Google.at')  -- Nur der erste Buchstabe wird bewertet


select ASCII('Hallo Welt, DO, PR, ST!') -- Nur der erste Buchstabe wird bewertet



--#####################################################################################





--#####################################################################################


-- **** char()

-- Wandelt einen int-ASCII-Code in ein Zeichen um.

-- CHAR ( integer_expression )


-- integer_expression   Eine ganze Zahl zwischen 0 und 255. NULL wird zurückgegeben, 
--                      wenn der ganzzahlige Ausdruck außerhalb dieses Bereichs liegt.


-- char- oder varchar-Daten können ein Zeichen enthalten oder aus einer Zeichenfolge
-- mit maximal 8.000 Zeichen (char-Daten) bzw. 2^31 Zeichen (varchar-Daten) bestehen.


-- Char funktion ( ascii code )
-- Der Charakter gibt mit der Zahl den Ascii Wert raus


SELECT CHAR(116);


SELECT CHAR(84);


SELECT CHAR(65);



-- Beispiel aus der PUBS datenbank:


select fname + ' ' + CHAR(86) + '. ' + lname
from employee





--Charindex

--Charindex ist case sensitive, Die funktion gibt die erste Stelle des Gesuchten Wortes zurück
--Rückgabetyp: int
select CHARINDEX('Moon',pub_name)
from publishers

select *
from publishers


select CHARINDEX('Test','Das ist ein Test Text!')












/************ DATUMSFUNKTIONEN ***********************/
--	GETUTCDATE
--	
--	Gibt das aktuelle Datum und Uhrzeit des DB-Systems vobei der Zeitzonenoffset nicht einbezogen wird(UTC-Zeit)
--	Heisst wenn man in der Zeitzone +2 ist werden die 2 Stunden abgezogen. Aus 12 Uhr wird 10
--	
--	Parameter:			----
--	Rueckgabetyp:		DATETIME

--	Aktuelle Zeit (mit Zeitzonenoffset)
SELECT GETDATE()

--	Aktuelle Zeit in UTC-Zeit (ohne Zeitzonenoffset)
SELECT GETUTCDATE()

----------------------------------------------------------------------------------

--									D A T E N A M E

-- Gibt das gewählte Format (year, quater, month, week, day,...) eines Datum Datentypes zurück.
select DATENAME(WEEKDAY, ord_date) as Wochentag  -- Der Wochentag wird von ord_date definiert und zurückgegeben.
from sales
where title_id = 'PC8888'

select DATENAME(MONTH, '06 05.1989') as Wochentag  -- Der Wochentag wird von ord_date definiert und zurückgegeben.
from sales
where title_id = 'PC8888'


-- ('End of month')
-- Eomonth():   Gibt den letzten Tag des Monats, der das angegebene Datum enthält,
--				mit einem optionalen Versatz zurück
-- (zB. um Taggeld zu berechnen)

select eomonth('12/1/2011') as Result;

select eomonth(hire_date)
	from employee



-- dateadd():   Wenn der angegeben Tag des Monats größer ist als der des Nachfolge-
--				monats, dann wird das Folgemonat mit dem maximal möglichen Monat an-
--				gegeben, .zB. 31.8.2011 um 1 Monat zu erhöhen wäre dann 30.9.2011

select dateadd(month, 1, '2011/30/8')

select dateadd(day, 1, '2011/30/8')

select dateadd(month, 1, '2011/31/8')

select dateadd(year, 1, '2011/31/8')

Select dateadd(quarter, 1, '2011/31/8')

select dateadd(month, 1, '2016/31/01')

select dateadd(month, 1, '31.1.2016')


update employee set
	hire_date = DATEADD(month, 1, hire_date)
where fname = 'Paolo'


select *
from employee 
where fname = 'Paolo'


-- DATEPART
--Rückgabetyp: int
--Gibt einen Teil von einem Datum zurück.

select ord_date
from sales

select DATEPART(yyyy,ord_date) as Jahr, DATEPART(mm,ord_date) as Monat,DATEPART(dd,ord_date)as Tag ,DATEPART(dy,ord_date) as TagvomJahr
from sales


/*
year		yy, yyyy
quarter		qq, q
month		mm, m
dayofyear	dy, y
day			dd, d
week		wk, ww
weekday		dw, w
hour		hh
minute		mi, n
second		ss, s
*/



select DATEPART(yyyy,ord_date)
from sales

select DATEPART(mm,ord_date)
from sales

select DATEPART(dy,ord_date)
from sales






/************ MATH.FUNKTIONEN ***********************/
--	POWER
--	
--	Gibt die Potenz von den angegebenen Parametern zurueck
--
--	Parameter:			[FLOAT | muss implizit in ein FLOAT konvertierbar sein], [Numerischer Datenyp | mit Ausnahme von BIT]
--	RueckgabeTyp		FLOAT

--	POWER mit INTEGER und INTEGER
SELECT POWER(5,3)

--	POWER mit FLOAT und FLOAT
SELECT POWER(13.96, 3.4)

--	mit POWER auf Wert einer Spalte
SELECT POWER(price, 2) FROM titles


****
SQRT	 ==>		Gibt die Quadratwurzel des angegebenen float-Werts zurück.	
****
		   Syntax:	 SQRT ( float_expression )
 float_expression:	 Ein Ausdruck vom Typ float od. von einem Typ, der implizit in float 
						 konvertiert werden kann.
	  Rückgabetyp:	 float
----------------------------------------------------------------------------------------


******
SQUARE	 ==>		Gibt das Quadrat des angegebenen float-Werts zurück.
******
		   Syntax:	 SQUARE ( float_expression )
 float_expression:	 Ein Ausdruck vom Typ float od. von einem Typ, der implizit in float 
						 konvertiert werden kann.
	  Rückgabetyp:	 float
----------------------------------------------------------------------------------------


Beispiele:
----------
*/
use pubs go


--(1)--
select price as Buchpreis, square(price) as [Buchpreis zum Quadrat]
from titles

--(2)--
select price as Buchpreis, sqrt(price) as [Wurzel aus Buchpreis]
from titles


--(3)--
select price as Buchpreis, square(price) as [Buchpreis zum Quadrat], 
						   sqrt(square(price)) as [Wurzel aus quadriertem Buchpreis],
						   sqrt(price) as [Wurzel aus Buchpreis] 
from titles

select square(5), sqrt(25)



--------------------------------------------------------------------------------

--							R O U N D

-- Rundet den angegebenen Wert-Datentyp
-- Deklatation: select Round(Wert-Datentyp, Länge auf der gerundet werden soll (wie Excel), [0]) 0 == optional und es wird gerundet. Wird 0 weggelassen wird der Wert abgeschnitten
select ROUND(price,1) , price 
from titles
order by price desc

select ROUND(price,-1) , price 
from titles
order by price desc
---------------------------------------------------------------------------------

/************ SYSTEMFUNKTIONEN ***********************/
--	ISNUMERIC
--	
--	Prueft ob der Angegebene Parameter ein gueltiger Nummerischer Datentyp ist
--	Gueltige Nummerische Datentypen sind(INT, BIGINT, DECIMAL, SMALLINT, TINYINT, FLOAT, MONEY, SMALLMONEY, REAL, NUMERIC)
--	Gibt 1 zurueck wenn der Ausdruck ein Nummerischer Datentyp ist ansonsten wird 0 zurueckgegeben
--	
--	Parameter:			Irgendein Ausdruck
--	Rueckgabetyp:		INT

--	ISNUMERIC mit String
SELECT ISNUMERIC('Bin ich nummerisch?')

--	ISNUMERIC mit FLOAT
SELECT ISNUMERIC(17.1)

--	ISNUMERIC mit GETDATE()
SELECT ISNUMERIC(GETDATE())

--	ISNUMERIC mit INT
SELECT ISNUMERIC(2)

------------------------------------------------------------------------------

--							R O W C O U N T

--Gibt die Anzahl der Zeilen zurück, auf die sich die letzte Anweisung
-- ausgewirkt hat. Beträgt die Anzahl der Zeilen mehr als 2 Milliarden, 
-- verwenden Sie ROWCOUNT_BIG.
-- Hat einen Rückgabetypen des Typen int (Integer)
insert into jobs(job_desc, min_lvl, max_lvl) 
values
('Programmierer', 10 , 20)

select @@ROWCOUNT






--Username

       --Returns a database user name from a specified identification number.

       -- Gibt den Datenbank Usernamen einer bestimmten ID zurueck

       select USER_NAME(1) as Erster ,USER_NAME(2) as Zweiter,USER_NAME(3) as Dritter,USER_NAME(4) as Vierter,
                    USER_NAME(5) as Fuenfter,USER_NAME(6) as Sechster,USER_NAME(7) as Siebter 
       go

       select USER_NAME() as CurrentUser



	   --Is Null

       
       -- ISNULL ( check_expression , replacement_value )
       --***Argumente***
       --check_expression
       --Der Ausdruck, der auf NULL überprüft werden soll. check_expression kann einen beliebigen Typ aufweisen.
       --replacement_value
       --Der Ausdruck, der zurückgegeben werden soll, wenn check_expression NULL ist. 
       --Der Datentyp von replacement_value muss implizit in den Typ von check_expresssion konvertiert werden können.
             
             --**** INT Beispiel
             select * from titles
             where price is null 

             select ISNULL(price,0.00)
              from titles
                    -- Auf gut deutsch , dort wo der Gesuchte Datensatz NULL ist , wird stattdessen 0.00 angezeigt 

             -- Text Beispiel
       select * from titles 
       where notes is null

       select isnull(notes,'Nicht Vorhanden') from titles
                    -- Hier wird statt NUll  der Beispieltext('Nicht Vorhanden') angezeigt 

                    -- Also wie man sieht verhindert man damit die Ausgabe von NULL ,
                    -- ändert jedoch NICHT dauerhaft den Datzensatz
                    -- Der Datentyp der Ersatzanzeige muss dem des gesuchten Datensatzes entsprechen 
                    -- Der typ des gesuchten muss mit der gewuenschten ersatzausgabe übereinstimmen



-- N E U
use pubs
go
select * from titles

select * from authors
select * from stores
select * from publishers
select * from employee

-- Geben Sie den Verlag (Alias), die Stadt (Alias) und den Namen der Angestellten (Alias) in diesem Verlag aus, 
-- sortiert nach Verlag
select pub_name as Verlag, city as Stadt, fname + ' ' + lname as Angestellter
from publishers as p
	inner join employee as e on  p.pub_id = e.pub_id
order by verlag 

-- Welche Bücher (Titel und Preis in einem Feld Buchinfo) sind vom Verlag New Moon Books?
select title + ' ' + convert(varchar(10), price) as Buchinfo, pub_name, 
cast(price as varchar(10))
from titles as t 
	inner join publishers as p on t.pub_id = p.pub_id
where pub_name = 'New Moon Books'

-- Welche Angestellten haben einen Job als Editor und was verdienen sie (job_lvl), sortiert nach Verdienst.
select *
from jobs as j 
	inner join employee as e on j.job_id = e.job_id
where job_desc = 'Editor'
order by job_lvl

-- Welche Angestellten wurden im Jahr 1991 eingestellt (hire_date), sortiert nach Einstellungsdatum?
select fname + ' ' + lname as Angestellter, hire_date as Einstellungsdatum
from employee 
where year(hire_date) = 1991
-- where hire_date between '1.1.1991' and '31.12.1991 23:59'
order by Einstellungsdatum

-- Datumsabfragen problematisch mit datetime oder smalldatetime, da die Uhrzeit hier 
-- berücksichtigt werden muss
where bestelldatum = '3.9.2015'
where bestelldatum between '3.9.2015' and '3.9.2015 23:59'

-- Rechnen Sie zu jedem Buch der Kategorie mod_cook 2 Euro dazu und geben Sie den Titel des Buches, den alten und den 
-- neuen Preis mit Aliasname aus.
select title as Titel, price as Altpreis, price + 2 as Neupreis
from titles 
where [type] = 'mod_cook'

-- Welche Angestellten arbeiten in einem Verlag in CA oder DC und verdienen mehr als 40 (job_lvl)?
select fname, lname, job_lvl, state as Staat
from employee 
	join publishers on employee.pub_id = publishers.pub_id
where job_lvl > 40 and (state ='DC' or state ='CA') 
order by job_lvl
-- ACHTUNG: bei Kombination von AND und OR in der where Klausel ist es wichtig welches Eregbnis 
-- gefragt ist. In dem obigen Bsp wollen wir alle Angestellten aus CA und DC, die mehr als 40
-- verdienen. Daher muss der Ausdruck um den OR Operator in Klammern gesetzt werden, 
-- damit er zuerst ausgewertet wird und DANN ERST wird mit AND das job_lvl dazugerechnet.
-- einfacher: die OR Verknüpfung durch den in Operator ersetzen
where job_lvl < 40 and state in ('CA','DC')


-- Welche Bücher (Titel und Preis in einem Feld Buch) werden im Verlag Algodata Infosystems verlegt?
select title + ' ' + convert(varchar(10),price) as Buch
from titles as t
	join publishers as p on t.pub_id = p.pub_id
where pub_name = 'Algodata Infosystems'

-- Welche Bücher aus der Kategorie business oder mod_cook kosten mehr als 10?
select title, type, price
from titles 
where (type = 'mod_cook' or type = 'business') and price > 10

-- Geben Sie alle Autoren (Vorname und Nachname in einem Feld Autor) aus, die keinen Vertrag haben
-- und aus CA kommen, sortiert nach Nachname absteigend. 
select au_fname + ' ' + au_lname as Autor, contract, state
from authors 
where contract = 0 and state = 'CA'
order by au_lname desc

-- Welcher Autor hat welches Buch geschrieben, sortiert nach Buch? (join über 3 Tabellen - recherchieren Sie)
select au_fname + ' ' + au_lname as Autor, title as Titel
from titles as t 
	join titleauthor as ta on t.title_id = ta.title_id
	join authors as a on a.au_id = ta.au_id
order by titel
	 
-- Welche Bücher hat Ann Dull geschrieben?
select au_fname + ' ' + au_lname as Autor, title as Titel
from titles as t 
	join titleauthor as ta on t.title_id = ta.title_id
	join authors as a on a.au_id = ta.au_id
where au_fname = 'ann' and au_lname = 'Dull'

-- aus welchen Städten haben wir Autoren? (recherchieren Sie)
select city, count(*) as Anzahl
from authors 
group by city

select distinct city as Stadt
from authors

-- geben Sie das teuerste Buch aus. (recherchieren Sie)
select top 5 title, price
from titles
order by price desc

-- mit ex equo Ergebnissen
select top 5 with ties title, price
from titles
order by price desc

select *
from titles
where price = (select max(price) as Teuerstes
				from titles)


-- N E U
use pubs
go
select * from titles
select * from employee
select * from authors
select * from stores
select * from publishers

-- Geben Sie alle Bücher der Kategorie business aus
select *
from titles
where type = 'business'

-- Geben Sie von den Büchern die Felder title, types und price aus
-- sortiert nach Preis absteigend
select title, [type], price
from titles
order by price desc

-- Geben Sie Vorname und Nachname der Employees aus in einem Feld
-- mit dem Alias Angestellter, dazu das Feld hire_Date
-- sortiert nach hire_date
select fname + ' ' + lname as Angestellte, 
hire_date
from employee
order by hire_date

-- Geben Sie den Titel, Type und Preis der Bücher aus der Kategorie 
-- mod_cook und business aus, sortiert nach Kategorie und 
-- innerhalb von Kategorie nach Preis absteigend
select title, type, price
from titles
-- where type = 'business' or type = 'mod_cook'
where type in ('business','mod_cook')
order by type, price desc

-- Welche Bücher haben keine notes? (recherchieren Sie)
select *
from titles
where notes is null

-- Geben Sie den Gesamtnamen der Angestellten (in einem Feld Namen) aus, 
-- die zwischen 30 und 60 job_lvl verdienen, sortiert nach job_lvl
select fname + ' ' + lname as Namen, job_lvl
from employee
where job_lvl between 30 and 60
order by job_lvl

-- Welche Autoren kommen aus dem state CA oder UT?
-- Geben Sie den Gesamtnamen und mit -- getrennt in einem Feld mit Alias Info aus
select au_lname + ' -- ' + au_fname as Info, [state]
from authors
where [state] in ('CA','UT')

USE pubs
GO

--	UPPER
--	
--	Gibt den uebergebenen Character in Grossbuchstaben zurueck
--	
--	Parameter:			[Character or Binary data | kann eine Konstante, Variable oder Spalte sein]
--						muss implizit in ein Varcharacter konvertierbar sein ansonsten muss man es mittels CAST oder CONVERT explizit konvertieren
--	Rueckgabetyp:		VARCHAR or NVARCHAR

--	ohne UPPER
SELECT 'google ist gross'

--	mit UPPER
SELECT UPPER('google') + ' ist ' + UPPER('gross')

--	mit UPPER auf Wert einer Spalte
SELECT UPPER(fname) FROM employee

--	LOWER
--	
--	Gibt den uebergebenen Character in Kleinbuchstaben zurueck
--	
--	Parameter:			[Character or Binary data | kann eine Konstante, Variable oder Spalte sein]
--						muss implizit in ein Varcharacter konvertierbar sein ansonsten muss man es mittels CAST oder CONVERT explizit konvertieren
--	Rueckgabetyp:		VARCHAR or NVARCHAR

--	ohne LOWER
SELECT 'MICROSOFT ist BOESE'

--	mit LOWER
SELECT LOWER('MICROSOFT') + ' ist ' + LOWER('BOESE')

--	mit LOWER auf Wert einer Spalte
SELECT LOWER(fname) FROM employee

--	GETUTCDATE
--	
--	Gibt das aktuelle Datum und Uhrzeit des DB-Systems vobei der Zeitzonenoffset nicht einbezogen wird(UTC-Zeit)
--	Heisst wenn man in der Zeitzone +2 ist werden die 2 Stunden abgezogen. Aus 12 Uhr wird 10
--	
--	Parameter:			----
--	Rueckgabetyp:		DATETIME

--	Aktuelle Zeit (mit Zeitzonenoffset)
SELECT GETDATE()

--	Aktuelle Zeit in UTC-Zeit (ohne Zeitzonenoffset)
SELECT GETUTCDATE()

--	POWER
--	
--	Gibt die Potenz von den angegebenen Parametern zurueck
--
--	Parameter:			[FLOAT | muss implizit in ein FLOAT konvertierbar sein], [Numerischer Datenyp | mit Ausnahme von BIT]
--	RueckgabeTyp		FLOAT

--	POWER mit INTEGER und INTEGER
SELECT POWER(5,3)

--	POWER mit FLOAT und FLOAT
SELECT POWER(13.96, 3.4)

--	mit POWER auf Wert einer Spalte
SELECT POWER(price, 2) FROM titles

--	ISNUMERIC
--	
--	Prueft ob der Angegebene Parameter ein gueltiger Nummerischer Datentyp ist
--	Gueltige Nummerische Datentypen sind(INT, BIGINT, DECIMAL, SMALLINT, TINYINT, FLOAT, MONEY, SMALLMONEY, REAL, NUMERIC)
--	Gibt 1 zurueck wenn der Ausdruck ein Nummerischer Datentyp ist ansonsten wird 0 zurueckgegeben
--	
--	Parameter:			Irgendein Ausdruck
--	Rueckgabetyp:		INT

--	ISNUMERIC mit String
SELECT ISNUMERIC('Bin ich nummerisch')

--	ISNUMERIC mit FLOAT
SELECT ISNUMERIC(17.1)

--	ISNUMERIC mit GETDATE()
SELECT ISNUMERIC(GETDATE())

--	ISNUMERIC mit INT
SELECT ISNUMERIC(2)




/*
create database dbTeilnehmer
go
*/
-- drop database dbTeilnehmer

use dbteilnehmer
go

create table tblKlasse
(	IDKlasse char(4) primary key,
	RaumNr varchar(10) not null,
	Sitzplätze tinyint
)

create table tblTrainer
(	IDTrainer int identity(1,1) primary key,
	TRVN varchar(30) not null, 
	TRNN varchar(50) not null, 
	Gehalt float check(gehalt >= 0)
)

create table tblGerät
(	IDGerät int identity(1,1) primary key,
	FKKlasse char(4) references tblKlasse,
	Gerät varchar(100), 
	Inventar varchar(20)
)

create table tblTrainerKlasse
(	FKKlasse char(4) not null references tblKlasse, 
	FKTrainer int not null references tblTrainer, 
	primary key(FKKlasse,FKTrainer)
)

create table tblTeilnehmer
(	IDTeilnehmer int identity(1,1) primary key,
	FKKlasse char(4) references tblKlasse,
	Vorname varchar(30) not null,
	Nachname varchar(100) not null,
	PLZ char(4), 
	Ort varchar(100),
	Geburtsdatum date check (geburtsdatum < getdate()),
	Einstieg date default getdate(),
	IstMAAB bit default 0,
	SVNR char(10),
	Geschlecht char(1) check (geschlecht in ('m','w')),
	Grösse tinyint
)

/*
drop table tblTrainerKlasse
drop table tblTeilnehmer
drop table tblGerät
drop table tblKlasse
drop table tblTrainer
*/
