# 🎯 Warframe Utils - Developer Quick Reference

Quick reference guide for common development tasks.

## 🚀 Quick Start Commands

### Backend (.NET)
```bash
cd "Warframe Utils .NET"

# Development
dotnet run                          # Start server
dotnet watch run                    # Start with hot reload
dotnet build                        # Build project
dotnet test                         # Run tests

# Database
dotnet ef migrations add MigrationName    # Create migration
dotnet ef database update                 # Apply migrations
dotnet ef database drop                   # Drop database

# Publishing
dotnet publish -c Release          # Build for production
```

### Frontend (Next.js)
```bash
cd warframe-frontend

# Development
npm run dev                        # Start dev server (port 3000)
npm run dev -- -p 3001            # Start on different port
npm run build                      # Build for production
npm start                          # Run production build
npm run lint                       # Check for errors

# Clean
rm -rf .next node_modules         # Clean build cache
npm install                        # Reinstall dependencies
```

---

## 📁 Key File Locations

### Backend
```
Warframe Utils .NET/
├── Program.cs                    # App configuration, DI, middleware
├── appsettings.json             # Connection strings, settings
├── Controllers/API/             # API endpoints
│   ├── GameStatusController.cs  # Game status API
│   ├── SearchController.cs      # Market search API
│   ├── AlertController.cs       # Price alerts API
│   ├── AuthController.cs        # Authentication API
│   ├── UserController.cs       # User info API
│   └── ApiTestController.cs     # Test endpoint
├── Services/                    # Business logic
│   ├── WarframeMarketApiService.cs
│   ├── WarframeStatApiService.cs
│   ├── PriceAlertCheckService.cs
│   └── DevEmailSender.cs
└── Models/                      # Data models & DTOs
```

### Frontend
```
warframe-frontend/
├── src/app/
│   ├── page.tsx                 # Home page
│   ├── layout.tsx               # Root layout
│   └── globals.css              # Global styles
├── src/components/
│   ├── navbar.tsx               # Top navigation
│   ├── game-status-section.tsx  # Game status cards
│   ├── search-section.tsx        # Market search
│   ├── NotificationBell.tsx      # Alert & notification management
│   ├── theme-provider.tsx        # Theme context
│   └── ui/                       # Reusable UI components
├── next.config.mjs              # Next.js config (API proxy)
└── tailwind.config.ts           # Tailwind theme
```

---

## 🔌 API Endpoints Reference

### Game Status
```
GET /api/GameStatus
Response: { cetusCycle, voidTrader, vallisCycle }
```

### Market Search
```
GET /api/Search?modName=serration
Response: { modDetails, orders }

GET /api/Search/items
Response: [{ id, url_name, item_name }]
```

### Price Alerts (Auth Required)
```
GET    /api/Alert                           # List user's alerts
POST   /api/Alert                           # Create alert
PUT    /api/Alert/{id}                      # Update alert price
DELETE /api/Alert/{id}                      # Delete alert
GET    /api/Alert/{id}                      # Get specific alert
GET    /api/Alert/notifications/unread      # Get unread notifications
POST   /api/Alert/notifications/{id}/read   # Mark notification as read
```

### User & Authentication
```
GET    /api/User/me          # Get current user info (Auth Required)
GET    /api/User/check       # Check if user is authenticated
POST   /api/Auth/login       # Login
POST   /api/Auth/register    # Register
POST   /api/Auth/logout      # Logout
```

---

## 🎨 UI Component Usage

### Import Components
```typescript
import { Button } from '@/components/ui/button';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { useToast } from '@/hooks/use-toast';
```

### Button Examples
```typescript
<Button>Default</Button>
<Button variant="destructive">Delete</Button>
<Button variant="outline">Outline</Button>
<Button variant="ghost">Ghost</Button>
<Button size="sm">Small</Button>
<Button size="lg">Large</Button>
```

### Card Example
```typescript
<Card>
  <CardHeader>
    <CardTitle>Title</CardTitle>
    <CardDescription>Description</CardDescription>
  </CardHeader>
  <CardContent>
    Content here
  </CardContent>
</Card>
```

### Toast Notifications
```typescript
const { toast } = useToast();

toast({
  title: "Success",
  description: "Operation completed successfully",
});

toast({
  title: "Error",
  description: "Something went wrong",
  variant: "destructive",
});
```

---

## 🎨 Styling Reference

### Tailwind Classes

**Layout:**
```
flex, grid, container, mx-auto, px-4, py-8, space-y-4, gap-4
```

**Colors (Custom theme):**
```
bg-primary, text-primary-foreground
bg-secondary, text-secondary-foreground
bg-destructive, text-destructive-foreground
bg-card, text-card-foreground
```

**Warframe Theme Colors:**
```
text-blue-400, text-purple-400, text-pink-400
border-blue-500/20, bg-gradient-to-r from-blue-400 to-purple-400
```

**Effects:**
```
hover:opacity-80, transition-all, animate-pulse
backdrop-blur-sm, shadow-lg, rounded-lg
```

### Custom Classes
```css
/* In globals.css */
.warframe-gradient {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.warframe-card {
  @apply backdrop-blur-sm bg-card/50 border-border/50;
}
```

