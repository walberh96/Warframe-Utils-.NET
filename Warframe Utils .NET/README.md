# Warframe Utils .NET

A web-based utility application built with ASP.NET Core 8.0 designed to help Warframe players access real-time market prices, game status information, and mod trading data. The application integrates with two external APIs: Warframe Market API and Warframe Status API.

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Technology Stack](#technology-stack)
3. [Architecture & Structure](#architecture--structure)
4. [Core Components](#core-components)
   - [Controllers](#controllers)
   - [Services](#services)
   - [Models & DTOs](#models--dtos)
   - [Views](#views)
   - [Data Layer](#data-layer)
5. [Detailed Class Documentation](#detailed-class-documentation)
6. [Configuration](#configuration)
7. [User Interface Features](#user-interface-features)
8. [Setup & Deployment](#setup--deployment)

---

## Project Overview

**Purpose**: This application provides Warframe players with a convenient interface to:
- Search for mods and items from the Warframe market
- View live trading orders for specific items
- Check real-time game server status (plains cycles, void trader, arbitration, etc.)
- Display mod details including rarity, trading tax, and wiki links

**Target Users**: Warframe players interested in market trading information

**Key Features**:
- Real-time mod price comparison
- Live player trading orders with player status and activity
- Game status monitoring (Void Trader, Cetus Cycles, Orb Vallis, Deimos)
- Search with autocomplete suggestions
- User identity/authentication system (via ASP.NET Identity)
- Dark mode support for comfortable viewing
- Full Warframe Market v2 API integration with proper response mapping

---

## Technology Stack

| Component | Version | Purpose |
|-----------|---------|---------|
| **.NET Framework** | 8.0 | Core framework for ASP.NET Core web application |
| **ASP.NET Core** | 8.0 | Web application framework |
| **Entity Framework Core** | 8.0.15 | ORM for database operations |
| **SQL Server** | LocalDB | Database for user identity and app data |
| **ASP.NET Identity** | 8.0.15 | User authentication and authorization |
| **Bootstrap 5** | (via CDN) | Frontend UI framework |
| **jQuery** | (via CDN) | JavaScript DOM manipulation |
| **System.Text.Json** | Built-in | JSON serialization for API responses |
| **HttpClient** | Built-in | External API communication (connection pooling) |
| **Warframe Market API** | v2 | Live market and trading order data |
| **Warframe Status API** | Current | Game status and event cycles |

---

## Architecture & Structure

The application follows the **MVC (Model-View-Controller)** pattern with additional service layers:

```
Warframe Utils .NET/
├── Controllers/                 # HTTP request handlers
│   ├── HomeController.cs        # Main UI controller
│   └── API/ApiTestController.cs # API endpoint testing
├── Services/                    # Business logic & external API calls
│   ├── WarframeMarketApiService.cs    # Warframe Market API integration
│   └── WarframeStatApiService.cs      # Warframe Status API integration
├── Models/                      # Data models
│   ├── DTOS/                    # Data Transfer Objects for API responses
│   │   ├── ModDetailResponse.cs
│   │   ├── ModsResponse.cs
│   │   ├── OrdersResponse.cs
│   │   └── WarframeStatus.cs
│   ├── ViewModels/              # Models passed to views
│   │   ├── WarframeHomeViewModel.cs
│   │   └── ErrorViewModel.cs
│   └── ErrorViewModel.cs
├── Views/                       # Razor templates
│   ├── Home/
│   │   └── Index.cshtml         # Main search & results page
│   ├── Shared/                  # Shared layout templates
│   │   ├── _Layout.cshtml
│   │   ├── _ValidationScriptsPartial.cshtml
│   │   └── Error.cshtml
│   └── _ViewImports.cshtml
├── Data/                        # Database & ORM configuration
│   ├── ApplicationDbContext.cs  # Entity Framework context
│   └── Migrations/              # Database schema migrations
├── Properties/                  # Application configuration
├── wwwroot/                     # Static files (CSS, JS, images)
├── Program.cs                   # Application entry point & DI setup
├── appsettings.json             # Configuration settings
└── Warframe Utils .NET.csproj   # Project file with dependencies
```

---

## Core Components

### Controllers

#### [HomeController.cs](Controllers/HomeController.cs)

**Location**: `Controllers/HomeController.cs`

**Purpose**: Handles all user-facing HTTP requests for the main application UI

**Class**: `HomeController : Controller`

**Dependencies Injected**:
- `ILogger<HomeController> _logger` - Logging service
- `WarframeStatApiService warframeStatApiService` - Service for fetching Warframe server status
- `WarframeMarketApiService warframeMarketApiService` - Service for fetching market mod data

**Methods**:

##### `IndexAsync(string? modName) : Task<IActionResult>`
- **Purpose**: Main page handler that displays game status and mod search results
- **Parameters**:
  - `modName` (optional): Name of the mod to search for
- **Process Flow**:
  1. Calls `warframeStatApiService.GetWarframeStatusAsync()` to fetch current game status (Baroo, cycles, etc.)
  2. Calls `warframeMarketApiService.GetAllModsAsync()` to fetch all available mods
  3. If `modName` is provided:
     - Searches for the mod in the mods list (case-insensitive)
     - If found, fetches trading orders via `GetAllOrdersAsync(modFound.UrlName)`
     - Fetches mod details via `GetItemAsync(modFound.UrlName)`
     - Catches and logs any errors, setting orders and details to null
  4. Constructs a `WarframeHomeViewModel` with all gathered data
  5. Returns the Index view with the viewmodel
- **Error Handling**: Wraps API calls in try-catch to gracefully handle failures
- **Returns**: `IActionResult` (Razor view)

##### `Error() : IActionResult`
- **Purpose**: Handles error page display
- **Attributes**: `[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]` - Disables caching
- **Returns**: Error view with `ErrorViewModel` containing request trace ID

#### [ApiTestController.cs](Controllers/API/ApiTestController.cs)

**Location**: `Controllers/API/ApiTestController.cs`

**Purpose**: Test API endpoint (for debugging/testing purposes)

**Class**: `ApiTestController : Controller`

**Attributes**:
- `[ApiController]` - Marks as API controller
- `[Route("api/[controller]")]` - Routes to `/api/apitest`

**Methods**:

##### `Get() : IActionResult`
- **Purpose**: Test endpoint that returns a simple JSON response
- **Returns**: JSON object `{ message: "Api Test" }`
- **HTTP Method**: GET

---

### Services

#### [WarframeMarketApiService.cs](Services/WarframeMarketApiService.cs)

**Location**: `Services/WarframeMarketApiService.cs`

**Purpose**: Encapsulates all HTTP calls to the Warframe Market API (https://api.warframe.market/v1/)

**Class**: `WarframeMarketApiService`

**Field**:
- `readonly HttpClient _httpClient` - HTTP client for making API requests
- `private const string apiUrl = "https://api.warframe.market/v1/"` - Base URL for API

**Constructor**:
```csharp
public WarframeMarketApiService(HttpClient httpClient)
```
- Dependency-injected HttpClient (configured in Program.cs)

**Methods**:

##### `GetAllModsAsync() : Task<ModsResponse>`
- **Purpose**: Fetches all available mods from the Warframe Market
- **Endpoint**: `GET https://api.warframe.market/v1/items`
- **Response Type**: `ModsResponse` containing list of all mods with basic info
- **Error Handling**:
  - Throws exception if HTTP request fails
  - Throws exception if JSON deserialization fails
- **Notes**: Logs mod count to console for debugging

##### `GetAllOrdersAsync(string itemId) : Task<OrdersResponse>`
- **Purpose**: Fetches all trading orders for a specific item
- **Parameters**:
  - `itemId`: URL-safe name of the item (e.g., "energy_nexus")
- **Endpoint**: `GET https://api.warframe.market/v2/orders/item/{itemId}`
- **Response Type**: `OrdersResponse` containing list of active orders with pricing and seller info
- **Response Structure**: 
  ```json
  {
    "apiVersion": "0.22.7",
    "data": [ ... array of Order objects ... ],
    "error": null
  }
  ```
- **Key Implementation Detail**: The v2 API returns orders in the `"data"` property, not `"orders"`. The `OrdersResponse` DTO uses `[JsonPropertyName("data")]` to properly map this response.
- **Error Handling**: Throws exception if HTTP request or deserialization fails; returns empty list on null response
- **Use Case**: When user searches for a specific mod, shows current market prices and available sellers
- **Filtering**: The view filters to show only "sell" orders from players with "ingame" status to show active traders

##### `GetItemAsync(string itemId) : Task<ModDetailResponse>`
- **Purpose**: Fetches detailed information about a specific item
- **Parameters**:
  - `itemId`: URL-safe name of the item
- **Endpoint**: `GET https://api.warframe.market/v2/item/{itemId}`
- **Response Type**: `ModDetailResponse` containing item set details (rarity, trading tax, tags, description, wiki link, etc.)
- **Response Structure**: Uses payload wrapper pattern with nested Item object
- **Error Handling**: Throws exception if request or deserialization fails; returns null on error
- **Use Case**: Displays mod description, trading tax, rarity, and other metadata on the search results page

#### [WarframeStatApiService.cs](Services/WarframeStatApiService.cs)

**Location**: `Services/WarframeStatApiService.cs`

**Purpose**: Encapsulates HTTP calls to the Warframe Status API (https://api.warframestat.us/pc)

**Class**: `WarframeStatApiService`

**Fields**:
- `private readonly HttpClient _httpClient` - HTTP client for API requests
- `private const string apiUrl = "https://api.warframestat.us/pc"` - API base URL for PC platform

**Constructor**:
```csharp
public WarframeStatApiService(HttpClient httpClient)
```
- Dependency-injected HttpClient

**Methods**:

##### `GetWarframeStatusAsync() : Task<WarframeStatus>`
- **Purpose**: Fetches current game server status including cycle times and special events
- **Endpoint**: `GET https://api.warframestat.us/pc`
- **Response Type**: `WarframeStatus` containing:
  - Void Trader (Baroo) active status
  - Arbitration mission node and type
  - Cetus Cycle (Plains of Eidolon) day/night status
  - Orb Vallis cycle (Venus) warm/cold status
  - Cambion Cycle (Deimos) state (Fass/Vome)
- **Error Handling**: Throws exception if request or deserialization fails
- **Use Case**: Displays game status bar at top of home page with emojis

---

### Models & DTOs

**Purpose**: Data Transfer Objects define the structure of API responses for JSON deserialization

#### [ModsResponse.cs](Models/DTOS/ModsResponse.cs)

**Location**: `Models/DTOS/ModsResponse.cs`

**Classes**:

##### `ModsResponse`
- **Purpose**: Top-level container for mod list API response
- **Properties**:
  - `Payload : ModsPayload` - Contains the actual mod data

##### `ModsResponse.ModsPayload`
- **Purpose**: Container for the list of mods
- **Properties**:
  - `Mods : List<Mod>` - List of all available mods

##### `ModsResponse.Mod`
- **Purpose**: Individual mod data from search
- **Properties**:
  - `ItemName : string` - Display name of the mod (e.g., "Energy Nexus")
  - `Thumb : string` - URL to thumbnail image
  - `UrlName : string` - URL-safe identifier (e.g., "energy_nexus") used for lookups
- **Usage**: Used in autocomplete suggestions and search results

---

#### [OrdersResponse.cs](Models/DTOS/OrdersResponse.cs)

**Location**: `Models/DTOS/OrdersResponse.cs`

**Classes**:

##### `OrdersResponse`
- **Purpose**: Top-level container for trading orders API response
- **Properties**:
  - `ApiVersion : string` - API version string (e.g., "0.22.7")
  - `Orders : List<Order>?` - List of trading orders for an item
  - `Error : object?` - Error information (null if successful)

**Important Note**: The v2 API response uses `"data"` property name (mapped to `Orders`), not `"orders"`. The DTO uses `[JsonPropertyName("data")]` attribute to properly deserialize responses from the endpoint.

##### `OrdersResponse.Order`
- **Purpose**: Individual trading order from a player
- **Properties**:
  - `Platinum : int` - Price per unit in Platinum (in-game currency)
  - `Quantity : int` - Number of items available
  - `Type : string` - "sell" or "buy" order type
  - `User : User` - Information about the seller/buyer
  - `Visible : bool` - Whether order is active and visible
  - `Rank : int` - Rank of the mod being sold (0-10 typically)
  - `PerTrade : int` - Items included per trade transaction
  - `CreatedAt : DateTime` - When the order was created
  - `UpdatedAt : DateTime` - When the order was last updated
- **UI Display**: Shown in table format, filtered by sell orders only from players with "ingame" status

##### `OrdersResponse.User`
- **Purpose**: Information about the player making the order
- **Properties**:
  - `Id : string` - Unique user identifier
  - `IngameName : string` - Warframe username (displayed in table)
  - `Slug : string` - URL-safe username
  - `Status : string` - "ingame", "offline", or "away" (filtered to show only "ingame")
  - `Avatar : string` - Profile avatar image URL
  - `Reputation : int` - Player's market reputation score
  - `Platform : string` - PC/PS4/Xbox/Switch
  - `Activity : DateTime` - Last activity timestamp
- **Usage**: Username can be copied to clipboard via UI button for direct messaging

---

#### [ModDetailResponse.cs](Models/DTOS/ModDetailResponse.cs)

**Location**: `Models/DTOS/ModDetailResponse.cs`

**Classes**:

##### `ModDetailResponse`
- **Purpose**: Top-level container for item detail API response
- **Properties**:
  - `Payload : ModDetailPayload` - Contains item information

##### `ModDetailResponse.ModDetailPayload`
- **Purpose**: Container for the item
- **Properties**:
  - `Item : ModDetailItem` - The actual item details

##### `ModDetailResponse.ModDetailItem`
- **Purpose**: Contains all variants/ranks of an item
- **Properties**:
  - `ItemsInSet : List<ModDetailSetItem>` - List of item variants (different ranks/versions)

##### `ModDetailResponse.ModDetailSetItem`
- **Purpose**: Individual item variant with metadata
- **Properties**:
  - `UrlName : string` - URL-safe identifier
  - `Rarity : string` - Item rarity (Common, Uncommon, Rare, Legendary, etc.)
  - `Thumb : string` - Thumbnail image URL
  - `TradingTax : int` - NPC trading cost in platinum
  - `SubIcon : string` - Secondary icon URL
  - `ModMaxRank : int` - Maximum rank/level for this mod
  - `Icon : string` - Main item icon URL
  - `Tags : List<string>` - Category tags (e.g., ["melee", "tenno"])
  - `En : ModDetailLocalization` - English language details

##### `ModDetailResponse.ModDetailLocalization`
- **Purpose**: English language text and links for the item
- **Properties**:
  - `ItemName : string` - Display name
  - `Description : string` - Item description/stats
  - `WikiLink : string` - Link to Warframe Wiki page
  - `Icon : string` - Full-size icon URL
  - `Thumb : string` - Thumbnail URL
  - `Drop : List<string>` - Where the item can be obtained (mission nodes, enemy types, etc.)
- **UI Display**: Description shown in mod detail card, wiki link clickable

---

#### [WarframeStatus.cs](Models/DTOS/WarframeStatus.cs)

**Location**: `Models/DTOS/WarframeStatus.cs`

**Class**: `WarframeStatus`

**Purpose**: Aggregates all game server status information

**Properties** (each contains status data for a specific game mechanic):
- `BarooData : VoidTrader` - Void Trader (Baroo) presence
- `ArbutrationData : Arbitration` - Current arbitration mission
- `CetusData : CetusCycle` - Plains of Eidolon time (day/night)
- `VenusData : VallisCycle` - Orb Vallis climate (warm/cold)
- `DeimosData : CambionCycle` - Cambion Drift state (Fass/Vome)

**Nested Classes**:

##### `VoidTrader`
- **Properties**:
  - `isActive : bool` - Whether Void Trader is currently available
- **UI Display**: Shows "⚡ Active" or "❌ Not Active"

##### `Arbitration`
- **Properties**:
  - `node : string` - Current mission node (e.g., "Corpus Gas City on Venus")
- **Current Usage**: Data structure exists but not displayed on UI

##### `CetusCycle`
- **Properties**:
  - `isDay : bool` - True = daytime, False = nighttime
- **UI Display**: Shows "☀️ Day" or "🌙 Night"

##### `VallisCycle`
- **Properties**:
  - `state : string` - "warm" or "cold"
- **UI Display**: Shows "🔥 Heat" or "❄️ Cold"

##### `CambionCycle`
- **Properties**:
  - `state : string` - "fass" or "vome" (day/night equivalent)
- **UI Display**: Shows "☀️ Fass" or "🌙 Vome"

---

#### [ErrorViewModel.cs](Models/ErrorViewModel.cs)

**Location**: `Models/ErrorViewModel.cs`

**Class**: `ErrorViewModel`

**Purpose**: Passes error information to error page views

**Properties**:
- `RequestId : string` - Unique request identifier for tracing
- `ShowRequestId : bool` (computed) - Returns true if RequestId is not empty

**Usage**: Displayed when an unhandled exception occurs

---

#### [WarframeHomeViewModel.cs](Models/ViewModels/WarframeHomeViewModel.cs)

**Location**: `Models/ViewModels/WarframeHomeViewModel.cs`

**Class**: `WarframeHomeViewModel`

**Purpose**: Aggregates all data needed for the home page view

**Properties**:
- `Status : WarframeStatus` - Game server status information
- `Mods : ModsResponse` - All available mods for autocomplete
- `ModFound : Mod` - The specific mod user searched for (null if no search)
- `Orders : OrdersResponse?` - Trading orders for the found mod (nullable)
- `ModDetail : ModDetailResponse?` - Detailed information about the found mod (nullable)

**Usage**: Single viewmodel passed to Index.cshtml view containing everything needed for rendering

---

### Views

#### [Index.cshtml](Views/Home/Index.cshtml)

**Location**: `Views/Home/Index.cshtml`

**Purpose**: Main user-facing page for mod search and results

**Model Type**: `WarframeHomeViewModel`

**Sections**:

##### 1. **Game Status Bar** (Lines 1-102)
- Displays current status of game cycles and events
- Shows emojis for visual indication:
  - Plains of Eidolon: ☀️ Day or 🌙 Night
  - Orb Vallis: 🔥 Heat or ❄️ Cold
  - Deimos: ☀️ Fass or 🌙 Vome
  - Void Trader: ⚡ Active or ❌ Not Active
- Handles null API responses gracefully with "❌ Api not Working"

##### 2. **Search Form** (Lines 104-110)
- Text input with id "searchBox" for mod name entry
- Search button to submit
- Dropdown suggestions list (id "suggestions")
- Form id "searchForm" for JavaScript handling

##### 3. **Mod Detail Card** (Lines 112-135)
- Displays when `Model.ModFound != null`
- Shows mod icon/image from Warframe Market
- Displays mod name
- Shows mod description from detail response
- Displays trading tax cost
- Includes link to official Warframe Wiki page for the mod
- Card layout with image on left, details on right

##### 4. **Orders Table** (Lines 136-162)
- Table showing all trading orders filtered by:
  - Order type must be "sell" (not "buy")
  - Seller must be online ("ingame" status)
  - Order must be visible
- Sorted by: mod rank (ascending), then price (ascending)
- Columns:
  - **Username**: Player name with "Copy" button (copies `/w username` format)
  - **Status**: Badge showing "Ingame", "Offline", or "Away"
  - **Price**: Platinum cost
  - **Quantity**: Number of items available
  - **Rank**: Mod rank level
- Shows "No orders found" message if no matching orders

##### 5. **JavaScript Functionality** (Lines 164-232)

###### `copyUsername(username) : void`
- Copies `/w username` to clipboard for easy whisper format
- Used by Copy buttons in the orders table

###### **Search Autocomplete System** (Lines 235-232)
- **Variables**:
  - `mods`: Array of all mod names for suggestions
  - `input`: Reference to search input element
  - `suggestions`: Reference to dropdown list element
  - `form`: Reference to search form
  - `filtered`: Array of matching mods
  - `activeIndex`: Currently highlighted suggestion index

- **Functions**:
  - `renderSuggestions()`: Renders filtered mod list with highlighting
  
- **Event Listeners**:
  - **Input event**: 
    - Gets search query (lowercase)
    - Filters mods by substring match
    - Limits to 8 suggestions
    - Re-renders suggestions list
  
  - **Keydown event**:
    - ArrowDown: Move highlight down (circular)
    - ArrowUp: Move highlight up (circular)
    - Enter: Select highlighted mod or submit if none highlighted
  
  - **Document click event**: 
    - Closes suggestions dropdown when clicking outside
  
  - **Form submit event**:
    - Validates that entered mod name exists in mod list
    - Auto-selects first match if user typed partial name
    - Prevents submission if mod name invalid

---

### Data Layer

#### [ApplicationDbContext.cs](Data/ApplicationDbContext.cs)

**Location**: `Data/ApplicationDbContext.cs`

**Class**: `ApplicationDbContext : IdentityDbContext`

**Purpose**: Entity Framework Core context for database operations and user identity

**Inheritance**: `IdentityDbContext` (from Microsoft.AspNetCore.Identity.EntityFrameworkCore)

**Constructor**:
```csharp
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
```
- Takes DbContextOptions passed via dependency injection

**Current DbSets**: None explicitly defined (inherits Identity tables from IdentityDbContext)

**Inherits Database Tables**:
- AspNetUsers - User accounts
- AspNetRoles - User roles
- AspNetUserRoles - User to role mappings
- AspNetUserClaims - User claims
- AspNetRoleClaims - Role claims
- AspNetUserTokens - Authentication tokens
- AspNetUserLogins - External login mappings

**Configuration**: 
- Configured in Program.cs with SQL Server provider
- Uses LocalDB by default
- Connection string from appsettings.json

---

## Detailed Class Documentation

### Program.cs (Entry Point)

**Location**: `Program.cs`

**Purpose**: Application startup and dependency injection configuration

**Execution Flow**:

1. **WebApplication Builder Setup**:
   - `builder = WebApplication.CreateBuilder(args)` - Creates application builder

2. **Service Registration**:
   - **Database**:
     - `AddDbContext<ApplicationDbContext>()` - Registers EF Core context with SQL Server
     - Connection string from configuration
   - **Identity**:
     - `AddDefaultIdentity<IdentityUser>()` - Registers ASP.NET Identity
     - Requires email confirmation for login
     - `AddEntityFrameworkStores<ApplicationDbContext>()` - Uses database for user storage
   - **MVC**:
     - `AddControllersWithViews()` - Registers MVC controllers and Razor views
   - **HTTP Clients** (Custom services):
     - `AddHttpClient<WarframeStatApiService>()` - HttpClient for Warframe Status API calls
     - `AddHttpClient<WarframeMarketApiService>()` - HttpClient for Warframe Market API calls

3. **Middleware Pipeline Configuration**:
   - **Development**:
     - `UseMigrationsEndPoint()` - Database migration UI in development
   - **Production**:
     - `UseExceptionHandler("/Home/Error")` - Centralized error handling
     - `UseHsts()` - HTTP Strict Transport Security
   - **All Environments**:
     - `UseHttpsRedirection()` - Enforce HTTPS
     - `UseStaticFiles()` - Serve static files (CSS, JS, images)
     - `UseRouting()` - Enable routing
     - `UseAuthorization()` - Enable authorization

4. **Route Configuration**:
   - Default MVC route: `{controller=Home}/{action=Index}/{id?}`
   - Maps Razor Pages (Identity pages)

5. **Application Run**:
   - `app.Run()` - Start listening for requests

---

## Configuration

### appsettings.json

**Location**: `appsettings.json`

**Settings**:

#### ConnectionStrings
```json
{
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-Warframe_Utils_.NET-bb3721c6-f575-4895-b2de-4d44d62bdebe;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
- **Database**: SQL Server LocalDB
- **Database Name**: `aspnet-Warframe_Utils_.NET-bb3721c6-f575-4895-b2de-4d44d62bdebe`
- **Authentication**: Windows integrated authentication

#### Logging
```json
{
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```
- Application logs at Information level
- ASP.NET Core framework logs at Warning level (reduces noise)

#### AllowedHosts
```json
{
  "AllowedHosts": "*"
}
```
- Accepts requests from any host (useful for development)

---

### Project Configuration (Warframe Utils .NET.csproj)

**Target Framework**: .NET 8.0

**Project Properties**:
- **Nullable Reference Types**: Enabled (strict null checking)
- **Implicit Usings**: Enabled (auto-includes common namespaces)
- **User Secrets ID**: Configured for storing sensitive development data

**NuGet Dependencies**:
| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | 8.0.15 | EF Core error diagnostics UI |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.15 | Identity with EF Core |
| Microsoft.AspNetCore.Identity.UI | 8.0.15 | Pre-built Identity UI pages |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.15 | SQL Server database provider |
| Microsoft.EntityFrameworkCore.Tools | 8.0.15 | EF Core CLI tools (migrations) |

---

## User Interface Features

### Home Page (Index.cshtml)

**Top Section - Game Status Dashboard**:
- Real-time display of Warframe game cycles and events
- Responsive layout with centered status indicators
- Color-coded badges and emojis for quick visual reference

**Search Feature**:
- Text input field with autocomplete
- Suggestions appear as user types (limited to 8 results)
- Keyboard navigation support (arrow keys, enter)
- Click outside to dismiss suggestions
- Validation prevents searching for non-existent mods

**Mod Detail Card** (when mod selected):
- High-resolution mod icon/image
- Mod name and description
- Trading tax information
- Clickable link to official Warframe Wiki
- Card layout optimizes for readability

**Orders Table** (when orders exist):
- Clean, striped table for easy reading
- Sorting by price and rank (lowest first)
- Copy button for quick whisper format
- Status badges with color coding (green=ingame, gray=offline)
- Pagination implied for large datasets (not implemented)
- "No orders" message when no matches

### Responsive Design
- Bootstrap 5 grid system for responsive layouts
- Mobile-friendly with proper spacing and sizing
- Proper text sizing for readability
- Adequate button/link sizes for touch interfaces

---

## Setup & Deployment

### Prerequisites
- **.NET SDK 8.0** or later
- **SQL Server LocalDB** (included with Visual Studio)
- **Visual Studio 2022** or VS Code with C# extension

### Local Development Setup

1. **Clone/Open Project**:
   ```bash
   cd "Warframe Utils .NET"
   ```

2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Apply Database Migrations**:
   ```bash
   dotnet ef database update
   ```
   - Creates LocalDB database
   - Applies Identity schema and any custom migrations

4. **Run Application**:
   ```bash
   dotnet run
   ```
   - Application starts on HTTPS port (typically https://localhost:7281)

### Database Migrations

**Migration Files** located in `Data/Migrations/`:
- `00000000000000_CreateIdentitySchema.cs` - Initial Identity tables
- `20250427232943_First Migration.cs` - First project migration (if any)
- `ApplicationDbContextModelSnapshot.cs` - Current database schema snapshot

**To Create New Migration**:
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Deployment Considerations

- **Database**: Replace LocalDB connection string with Azure SQL Database or production SQL Server
- **HTTPS**: Ensure SSL certificates configured for production
- **API Keys**: No API keys needed (public APIs used)
- **Logging**: Configure appropriate logging level for production
- **Error Handling**: Current error messages expose some details, consider more generic messages for prod

---

## File Structure Summary

| File/Folder | Type | Purpose |
|------------|------|---------|
| `Program.cs` | C# | Application startup and DI configuration |
| `Controllers/HomeController.cs` | C# | Main UI request handler |
| `Controllers/API/ApiTestController.cs` | C# | Test API endpoint |
| `Services/WarframeMarketApiService.cs` | C# | Warframe Market API integration |
| `Services/WarframeStatApiService.cs` | C# | Warframe Status API integration |
| `Models/DTOS/*.cs` | C# | API response data models |
| `Models/ViewModels/*.cs` | C# | View-specific data models |
| `Data/ApplicationDbContext.cs` | C# | Entity Framework Core context |
| `Views/Home/Index.cshtml` | Razor | Main search and results page |
| `Views/Shared/_Layout.cshtml` | Razor | Master layout template |
| `wwwroot/` | Static | CSS, JavaScript, Bootstrap, jQuery |
| `Properties/launchSettings.json` | JSON | Launch configuration |
| `appsettings.json` | JSON | Application configuration |
| `Warframe Utils .NET.csproj` | XML | Project and dependencies |

---

## Key Design Decisions

1. **External API Services**: Created separate service classes for each API to keep concerns separated and make testing/modifications easier
2. **ViewModels**: Used specific viewmodel class instead of passing multiple models, providing cleaner view code
3. **DTOs with JSON Mapping**: Used `[JsonPropertyName]` attributes to handle snake_case API responses
4. **Client-side Search**: Implemented autocomplete in JavaScript rather than server-side to provide faster UX
5. **Error Handling**: Exceptions caught at controller level to prevent crashes, logged for debugging
6. **Bootstrap UI**: Used popular Bootstrap framework for professional-looking responsive design
7. **Async/Await**: All API calls use async/await pattern for non-blocking I/O

---

## Future Enhancement Opportunities

- Implement pagination for large order lists
- Add filtering/sorting options to orders table
- Cache API responses to reduce external requests
- Add user favorites for frequently searched mods
- Implement price history charts
- Add user accounts for saved preferences
- Support other game platforms (PS4, Xbox, Switch)
- Real-time WebSocket updates for price changes
- Mobile app version

---

**Last Updated**: 2025-04-27 (Project Creation Date)
**Framework**: ASP.NET Core 8.0
**Namespace**: `Warframe_Utils_.NET`
