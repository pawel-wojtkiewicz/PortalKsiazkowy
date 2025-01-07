# Portal Książkowy

Portal Książkowy to aplikacja ASP.NET Core, która umożliwia użytkownikom przeglądanie książek, ocenianie ich oraz otrzymywanie rekomendacji na podstawie swoich ocen.

## Funkcjonalności

- Przeglądanie dostępnych książek
- Dodawanie nowych książek oraz usuwanie książek (dostępne dla administratorów)
- Ocena książek
- Rekomendacje książek na podstawie ocenionych książek

## Wymagania

- .NET 8.0 SDK lub wyższy
- MS SQL Server

## Jak uruchomić projekt?

1.	Pobierz pliki projektu z repozytorium / sklonuj repozytorium
2.	Wypakuj pliki w dowolnym miejscu (w przypadku pobierania plików bezpośrednio z repozytorium)
3.	Uruchom Visual Studio oraz Zaimportuj projekt za pomocą opcji otwórz projekt lub rozwiązanie, wskaż w projekcie plik PortalKsiazkowy.sln LUB w projekcie dwukrotnie kliknij plik PortalKsiazkowy.sln.
4.	Uruchom projekt 
5.	W przypadku problemów z uruchomieniem projektu: </br>
Otwórz Narzędzia -> Menedżer pakietów NuGet -> Konsola menedżera pakietów </br>
Uruchom polecenia: </br>
dotnet tool install --global dotnet-ef </br>
dotnet ef migrations add InitialCreate </br>
dotnet ef database update </br>
6.	Podczas uruchamiania projektu dodawane jest kilka przykładowych książek oraz tworzone jest domyślne konto administratora (login: admin hasło: Admin@123)