---

## 🔧 Configuration

### Backend Ports
Edit `Warframe Utils .NET/Properties/launchSettings.json`:
```json
"applicationUrl": "https://localhost:5001;http://localhost:5000"
```

### CORS Origins
Edit `Warframe Utils .NET/Program.cs`:
```csharp
policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
```

### Database Connection
Edit `Warframe Utils .NET/appsettings.json`:
```json
"DefaultConnection": "Host=localhost;Database=warframe_utils;Username=postgres;Password=yourpass"
```

### API Proxy (Frontend)
Edit `warframe-frontend/next.config.mjs`:
```javascript
destination: 'http://localhost:5000/api/:path*'
```

---

## 📊 Database Commands

### PostgreSQL
```bash
# Connect
psql -U postgres -d warframe_utils

# List tables
\dt

# View data
SELECT * FROM "PriceAlerts";
SELECT * FROM "AspNetUsers";

# Count records
SELECT COUNT(*) FROM "PriceAlerts";

# Exit
\q
```

### Entity Framework
```bash
# Create migration
dotnet ef migrations add AddNewFeature

# Apply migrations
dotnet ef database update

# Revert to specific migration
dotnet ef database update PreviousMigrationName

# Remove last migration (if not applied)
dotnet ef migrations remove

# Generate SQL script
dotnet ef migrations script
```

---

## 🐛 Debugging Tips

### Frontend Debugging
```typescript
// Console logging
console.log('Data:', data);
console.table(array);
console.error('Error:', error);

// React DevTools
// Install browser extension
// Inspect component props and state

// Network tab
// Check API calls in browser DevTools (F12)
```

### Backend Debugging
```csharp
// Logging
_logger.LogInformation("Processing request for {ItemName}", itemName);
_logger.LogWarning("Price alert threshold reached");
_logger.LogError(ex, "Error fetching data");

// Breakpoints in Visual Studio/VS Code
// F5 to start debugging, F9 to set breakpoint

// Watch expressions
// Add variables to watch window
```

### Common Issues

**CORS Error:**
- Check backend CORS policy includes frontend URL
- Verify credentials: true in frontend fetch calls

**404 API Error:**
- Check API endpoint URL
- Verify controller route attribute
- Check backend is running

**Database Connection:**
- Verify PostgreSQL is running
- Check connection string
- Test with psql command

---

## 🚀 Performance Tips

### Frontend
```typescript
// Use React.memo for expensive components
const ExpensiveComponent = React.memo(({ data }) => {
  // ...
});

// Debounce search inputs
import { useDebouncedCallback } from 'use-debounce';

// Lazy load heavy components
const HeavyComponent = lazy(() => import('./HeavyComponent'));
```

### Backend
```csharp
// Use async/await properly
public async Task<IActionResult> GetData()
{
    var data = await _service.FetchAsync();
    return Ok(data);
}

// Cache expensive operations
services.AddMemoryCache();

// Use select to limit data
var items = await _context.Items
    .Select(i => new { i.Id, i.Name })
    .ToListAsync();
```

---

## 📦 Adding New Features

### Add New Backend API Endpoint

1. **Create Controller:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Hello" });
    }
}
```

2. **Test in browser:**
```
http://localhost:5000/api/My
```

### Add New Frontend Component

1. **Create component:**
```typescript
// src/components/my-component.tsx
export function MyComponent() {
  return <div>My Component</div>;
}
```

2. **Import and use:**
```typescript
import { MyComponent } from '@/components/my-component';

<MyComponent />
```

### Add New Database Model

1. **Create model class:**
```csharp
public class MyEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

2. **Add to DbContext:**
```csharp
public DbSet<MyEntity> MyEntities { get; set; }
```

3. **Create migration:**
```bash
dotnet ef migrations add AddMyEntity
dotnet ef database update
```

---

## 📚 Useful Links

### Documentation
- [Next.js Docs](https://nextjs.org/docs)
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Tailwind CSS](https://tailwindcss.com/docs)
- [shadcn/ui](https://ui.shadcn.com/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)

### APIs
- [Warframe Market API](https://docs.warframe.market)
- [Warframe Status](https://docs.warframestat.us/)

---

## 🔑 Keyboard Shortcuts

### VS Code
- `Ctrl/Cmd + P` - Quick file open
- `Ctrl/Cmd + Shift + F` - Search in files
- `F12` - Go to definition
- `Alt + Click` - Multi-cursor
- `Ctrl/Cmd + /` - Toggle comment

### Browser DevTools
- `F12` - Open DevTools
- `Ctrl/Cmd + Shift + C` - Inspect element
- `Ctrl/Cmd + R` - Refresh page
- `Ctrl/Cmd + Shift + R` - Hard refresh

---

## 💡 Pro Tips

1. **Use TypeScript** - It catches errors before runtime
2. **Test API endpoints** - Use Postman or browser DevTools
3. **Check logs** - Backend terminal shows useful info
4. **Use Git** - Commit often, branch for features
5. **Read errors carefully** - They usually tell you what's wrong
6. **Keep dependencies updated** - But test after updating

---

**Happy coding! 🚀**
