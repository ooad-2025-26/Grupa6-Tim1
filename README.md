# CampusEats

## Studentska menza bez nepotrebnog cekanja

## Opis projekta

**CampusEats** je ASP.NET Core MVC web aplikacija za digitalno upravljanje studentskom menzom. Studentima omogucava pregled trenutnog menija, rezervaciju obroka, izbor licnog preuzimanja ili dostave i pracenje statusa narudzbe.

Radnici menze upravljaju obrocima, menijem, zalihama i statusima narudzbi. Kuriri preuzimaju dostavne narudzbe i evidentiraju isporuku, dok administrator upravlja korisnicima, ulogama i ostalim dijelovima sistema.

Cilj projekta je smanjiti guzve, ubrzati preuzimanje obroka i studentima omoguciti jednostavnije planiranje ishrane.

## Razvojni tim

- Ensar Basic
- Ali Fisic
- Faris Tarahija
- Belma Djulic

Projekat je razvijen u okviru predmeta **Objektno orijentisana analiza i dizajn (OOAD)** na Elektrotehnickom fakultetu Univerziteta u Sarajevu, akademske 2025/2026. godine.

## Deployment

[CampusEats aplikacija](https://ftarahija1-001-site1.mtempurl.com/)

## Testni racuni

| Uloga | Email | Lozinka |
|---|---|---|
| Administrator | `admin@campuseats.ba` | `admin123` |
| Student | `afisictech@gmail.com` | `tech123` |
| Radnik menze | `radnik@campuseats.ba` | `radnik123` |
| Kurir | `kurir@campuseats.ba` | `kurir123` |

Novi korisnici koji se registruju kroz aplikaciju automatski dobijaju ulogu **Student**. Samo administrator moze mijenjati uloge korisnika.

## Akteri sistema

- **Student** - pregleda meni, rezervise obrok, prati status i koristi QR kod.
- **Radnik menze** - upravlja ponudom i mijenja status narudzbe.
- **Kurir** - prihvata dostavu, skenira QR kod narudzbe i pokazuje QR kod studentu.
- **Administrator** - upravlja korisnicima, ulogama, obrocima i sistemskim podacima.

## Koristene tehnologije

- ASP.NET Core MVC i C#
- .NET 10
- Entity Framework Core
- Microsoft SQL Server
- ASP.NET Identity
- Razor Views, HTML i CSS
- Bootstrap
- JavaScript i kamera preglednika za QR skeniranje
- Git i GitHub

