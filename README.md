# OwnerTrack

**OwnerTrack** je desktop aplikacija namijenjena finansijskim institucijama i kompanijama za upravljanje klijentima, vlasničkim strukturama i procjenom rizika. Omogućava potpunu evidenciju pravnih i fizičkih lica, njihovih vlasnika, direktora i ugovora, uz automatizovano praćenje važenja dokumenata i export izvještaja u PDF.

---

## Sadržaj

- [Tehnologije](#tehnologije)
- [Struktura projekta](#struktura-projekta)
- [Funkcionalnosti](#funkcionalnosti)
- [Pokretanje projekta](#pokretanje-projekta)
- [Import podataka iz Excela](#import-podataka-iz-excela)
- [Baza podataka i migracije](#baza-podataka-i-migracije)
- [PDF export](#pdf-export)
- [Revizijski trag](#revizijski-trag)

---

## Tehnologije

| Tehnologija | Verzija | Svrha |
|---|---|---|
| .NET | 8.0 | Framework |
| C# | 12 | Programski jezik |
| Windows Forms | net8.0-windows | UI |
| Entity Framework Core | latest | ORM |
| SQLite | — | Lokalna baza podataka |
| EPPlus | 5.0.0 | Čitanje Excel fajlova |
| QuestPDF | 2026.2.1 | Generisanje PDF izvještaja |

---


## Funkcionalnosti

### Upravljanje klijentima
- Dodavanje, izmjena i arhiviranje firmi
- Polja: naziv, ID broj (JIB), adresa, djelatnost, vrsta klijenta, veličina firme, datumi osnivanja i uspostave odnosa, email, telefon, napomena
- Filtriranje tabele po **pretrazi** (naziv/ID), **djelatnosti** i **veličini firme**
- Soft-delete — arhivirani klijenti ostaju u bazi radi revizijskog traga

### Upravljanje vlasnicima (UBO)
- Evidencija stvarnih vlasnika po klijentu
- Praćenje postotka vlasništva, datuma važenja dokumenta, izvora podatka
- Upozorenja za istekle ili uskoro istekle dokumente

### Upravljanje direktorima
- Evidencija direktora/zastupnika po klijentu
- Podrška za vremenski ograničenu i trajnu valjanost mandata
- Upozorenja za istekle mandate

### Procjena rizika
- PEP rizik, UBO rizik, gotovinski rizik, geografski rizik
- Ukupna procjena i datum procjene
- Ovjera/CR status

### Upozorenja 🔔
- Badge na toolbaru prikazuje broj dokumenata koji ističu u narednih **60 dana**
- Crvena boja = već istekli dokumenti
- Narančasta boja = uskoro ističu
- Klik otvara detaljan pregled svih upozorenja

---


## Import podataka iz Excela

Aplikacija podržava masovni import klijenata iz Excel fajla.

| Kolona | Polje |
|--------|-------|
| A | Naziv firme |
| B | ID broj (JIB) |
| C | Adresa |
| D | Šifra djelatnosti |
| E | Naziv djelatnosti |
| F | Datum uspostave odnosa |
| G | Vrsta klijenta |
| H | Datum osnivanja |
| I | Veličina firme |
| J | PEP rizik |
| K | UBO rizik |
| L | Gotovinski rizik |
| M | Geografski rizik |
| N | Ukupna procjena |
| O | Datum procjene |
| P | Ovjera/CR |
| Q | Status ugovora |
| R | Datum ugovora |


### Reset i reimport
Dugme **Resetuj i reimportuj** briše sve postojeće podatke (uz pravljenje backupa baze) i pokreće novi import. Backup se automatski čuva pored originalne baze pod nazivom `ownertrack.db.backup_YYYYMMDD_HHmmss`.

---


## PDF export

### Izvještaj pojedinačne firme
Kliknite **📄 Sačuvaj kao PDF** nakon odabira firme iz tabele.

Izvještaj uključuje:
- Osnovne podatke firme
- Procjenu rizika (sa color-coding: DA = crveno, NE = zeleno)
- Status i detalje ugovora
- Tabelu vlasnika sa postocima vlasništva i datumima važenja
- Tabelu direktora sa tipom i datumom valjanosti mandata

### Export tabele klijenata
Kliknite **📋 Export tabele u PDF** za export svih trenutno **filtriranih** klijenata.

- Format: **A4 Landscape**
- Kolone: #, Naziv, ID broj, Djelatnost, Veličina, PEP, UBO, Procjena rizika, Status ugovora, Datum uspostave, Datum osnivanja, Status
- Redoslijed u PDF-u prati redoslijed u tabeli na ekranu
- Ako su aktivni filteri (djelatnost, veličina, pretraga) — exporta se samo filtrirano

---

## Revizijski trag

Sve akcije nad podacima automatski se bilježe u tabelu `AuditLogs`:

| Akcija | Kada se bilježi |
|--------|-----------------|
| `DODANO` | Novi klijent, vlasnik ili direktor |
| `IZMIJENJENO` | Izmjena podataka klijenta |
| `OBRISANO` | Arhiviranje klijenta, vlasnika ili direktora |

Svaki zapis sadrži: naziv tabele, ID entiteta, tip akcije, opis promjene i tačno vrijeme.

---

## Validacija

- **JIB validacija** — ID broj se provjerava prema algoritmu za bosanskohercegovačke identifikacione brojeve prije snimanja
- **Duplikat provjera** — naziv i ID broj moraju biti jedinstveni u sistemu
- **Obavezna polja** — naziv i ID broj su obavezni pri dodavanju klijenta
