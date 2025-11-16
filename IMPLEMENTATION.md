# Implementace Správy Zápasů

## Přehled implementovaných funkcí

Podle poskytnutého obrázku byla implementována kompletní funkcionalita pro správu volejbalových zápasů s následujícími funkcemi (F1-F10):

### Backend (ASP.NET Core Web API)

#### DAL (Data Access Layer)

**Nová rozhraní:**
- `IZapasDao` - CRUD operace pro zápasy + vyhledávání
- `ISezonaDao` - CRUD operace pro sezóny
- `ITymDao` - CRUD operace pro týmy + získání týmů podle sezóny
- `IHracDao` - CRUD operace pro hráče + získání hráčů podle týmu

**Nové repository:**
- `ZapasRepository` - implementace `IZapasDao`
- `SezonaRepository` - implementace `ISezonaDao`
- `TymRepository` - implementace `ITymDao`
- `HracRepository` - implementace `IHracDao`

#### BLL (Business Logic Layer)

**Rozšířené `IZapasService`:**
- `GetSeasonsAsync()` - F1: Načtení všech sezón
- `GetTeamsAsync()` - F2: Načtení všech týmů
- `GetTeamsBySeasonAsync(int idSezona)` - F2: Načtení týmů podle sezóny
- `FindMatchesAsync(ZapasFilterDto filter)` - F3: Vyhledání zápasů podle filtru
- `CreateMatchAsync(CreateZapasDto createDto)` - F4: Vytvoření nového zápasu
- `GetMatchDetailsAsync(int idZapas)` - F6: Načtení detailu zápasu
- `SaveMatchAsync(UpdateZapasDto updateDto)` - F7: Uložení změn zápasu
- `GetPlayersByTeamAsync(int idTym)` - F8: Načtení hráčů podle týmu
- `DeleteMatchAsync(int idZapas)` - F10: Smazání zápasu

**DTOs:**
- `ZapasDto` - DTO pro zápas s navigačními vlastnostmi
- `ZapasFilterDto` - DTO pro filtrování zápasů
- `CreateZapasDto` - DTO pro vytvoření zápasu
- `UpdateZapasDto` - DTO pro aktualizaci zápasu

#### Controllers

**Rozšířený `ZapasController` (route: `/api/zapas`):**
- `GET /seasons` - F1: Získání sezón
- `GET /teams?idSezona={id}` - F2: Získání týmů (volitelně podle sezóny)
- `POST /find` - F3: Vyhledání zápasů
- `POST /create` - F4: Vytvoření zápasu
- `GET /{idZapas}` - F6: Detail zápasu
- `PUT /save` - F7: Uložení zápasu
- `GET /players/{idTym}` - F8: Hráči podle týmu
- `POST /lineups` - F9: Uložení soupisk
- `DELETE /{idZapas}` - F10: Smazání zápasu

**Zachované legacy endpointy:**
- `POST /vloz-sestavu` - původní endpoint pro soupisky
- `POST /vloz-sestavu-tsql` - T-SQL verze

### Frontend (Angular)

#### Services

**`MatchService` (`src/app/services/match.service.ts`):**
- Služba pro komunikaci s API
- Metody pro všechny F1-F10 funkce
- TypeScript interfaces pro DTOs

#### Components

**`MatchManagementComponent` (`src/app/components/match-management/`):**

**Funkce podle obrázku:**

1. **Vyhledávací formulář:**
   - Datum zápasu (datepicker)
   - Sezona (dropdown s načtenými sezónami z DB) - F1
   - Domácí tým (dropdown s týmy) - F2
   - Hostující tým (dropdown s týmy) - F2
   - Tlačítko "Vyhledat zápas" - F3

2. **Výsledky vyhledávání:**
   - Seznam nalezených zápasů - F5 (FilteredMatches)
   - Pokud nenalezeno → "Žádné zápasy nebyly nalezeny" + tlačítko "Vytvořit zápas" - F4

3. **Detail vybraného zápasu:**
   - Editovatelné pole pro všechny údaje zápasu - F6
   - Datum, sezona, domácí tým, hostující tým, skóre
   - Tlačítko "Upravit zápas" / "Uložit zápas" - F7
   - Tlačítko "Smazat zápas" - F10

