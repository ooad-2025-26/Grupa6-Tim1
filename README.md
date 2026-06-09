# CampusEats1

CampusEats1 je ASP.NET Core MVC aplikacija za studentsku menzu. Aplikacija omogucava pregled menija i obroka, registraciju/prijavu korisnika, rezervacije obroka, QR evidenciju, kurirski pregled dostava i administrativni pregled sistema.

## Tehnologije

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server LocalDB / SQL Server
- ASP.NET Core Identity
- Repository + Service + Controller struktura
- Authentication i role-based Authorization

## Lokalno pokretanje

Otvoriti folder:

```text
CampusEats1
```

Pokrenuti aplikaciju:

```powershell
dotnet run --launch-profile http
```

Lokalni link:

```text
http://localhost:5119
```

Prije prvog pokretanja baze pokrenuti migracije:

```powershell
dotnet ef database update
```

## Baza podataka

Development connection string je lokalni:

```text
Server=(localdb)\mssqllocaldb;Database=CampusEatsDb;Trusted_Connection=True;MultipleActiveResultSets=true
```

Za deployment je potrebno postaviti produkcijski `DefaultConnection` connection string na hosting servisu.

## Korisnici i pristup

Registracija je funkcionalna kroz aplikaciju. Korisnik pri registraciji dobija odabranu ulogu.

Uloge u aplikaciji:

- Student
- RadnikMenze
- Kurir
- Administrator

Vidljivost navigacije i pristup stranicama zavise od prijave i uloge korisnika.

## Deployment

Za deployment aplikacije potrebno je:

1. Objaviti aplikaciju na ASP.NET hosting, npr. Azure App Service.
2. Postaviti produkcijski SQL Server connection string pod nazivom `DefaultConnection`.
3. Izvrsiti migracije baze na produkcijskoj bazi.
4. Dodati javni link aplikacije u ovaj README.

Deployment link:

```text
Nije jos postavljen.
```
