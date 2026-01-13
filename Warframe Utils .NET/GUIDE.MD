# Warframe Utils .NET - Complete Setup & Development Guide

**A beginner-friendly guide to setting up, running, testing, and developing the Warframe Utils .NET application.**

---

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Project Overview](#project-overview)
3. [Initial Setup](#initial-setup)
4. [Database Setup](#database-setup)
5. [Running the Application](#running-the-application)
6. [Testing the Application](#testing-the-application)
7. [Development Workflow](#development-workflow)
8. [Troubleshooting](#troubleshooting)
9. [Deployment](#deployment)

---

## Prerequisites

Before starting, ensure you have the following installed on your computer:

### Required Software

| Software | Version | Purpose | Download |
|----------|---------|---------|----------|
| **.NET SDK** | 8.0 or later | Core framework for the application | [Download](https://dotnet.microsoft.com/download) |
| **Visual Studio 2022** | Latest | IDE for development (Community is free) | [Download](https://visualstudio.microsoft.com/downloads/) |
| **SQL Server LocalDB** | Latest | Local database (included with Visual Studio) | Included with VS |
| **Git** | Latest | Version control (optional but recommended) | [Download](https://git-scm.com/) |

### Required Visual Studio Workloads

When installing Visual Studio, make sure to include:
- **ASP.NET and web development**
- **.NET desktop development**
- **Data storage and processing**

### System Requirements

- **OS**: Windows 10 or later (or Mac/Linux with appropriate SDKs)
- **RAM**: 8GB minimum (4GB if only running the app)
- **Disk Space**: 2GB minimum
- **Internet Connection**: Required (for NuGet packages and external APIs)

---

## Project Overview

### What Does This Application Do?

The Warframe Utils .NET application helps Warframe players find the best prices for mods in the game's trading market.

**Key Features**:
- 🔍 **Search Mods**: Find any mod/item from the Warframe Market
- 💰 **View Prices**: See all current selling prices from online players
- 📊 **Game Status**: Check real-time game events (Void Trader, day/night cycles)
- 🎨 **Dark Mode**: Toggle between light and dark themes
- 📱 **Responsive**: Works on desktop, tablet, and mobile

### Technology Stack

```
┌─────────────────────────────────────────┐
│     Frontend (Browser)                  │
│  - HTML5 / Razor Templates (cshtml)    │
│  - Bootstrap 5 (UI Framework)          │
│  - JavaScript (Autocomplete)           │
└─────────────────────────────────────────┘
              ↓ HTTP/HTTPS
┌─────────────────────────────────────────┐
│     Backend (ASP.NET Core)              │
│  - Controllers (Handle Requests)       │
│  - Services (API Integration)          │
│  - Entity Framework (Database)         │
└─────────────────────────────────────────┘
         ↙                    ↘
    Database          External APIs
    (LocalDB)      - Warframe Market
    (SQL Server)   - Warframe Status
```

---

## Initial Setup

### Step 1: Clone or Extract the Project

If you received the project as a ZIP file:

```powershell
# Extract the ZIP file to a location on your computer
# For example: C:\Projects\Warframe-Utils-.NET
```

If using Git:

```bash
git clone <repository-url>
cd "Warframe Utils .NET"
```

### Step 2: Open the Project in Visual Studio

1. Open **Visual Studio 2022**
2. Click **File** → **Open** → **Folder**
3. Navigate to and select the `"Warframe Utils .NET"` folder
4. Wait for Visual Studio to load the project (this may take a minute)

### Step 3: Verify .NET Installation

Open **PowerShell** or **Command Prompt** and run:

```powershell
dotnet --version
```

Expected output: `8.0.x` or higher

If you get an error, restart Visual Studio or your terminal.

### Step 4: Restore NuGet Packages

Visual Studio usually does this automatically, but you can manually restore packages:

```powershell
# Navigate to the project folder
cd "path\to\Warframe Utils .NET"

# Restore NuGet packages
dotnet restore
```

This downloads all required libraries listed in the `.csproj` file.

**What gets installed?**
- ASP.NET Core framework
- Entity Framework Core (database ORM)
- Identity (authentication)
- JSON serialization libraries

---

## Database Setup

### Understanding LocalDB

**LocalDB** is a lightweight SQL Server instance perfect for development. It:
- Runs locally on your machine
- Requires no installation (included with Visual Studio)
- Stores data in files under `%localappdata%\`
- Is perfect for development, not for production

### Step 1: Create the Database

The application uses **Entity Framework Core Migrations** to create the database automatically.

#### Option A: Using Visual Studio Package Manager Console (Recommended)

1. In Visual Studio, go to **Tools** → **NuGet Package Manager** → **Package Manager Console**
2. Make sure the **Default Project** is set to `"Warframe Utils .NET"`
3. Run the command:

```powershell
dotnet ef database update
```

**What happens?**
- Creates a new LocalDB database
- Runs all migrations (creates tables)
- Sets up Identity tables for user authentication
- The database name appears in `appsettings.json`

#### Option B: Using Command Line

```powershell
# Navigate to project folder
cd "path\to\Warframe Utils .NET"

# Run migrations
dotnet ef database update
```

#### Option C: Using Visual Studio's Built-in Tools

1. Go to **Tools** → **SQL Server** → **New Query**
2. Copy-paste the connection string from `appsettings.json`
3. Click **Connect**

### Step 2: Verify the Database Was Created

To verify the database exists and is working:

1. Open **SQL Server Object Explorer** in Visual Studio
   - **View** → **SQL Server Object Explorer**
2. Expand **SQL Server** → **(localdb)\mssqllocaldb**
3. Look for database named:
   ```
   aspnet-Warframe_Utils_.NET-bb3721c6-f575-4895-b2de-4d44d62bdebe
   ```
4. If you see it, the database is ready!

### Step 3: Understanding the Database

**Current Tables** (inherited from ASP.NET Identity):

| Table | Purpose |
|-------|---------|
| **AspNetUsers** | Stores user accounts |
| **AspNetRoles** | Stores user roles (Admin, User, etc.) |
| **AspNetUserRoles** | Links users to roles |
| **AspNetUserClaims** | Stores user permissions |
| **AspNetUserTokens** | Stores authentication tokens |
| **AspNetUserLogins** | Stores external login info |

**Note**: The application doesn't currently store mod data or prices in the database. All data comes from external APIs.

### Resetting the Database

If you mess up the database or want to start fresh:

```powershell
# Delete the database
dotnet ef database drop

# Recreate it
dotnet ef database update
```

**Warning**: This deletes all user accounts! Use only in development.

---

## Running the Application

### Step 1: Start the Application

#### Option A: Using Visual Studio (Easiest)

1. In Visual Studio, press **F5** (or click the green **Play** button)
2. The application will compile and launch in your browser
3. Visual Studio shows a **Debug** toolbar at the top

#### Option B: Using Command Line

```powershell
cd "path\to\Warframe Utils .NET"
dotnet run
```

This will output something like:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7281
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to exit.
```

#### Option C: Using Visual Studio Built-in Server

1. Click the dropdown next to the play button
2. Select **IIS Express** or specific browser
3. Click the play button

### Step 2: Access the Application

Once running, open your web browser and go to:

```
https://localhost:7281
```

Or whichever port Visual Studio shows (usually 7281 or similar).

**Expected to see:**
- A page titled "Warframe Utils .NET"
- A search box for mod names
- Game status indicators at the top

### Stopping the Application

- **In Visual Studio**: Click the red **Stop** button (⏹️)
- **In Command Line**: Press **Ctrl + C**

### Common Port Issues

If port 7281 is already in use:

```powershell
# Find what's using the port
netstat -ano | findstr :7281

# Kill the process (replace PID with the number shown)
taskkill /PID <PID> /F
```

Or configure a different port in `Properties/launchSettings.json`:

```json
"sslPort": 7282  // Change to any available port
```

---

## Testing the Application

### Manual Testing (Without Code Changes)

#### Test 1: Home Page Loads

1. Start the application (see above)
2. Navigate to `https://localhost:7281`
3. **Expected**: Page loads without errors, status bar shows game cycles

**What's being tested?**
- Server is running
- Views render correctly
- API calls to Warframe Status API work

#### Test 2: Search for a Mod

1. In the search box, type: **"energy nexus"**
2. **Expected**: 
   - Suggestion appears as you type
   - Click it or press Enter
   - Mod detail card appears with icon and description
   - Table below shows current prices from players

**What's being tested?**
- Autocomplete functionality
- Warframe Market API is responding
- Data deserialization works
- View displays data correctly

#### Test 3: Search Non-existent Mod

1. In the search box, type: **"this mod does not exist xyz"**
2. Press Enter
3. **Expected**:
   - Either no results or "No orders found" message
   - Page doesn't crash
   - Game status still displays

**What's being tested?**
- Error handling works
- Page is resilient to API failures

#### Test 4: Dark Mode Toggle

1. Click the 🌓 (moon/sun) button in the top right
2. **Expected**: Page colors invert to dark theme
3. Click again
4. **Expected**: Page returns to light theme

**What's being tested?**
- JavaScript theme toggle works
- CSS dark mode styles apply correctly

#### Test 5: Mobile Responsiveness

1. Press **F12** to open Developer Tools
2. Click the device icon (usually top-left of DevTools)
3. Select **iPhone** or **Android** device
4. Resize the window or use the responsive view
5. **Expected**: Layout rearranges for mobile

**What's being tested?**
- Bootstrap responsive classes work
- Touch-friendly button sizes
- Mobile-friendly tables

#### Test 6: Table Sorting and Filtering

1. Search for a mod (e.g., "serration")
2. Look at the orders table
3. **Expected**:
   - Lowest price appears first
   - Only "ingame" sellers shown
   - Prices are sorted ascending

**What's being tested?**
- LINQ filtering works (OrderBy)
- Data display is accurate

### Automated Testing (Advanced)

Currently, the project doesn't have automated tests. Here's how to add them:

#### Create a Test Project

```powershell
# In the solution directory
dotnet new xunit -n "Warframe.Tests"
dotnet sln add "Warframe.Tests/Warframe.Tests.csproj"
cd "Warframe.Tests"
dotnet add reference "../Warframe Utils .NET/Warframe Utils .NET.csproj"
```

#### Example Unit Test

Create a file: `Services/WarframeMarketApiServiceTests.cs`

```csharp
using Xunit;
using Moq;
using Warframe_Utils_.NET.Services;

public class WarframeMarketApiServiceTests
{
    [Fact]
    public async Task GetAllModsAsync_ReturnsModsList()
    {
        // Arrange
        var httpClient = new HttpClient();
        var service = new WarframeMarketApiService(httpClient);

        // Act
        var result = await service.GetAllModsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Payload.Mods);
    }
}
```

Run tests:

```powershell
dotnet test
```

---

## Development Workflow

### Making Code Changes

#### 1. Edit a Service

**File**: `Services/WarframeMarketApiService.cs`

Example: Add caching to reduce API calls

```csharp
private readonly IMemoryCache _cache;

public WarframeMarketApiService(HttpClient httpClient, IMemoryCache cache)
{
    _httpClient = httpClient;
    _cache = cache;
}

public async Task<ModsResponse> GetAllModsAsync()
{
    const string cacheKey = "all_mods";
    
    // Check cache first
    if (_cache.TryGetValue(cacheKey, out ModsResponse? cached))
    {
        return cached!;
    }

    // Get from API if not cached
    var result = await FetchAllModsFromApiAsync();
    
    // Cache for 1 hour
    _cache.Set(cacheKey, result, TimeSpan.FromHours(1));
    
    return result;
}
```

Don't forget to register in `Program.cs`:

```csharp
builder.Services.AddMemoryCache();  // Add this line
builder.Services.AddHttpClient<WarframeMarketApiService>();
```

#### 2. Edit a View

**File**: `Views/Home/Index.cshtml`

Example: Add a new status indicator

```html
<!-- Add after the existing status items -->
<div class="col-auto text-center">
    <strong>New Status</strong><br>
    <span class="text-muted">
        @if (Model.Status?.NewData != null)
        {
            <text>✅ Active</text>
        }
        else
        {
            <text>❌ Inactive</text>
        }
    </span>
</div>
```

#### 3. Edit CSS

**File**: `wwwroot/css/site.css`

Example: Change button colors

```css
.btn-success {
    background-color: #00ff00;  /* Bright green */
}
```

Changes take effect immediately (hard refresh: Ctrl+Shift+R)

### Hot Reload (Live Updates)

In Visual Studio, when running with F5:
- **CSS changes** appear immediately (refresh browser)
- **HTML changes** appear immediately (refresh browser)
- **C# code changes** trigger auto-rebuild and page refresh

To force reload:
- Press **Ctrl + Shift + R** in browser (hard refresh)
- Or click Stop and F5 again

### Building the Project

#### Debug Build (Default)

Used during development, includes debugging info:

```powershell
dotnet build
```

#### Release Build

Optimized for performance, smaller size:

```powershell
dotnet build -c Release
```

File sizes:
- Debug: ~10 MB (larger, slower)
- Release: ~5 MB (smaller, faster)

---

## Troubleshooting

### Common Issues and Solutions

#### Issue 1: "Connection string 'DefaultConnection' not found"

**Symptom**: Error when starting the app

**Solution**:
1. Check `appsettings.json` exists in the root folder
2. Verify it contains:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=..."
   }
   ```
3. Run migrations: `dotnet ef database update`

#### Issue 2: "Port 7281 is already in use"

**Symptom**: "Address already in use" error on startup

**Solution A** (Kill the process):
```powershell
netstat -ano | findstr :7281
taskkill /PID <PID> /F
```

**Solution B** (Use a different port):

Edit `Properties/launchSettings.json`:
```json
"sslPort": 7282  // Change to any free port
```

#### Issue 3: Database Migration Fails

**Symptom**: "Migrations have not been applied" or migration errors

**Solution**:
```powershell
# Drop and recreate the database
dotnet ef database drop
dotnet ef database update
```

#### Issue 4: NuGet Package Restore Fails

**Symptom**: "Could not find any project files" or package errors

**Solution**:
```powershell
# Clear NuGet cache and restore
dotnet nuget locals all --clear
dotnet restore
```

#### Issue 5: "System.Net.Http.HttpRequestException" when Searching

**Symptom**: Error when clicking search, offline message

**Solution**:
1. Check internet connection
2. Verify external APIs are working:
   - Visit: https://api.warframe.market/v1/items (should return JSON)
   - Visit: https://api.warframestat.us/pc (should return JSON)
3. If they're down, the app can't function (external service issue)

#### Issue 6: CSS Not Updating / Changes Not Appearing

**Symptom**: Browser shows old CSS, changes don't appear

**Solution**:
1. Hard refresh browser: **Ctrl + Shift + R** (Windows) or **Cmd + Shift + R** (Mac)
2. Clear browser cache (Ctrl+Shift+Delete)
3. Restart the application (Stop F5, then F5 again)

#### Issue 7: Visual Studio Won't Open the Project

**Symptom**: "Project type not supported" or loading fails

**Solution**:
1. Make sure you're using **Visual Studio 2022** (not older versions)
2. Update Visual Studio (Help → Check for Updates)
3. Repair Visual Studio (Programs and Features → Visual Studio → Repair)

#### Issue 8: Port Already Bound (Linux/Mac)

```bash
# Find what's using port 7281
lsof -i :7281

# Kill it
kill -9 <PID>
```

---

## Deployment

### Preparing for Production

Before deploying to a real server, make these changes:

#### 1. Update Configuration

**File**: `appsettings.json`

Change database to production SQL Server (not LocalDB):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server.database.windows.net;Database=WarframeUtils;User Id=admin;Password=YourPasswordHere;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",  // Less logging in production
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

#### 2. Enable HTTPS

Ensure `program.cs` includes:

```csharp
app.UseHttpsRedirection();  // Forces HTTPS
app.UseHsts();              // HTTP Strict Transport Security
```

#### 3. Create a Release Build

```powershell
dotnet publish -c Release -o ./publish
```

This creates an optimized folder ready for deployment.

#### 4. Deploy to Azure (Free Option)

```powershell
# Install Azure CLI
# Then:
az webapp up --name WarframeUtilsApp --resource-group myResourceGroup --runtime "DOTNET:8.0"
```

#### 5. Deploy to Traditional Web Hosting

1. Build: `dotnet publish -c Release`
2. Copy the `/publish` folder to your hosting provider
3. Configure the hosting provider to run `dotnet Warframe\ Utils\ .NET.dll`

---

## Getting Help

### Documentation

- **Microsoft Docs**: https://docs.microsoft.com/en-us/dotnet/
- **ASP.NET Core**: https://docs.microsoft.com/en-us/aspnet/core/
- **Warframe Market API**: https://api.warframe.market/v1/
- **Warframe Status API**: https://api.warframestat.us/

### Community Resources

- **Stack Overflow**: Tag your questions with `asp.net-core`
- **Reddit**: r/dotnet, r/Warframe
- **GitHub Issues**: File issues in the project repository

### Contact

For questions about this project, check:
1. The README.md file for architecture overview
2. Code comments in the relevant files
3. Microsoft documentation for ASP.NET Core

---

## Checklist

Use this checklist when setting up a fresh system:

- [ ] .NET SDK 8.0+ installed
- [ ] Visual Studio 2022 installed with ASP.NET workload
- [ ] SQL Server LocalDB working
- [ ] Project extracted/cloned to a folder
- [ ] NuGet packages restored (`dotnet restore`)
- [ ] Database created (`dotnet ef database update`)
- [ ] Application starts without errors (`dotnet run` or F5)
- [ ] Home page loads in browser
- [ ] Can search for a mod successfully
- [ ] Dark mode toggle works
- [ ] Game status displays correctly

---

**Last Updated**: January 2026  
**Framework**: .NET 8.0 ASP.NET Core  
**Status**: Development Ready  

For detailed technical documentation, see **README.md**
