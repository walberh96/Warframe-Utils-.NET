# Quick Start - 5 Minute Setup

**Get the Warframe Utils .NET application running in just 5 minutes.**

For detailed instructions, see **GUIDE.MD**

---

## Prerequisites Check (1 minute)

```powershell
# Check .NET is installed
dotnet --version
# Should show: 8.0.x or higher
```

**Need to install?**
- Download: https://dotnet.microsoft.com/download
- Visual Studio 2022: https://visualstudio.microsoft.com/

---

## Setup Steps (4 minutes)

### Step 1: Open Project (1 minute)
1. Open **Visual Studio 2022**
2. **File** → **Open** → **Folder**
3. Select the `"Warframe Utils .NET"` folder

### Step 2: Create Database (1 minute)
1. **Tools** → **NuGet Package Manager** → **Package Manager Console**
2. Make sure **Default Project** = "Warframe Utils .NET"
3. Run:
```powershell
dotnet ef database update
```
✅ Database created!

### Step 3: Run Application (1 minute)
- Press **F5** (or click green play button)
- Wait 30 seconds for compilation
- Browser opens automatically

### Step 4: Test It Works (1 minute)
1. Search for: **"energy nexus"**
2. See price table appear ✅
3. Toggle dark mode with 🌓 button ✅
4. Try another search ✅

---

## Success! 🎉

**What you now have:**
- ✅ Application running locally
- ✅ Database configured
- ✅ All APIs connected
- ✅ Ready to develop

---

## What's Next?

### To Learn More
- Read **README.MD** (15 mins) - Architecture overview
- Read **GUIDE.MD** (25 mins) - Detailed setup & development

### To Make Changes
1. Edit code in Visual Studio
2. Press **F5** to reload
3. Changes appear immediately

### Common Tasks

**Run Tests**:
```powershell
dotnet test
```

**Build Release**:
```powershell
dotnet build -c Release
```

**Deploy**:
See GUIDE.MD → Deployment section

---

## Troubleshooting

**Port Already in Use?**
Edit `Properties/launchSettings.json`, change `"sslPort": 7281` to `7282`

**Database Error?**
```powershell
dotnet ef database drop
dotnet ef database update
```

**Packages Not Found?**
```powershell
dotnet restore
```

**More Help?**
See GUIDE.MD → Troubleshooting section

---

## File Structure
```
Warframe Utils .NET/
├── Program.cs              ← Application startup
├── Controllers/            ← Handle requests
├── Services/               ← API integration
├── Models/                 ← Data structures
├── Views/                  ← HTML pages
├── Data/                   ← Database
├── wwwroot/                ← CSS, JavaScript
├── README.MD               ← Architecture
├── GUIDE.MD                ← Setup guide
└── appsettings.json        ← Configuration
```

---

## Key Files to Know

| File | Purpose |
|------|---------|
| `Program.cs` | App startup, DI setup |
| `Controllers/HomeController.cs` | Main page logic |
| `Services/*.cs` | API integration |
| `Views/Home/Index.cshtml` | Search page HTML |
| `wwwroot/css/site.css` | Styling |
| `appsettings.json` | Configuration |

---

## Command Reference

```powershell
# Restore packages
dotnet restore

# Create/update database
dotnet ef database update

# Run application
dotnet run

# Run with F5 in Visual Studio (preferred)

# Build for release
dotnet build -c Release

# Run tests
dotnet test

# Clean build
dotnet clean
```

---

## Feature Overview

### 🔍 Search Mods
- Type mod name
- See autocomplete suggestions
- Click to view prices

### 💰 View Prices
- See all current sell orders
- Prices sorted (lowest first)
- Copy player name for trading
- See player online status

### 📊 Game Status
- Void Trader (Baroo) - Is he available?
- Plains of Eidolon - Day or Night?
- Orb Vallis - Warm or Cold?
- Deimos - Fass or Vome?

### 🌓 Dark Mode
- Toggle with 🌓 button
- Remembers your preference
- Smooth transitions

---

## API Endpoints Used

| API | URL | Purpose |
|-----|-----|---------|
| Warframe Market | api.warframe.market/v1 | Mod prices |
| Warframe Status | api.warframestat.us/pc | Game status |

No API keys needed - both are public APIs!

---

## Keyboard Shortcuts

| Key | Action |
|-----|--------|
| **F5** | Start/Debug application |
| **Ctrl+Shift+R** | Hard refresh browser |
| **Ctrl+K, Ctrl+C** | Comment code (VS) |
| **Ctrl+K, Ctrl+U** | Uncomment code (VS) |
| **Ctrl+F5** | Run without debugging |

---

## Port Information

**Default Ports**:
- HTTPS: `https://localhost:7281`
- HTTP: `http://localhost:5172`

**Change Port**:
Edit `Properties/launchSettings.json`

---

## Database Info

**Location**: `%localappdata%\MSSQLLocalDB`

**Name**: `aspnet-Warframe_Utils_.NET-bb3721c6-f575-4895-b2de-4d44d62bdebe`

**Tables**:
- AspNetUsers (user accounts)
- AspNetRoles (user roles)
- AspNetUserRoles (user assignments)
- ... and more identity tables

---

## Environment Variables

**Not currently needed** - all config in `appsettings.json`

For production, add:
```
ASPNETCORE_ENVIRONMENT=Production
ConnectionString=your-production-server
```

---

## Help & Documentation

📖 **README.MD** - Full technical documentation
📘 **GUIDE.MD** - Detailed setup & development guide
📕 **IMPROVEMENTS.MD** - What was improved
✅ **IMPROVEMENTS_CHECKLIST.MD** - Complete change list

---

## Support

**Issues?** Check GUIDE.MD → Troubleshooting

**Want to learn more?** Read the comments in:
- `Program.cs` - Application structure
- `Controllers/HomeController.cs` - Request handling
- `Services/*.cs` - API integration

All files are heavily documented!

---

## You're Ready! 🚀

Application is running and you know the basics.

### Next: Pick One
- 🎓 Learn: Read README.MD and GUIDE.MD
- 🔧 Develop: Make code changes
- 🧪 Test: Try the test scenarios in GUIDE.MD
- 📚 Study: Read the code comments

---

**Enjoy building with Warframe Utils .NET!**
