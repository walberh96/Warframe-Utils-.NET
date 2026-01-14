# 🚀 Warframe Utils - Complete Setup Guide

This guide will walk you through setting up both the Next.js frontend and .NET backend from scratch.

## 📋 Prerequisites Checklist

Before you begin, ensure you have:

- [ ] **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- [ ] **Node.js 18.17+** - [Download here](https://nodejs.org/)
- [ ] **PostgreSQL 14+** - [Installation guide below](#postgresql-installation)
- [ ] **Git** (optional) - For cloning the repository
- [ ] **Code Editor** - VS Code, Visual Studio, or your preferred IDE

---

## Part 1: PostgreSQL Database Setup

### Windows Installation

1. **Download PostgreSQL:**
   - Visit: https://www.postgresql.org/download/windows/
   - Download the installer (e.g., postgresql-14.x-windows-x64.exe)

2. **Run the installer:**
   - Accept defaults (install directory, port 5432)
   - Set a password for the `postgres` user (remember this!)
   - Complete installation

3. **Verify installation:**
```powershell
# Open PowerShell
psql --version
# Should output: psql (PostgreSQL) 14.x
```

### macOS Installation

```bash
# Using Homebrew
brew install postgresql@14

# Start PostgreSQL service
brew services start postgresql@14

# Verify
psql --version
```

### Linux (Ubuntu/Debian) Installation

```bash
# Update package list
sudo apt-get update

# Install PostgreSQL
sudo apt-get install postgresql-14 postgresql-contrib

# Start service
sudo systemctl start postgresql
sudo systemctl enable postgresql

# Verify
psql --version
```

### Create the Database

```bash
# Connect to PostgreSQL (password: the one you set during installation)
psql -U postgres

# In the PostgreSQL prompt:
CREATE DATABASE warframe_utils;

# Grant permissions (if needed)
GRANT ALL PRIVILEGES ON DATABASE warframe_utils TO postgres;

# Exit
\q
```

---

## Part 2: Backend (.NET) Setup

### Step 1: Navigate to Backend Directory

```bash
cd "Warframe Utils .NET"
```

### Step 2: Configure Database Connection

Open `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=warframe_utils;Username=postgres;Password=YOUR_PASSWORD_HERE"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Replace `YOUR_PASSWORD_HERE` with your PostgreSQL password!**

### Step 3: Restore NuGet Packages

```bash
dotnet restore
```

### Step 4: Run Database Migrations

```bash
# This creates all necessary tables
dotnet ef database update

# If you get an error about dotnet-ef not found:
dotnet tool install --global dotnet-ef
# Then try again:
dotnet ef database update
```

### Step 5: Build the Backend

```bash
dotnet build
```

### Step 6: Run the Backend

```bash
dotnet run
```

You should see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

**✅ Backend is now running! Leave this terminal open.**

---

## Part 3: Frontend (Next.js) Setup

### Step 1: Open a New Terminal

Keep the backend terminal running and open a **new terminal window**.

### Step 2: Navigate to Frontend Directory

```bash
cd warframe-frontend
```

### Step 3: Install Dependencies

```bash
# Using npm
npm install

# Or using yarn
yarn install

# Or using pnpm
pnpm install
```

This may take a few minutes as it downloads all required packages.

### Step 4: Verify Configuration

Check `next.config.mjs` to ensure it's pointing to your backend:

```javascript
/** @type {import('next').NextConfig} */
const nextConfig = {
  images: {
    domains: ['warframe.market'],
  },
  async rewrites() {
    return [
      {
        source: '/api/:path*',
        destination: 'http://localhost:5000/api/:path*', // Should match your backend URL
      },
    ];
  },
};

export default nextConfig;
```

### Step 5: Start the Development Server

```bash
npm run dev
```

You should see:
```
  ▲ Next.js 14.2.3
  - Local:        http://localhost:3000
  - Ready in 2.5s
```

**✅ Frontend is now running!**

---

## Part 4: Access the Application

### Open Your Browser

Navigate to:
```
http://localhost:3000
```

You should see the beautiful Warframe Utils interface! 🎉

### Test the Features

1. **Game Status Cards** - Should display current Cetus cycle, Void Trader status, Venus cycle
2. **Market Search** - Try searching for "Serration" or any mod name (autocomplete should appear)
3. **Price Alerts** - Click the notification bell to create and manage alerts (requires login)

---

## 🔧 Troubleshooting

### Backend Issues

#### ❌ "Connection refused" or Database errors

**Solution:**
1. Ensure PostgreSQL is running:
```bash
# Windows (in Services)
# Check if "postgresql-x64-14" service is running

# macOS
brew services list

# Linux
sudo systemctl status postgresql
```

2. Verify connection string in `appsettings.json`
3. Try connecting manually:
```bash
psql -U postgres -d warframe_utils
```

#### ❌ "dotnet ef not found"

**Solution:**
```bash
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

#### ❌ Migration errors

**Solution:**
```bash
# Remove existing migrations (if any)
dotnet ef database drop
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Frontend Issues

#### ❌ "Module not found" errors

**Solution:**
```bash
# Clear cache and reinstall
rm -rf node_modules .next
npm install
```

#### ❌ API calls failing (CORS errors)

**Solution:**
1. Ensure backend is running on port 5000
2. Check CORS configuration in `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

3. Check browser console for specific error messages

#### ❌ "Port 3000 already in use"

**Solution:**
```bash
# Use a different port
npm run dev -- -p 3001

# Or kill the process using port 3000
# Windows PowerShell:
Get-Process -Id (Get-NetTCPConnection -LocalPort 3000).OwningProcess | Stop-Process

# macOS/Linux:
lsof -ti:3000 | xargs kill
```

---

## 🎯 Verification Checklist

After setup, verify everything works:

- [ ] Backend runs without errors on port 5000
- [ ] Frontend runs without errors on port 3000
- [ ] Game status cards show data (or loading states)
- [ ] Market search returns results
- [ ] No console errors in browser DevTools (F12)
- [ ] Database connection works (check backend logs)

---

## 📊 Understanding the Ports

| Service | Port | URL |
|---------|------|-----|
| **Backend API** | 5000 | http://localhost:5000 |
| **Backend HTTPS** | 5001 | https://localhost:5001 |
| **Frontend** | 3000 | http://localhost:3000 |

The frontend automatically proxies API calls to the backend via Next.js rewrites.

---

## 🔐 User Authentication (Optional)

To use the Price Alerts feature, you need to:

1. **Register a user account:**
   - Click "Register" (if using old UI) or visit `http://localhost:5000/Identity/Account/Register`
   - Create an account with email and password

2. **Login:**
   - Login with your credentials
   - Now you can create price alerts

**Note:** The Next.js frontend currently uses the backend's authentication. Future updates will include JWT-based auth.

---

## 🚀 Next Steps

Once everything is working:

1. **Explore the UI** - Search for items, create alerts
2. **Check the Documentation:**
   - [Frontend README](../warframe-frontend/README.md) - Component details
   - [README_ALERTS.md](../README_ALERTS.md) - Alert system documentation
3. **Customize:**
   - Change colors in `globals.css`
   - Modify components in `src/components/`
   - Add new features!

---

## 💡 Development Tips

### Hot Reload

Both frontend and backend support hot reload:
- **Frontend**: Changes automatically refresh the browser
- **Backend**: Use `dotnet watch run` for automatic rebuilds

### Debugging

**Frontend:**
```bash
# Next.js provides excellent error messages
# Check browser console (F12) for errors
```

**Backend:**
```bash
# Run with more logging
dotnet run --environment Development

# Or use Visual Studio debugger
```

### Database Management

```bash
# View database contents
psql -U postgres -d warframe_utils

# List tables
\dt

# View price alerts
SELECT * FROM "PriceAlerts";

# Exit
\q
```

---

## 🎨 Customization

### Change Theme Colors

Edit `warframe-frontend/src/app/globals.css`:

```css
:root {
  --primary: 221.2 83.2% 53.3%;  /* Blue - change these values */
  --secondary: 210 40% 96.1%;
}
```

### Modify Backend Port

Edit `Warframe Utils .NET/Properties/launchSettings.json`:

```json
{
  "applicationUrl": "https://localhost:YOUR_PORT;http://localhost:YOUR_PORT"
}
```

Don't forget to update `next.config.mjs` with the new port!

---

## 📞 Getting Help

If you encounter issues:

1. **Check the error message** - Most errors are descriptive
2. **Review this guide** - Follow each step carefully
3. **Check logs:**
   - Backend: Terminal output
   - Frontend: Browser console (F12)
4. **Search the issue** - Common problems have solutions online

---

## ✅ Success!

If you've made it this far and everything works, congratulations! 🎉

You now have a fully functional Warframe Utils application with:
- ✅ Modern Next.js frontend
- ✅ Powerful .NET backend
- ✅ PostgreSQL database
- ✅ Real-time market data
- ✅ Price alert system

**Happy trading, Tenno!**

---

**Need more help?** Check the other documentation files or create an issue on GitHub.
