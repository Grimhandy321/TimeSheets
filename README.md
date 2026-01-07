# TimeSheets
Projekt na databaze

## Předpoklady
.net 8
MSSQL


# Instalace 
```bash
  git clone https://github.com/yourusername/TimeSheets.git
  cd TimeSheets
```
2. Obnovte závislosti:

```bash
dotnet restore
```

3. Nakonfigurujte databázi v `appsettings.json`:

Nejdříve se připojte na MSSQL, vytvořte databázi a poté nastavte připojení v konfiguraci:

```json
{
  "ConnectionStrings": {
    "Default": "Your ConnectionString here;"
  },
}
```

4. Spusťte aplikaci:

```bash
dotnet run
```
5. Otevřete http://localhost:xxxx/swagger/  a zobrazte dokumentaci API.
6. Spusťte endpoint /api/setup pro inicializaci databáze.
