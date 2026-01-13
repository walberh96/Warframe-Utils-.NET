# Price Alert System - Quick Start Guide

## ⚡ 5-Minute Setup

### Step 1: Install PostgreSQL (2 minutes)
1. Download from https://www.postgresql.org/download/
2. Run installer, choose password for `postgres` user
3. Default port is 5432 ✓

### Step 2: Create Database (1 minute)
```cmd
psql -U postgres
CREATE DATABASE warframe_utils;
\q
```

### Step 3: Update Connection String (1 minute)
Edit `appsettings.json`:
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=warframe_utils;Username=postgres;Password=YOUR_PASSWORD"
```
(Replace `YOUR_PASSWORD` with what you set during PostgreSQL installation)

### Step 4: Run Migrations (1 minute)
```bash
cd "Warframe Utils .NET"
dotnet ef database update
```

### Done! 🎉
```bash
dotnet run
```

Open http://localhost:5089 and register an account!

---

## First Time Using Alerts?

1. **Register/Login** - Create a new account or log in
2. **Navigate to Price Alerts** - Click "Price Alerts" in the menu
3. **Create Alert** - Click "New Alert" button
4. **Enter Item Details**:
   - Item Name: Type the exact name (e.g., "Serration", "Excalibur Prime")
   - Alert Price: Set your target price in platinum (e.g., "100")
5. **Submit** - Click "Create Alert"
6. **Monitor** - The system checks prices every 5 minutes
7. **Get Notified** - See notification when price drops to your threshold

---

## Example Items to Try

- **Serration** (common mod)
- **Excalibur Prime** (warframe set)
- **Energy Nexus** (rare mod)
- **Condition Overload** (popular mod)

---

## Dashboard Overview

### Active Alerts
Shows all alerts you're currently monitoring
- Current Price: Live market price
- Alert Price: Your target price
- Status: Active (waiting for price drop)
- Actions: Edit, Delete, or create more

### Triggered Alerts
Shows alerts where the price dropped to your threshold
- Notification appears at top
- Price is shown in green
- Click "Acknowledge" to dismiss

### Notifications
Red notifications at top of page:
- Shows which item triggered
- Displays the triggered price
- Click X to dismiss

---

## API for Developers

**Base URL**: `http://localhost:5089/api/alert`

### Create Alert
```bash
curl -X POST http://localhost:5089/api/alert \
  -H "Content-Type: application/json" \
  -d '{"itemName":"Serration","alertPrice":50}'
```

### Get All Alerts
```bash
curl http://localhost:5089/api/alert
```

### Get Unread Notifications
```bash
curl http://localhost:5089/api/alert/notifications/unread
```

All endpoints require authentication (login first).

---

## Troubleshooting

| Problem | Solution |
|---------|----------|
| "Connection refused" | PostgreSQL not running. Start PostgreSQL service |
| "Database not found" | Run `CREATE DATABASE warframe_utils;` |
| "Authentication failed" | Check password in connection string matches PostgreSQL password |
| "No price found" | Item name must match exactly. Try full name from Warframe Market |
| Alerts not updating | Check if app is still running. Background service only works while app runs |

---

## Important Notes

- ✅ Only logged-in users can create alerts
- ✅ Alerts are checked every 5 minutes (background service)
- ✅ Prices update from Warframe Market API in real-time
- ✅ Notifications persist in database
- ✅ Each user sees only their own alerts
- ✅ Alerts survive app restarts

---

## Next Steps

- [Full Setup Guide](ALERT_SYSTEM_SETUP.md)
- [Implementation Details](ALERT_SYSTEM_IMPLEMENTATION.md)
- [API Documentation](ALERT_SYSTEM_SETUP.md#api-reference)

---

**Need help?** Check the full setup guide or implementation summary for detailed troubleshooting.