4. **Přidání sestav týmů:**
   - Zobrazení hráčů obou týmů - F8
   - Checkboxy pro: Hraje, Kapitán, Libero
   - Rozděleno na domácí a hostující tým
   - Tlačítko "Přidat soupisky" - F9

#### Routing

Upraveno `app.routes.ts`:
- Hlavní stránka (`/`) → `MatchManagementComponent`
- `/match-management` → `MatchManagementComponent`
- `/app-player-form` → `PlayerFormComponent` (zachováno)

## Struktura souborů

### Backend
```
api/api/
├── BLL/
│   ├── Interfaces/
│   │   └── IZapasService.cs (rozšířeno)
│   └── Services/
│       └── ZapasService.cs (rozšířeno)
├── Controllers/
│   └── ZapasController.cs (rozšířeno)
├── DAL/
│   ├── Interfaces/
│   │   ├── IZapasDao.cs (nové)
│   │   ├── ISezonaDao.cs (nové)
│   │   ├── ITymDao.cs (nové)
│   │   └── IHracDao.cs (nové)
│   └── Repositories/
│       ├── ZapasRepository.cs (nové)
│       ├── SezonaRepository.cs (nové)
│       ├── TymRepository.cs (nové)
│       └── HracRepository.cs (nové)
├── Models/
│   └── DTOs/
│       └── ZapasDtos.cs (nové)
└── Program.cs (aktualizováno - registrace nových DAO)
```

### Frontend
```
FE/src/app/
├── components/
│   └── match-management/
│       ├── match-management.component.ts (nové)
│       ├── match-management.component.html (nové)
│       └── match-management.component.scss (nové)
├── services/
│   └── match.service.ts (nové)
└── app.routes.ts (aktualizováno)
```

## Návod na použití

### 1. Spuštění API
```powershell
cd api/api
dotnet run
```

### 2. Spuštění Frontend
```powershell
cd FE
npm install
npm start
```

### 3. Použití aplikace

1. **Vyhledání/Vytvoření zápasu:**
   - Vyplňte filtry (datum, sezona, týmy)
   - Klikněte "Vyhledat zápas"
   - Pokud zápas neexistuje, klikněte "Vytvořit zápas"

2. **Editace zápasu:**
   - Vyberte zápas ze seznamu
   - Klikněte "Upravit zápas"
   - Upravte údaje
   - Klikněte "Uložit zápas"

3. **Přidání soupisk:**
   - Po výběru zápasu se načtou hráči obou týmů
   - Zaškrtněte checkboxy pro jednotlivé hráče
   - Klikněte "Přidat soupisky"

4. **Smazání zápasu:**
   - Vyberte zápas
   - Klikněte "Smazat zápas"
   - Potvrzení

## Poznámky k implementaci

- **Dependency Injection:** Všechny nové služby jsou zaregistrované v `Program.cs`
- **Validace:** Backend obsahuje validace pro kapitány, libera a počet hráčů
- **Error handling:** Všechny API endpointy mají try-catch s informativními chybovými hláškami
- **UI/UX:** Responsive design, hover efekty, barevné rozlišení stavů
- **Databázové schéma:** Implementace respektuje existující schéma z SQL složky

## API Dokumentace

### Endpointy

| Endpoint | Metoda | Popis | F# |
|----------|--------|-------|-----|
| `/api/zapas/seasons` | GET | Načtení sezón | F1 |
| `/api/zapas/teams` | GET | Načtení týmů | F2 |
| `/api/zapas/find` | POST | Vyhledání zápasů | F3 |
| `/api/zapas/create` | POST | Vytvoření zápasu | F4 |
| `/api/zapas/{id}` | GET | Detail zápasu | F6 |
| `/api/zapas/save` | PUT | Uložení zápasu | F7 |
| `/api/zapas/players/{idTym}` | GET | Hráči týmu | F8 |
| `/api/zapas/lineups` | POST | Uložení soupisk | F9 |
| `/api/zapas/{id}` | DELETE | Smazání zápasu | F10 |

## Databázové požadavky

Aplikace očekává následující tabulky (podle SQL schématu):
- `Zapas` - zápasy
- `Sezona` - sezóny
- `Tym` - týmy
- `Hrac` - hráči
- `SestavaZapasu` - soupisky zápasů
- `Tym_Sezona` - vazba týmů na sezóny
