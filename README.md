# OwnerTrack

**OwnerTrack** je aplikacija razvijena u .NET 8 koristeći C# i Entity Framework Core koja omogućava praćenje klijenata, vlasnika, direktora i ugovora firmi. Projekt je osmišljen za upravljanje poslovnim podacima, procjenu rizika i evidenciju vlasničkih struktura.

---

## Tehnologije

- **Backend:** .NET 8, C#  
- **ORM:** Entity Framework Core  
- **Baza podataka:** SQLite  
- **Projektna struktura:**  
  - `OwnerTrack.App` – aplikacijski interfejs i logika  
  - `OwnerTrack.Data` – entiteti i DbContext  
  - `OwnerTrack.Infrastructure` – repozitoriji, servisni sloj i pristup podacima  

---



## Funkcionalnosti

- Evidencija i upravljanje klijentima, vlasnicima, direktorima i ugovorima  
- Praćenje procjene rizika (PEP, UBO, gotovina, geografski)  
- Automatizovana veza između klijenata i njihovih entiteta  
- Podrška za SQLite bazu i lokalno čuvanje podataka  

---

