# Warframe Utils - Price Alert System Implementation Summary

## Overview

A complete price alert system has been implemented for the Warframe Utils .NET application. The system allows authenticated users to set price thresholds for items and receive real-time notifications when prices fall to or below their specified targets.

## Features Implemented

### 1. **User Authentication**
- Leverages existing ASP.NET Identity system
- Alerts are user-specific and require login
- Each user has their own alerts and notifications

### 2. **Price Alert Management**
- Create, read, update, delete price alerts
- Set custom price thresholds for any Warframe item
- Enable/disable alerts without deletion
- Track current market prices
- View last checked timestamp

### 3. **Automatic Price Checking**
- Background service runs every 5 minutes
- Fetches current prices from Warframe Market API
- Automatically triggers notifications when thresholds are met
- Graceful error handling with detailed logging

### 4. **Notifications System**
- Real-time in-app notifications
- Track triggered alerts with timestamps
- Mark notifications as read
- Display current price vs. alert threshold

### 5. **Database**
- PostgreSQL support for reliable data persistence
- User-specific alerts stored in the database
- Complete notification history

## Project Structure

### New Files Created

1. **Models**
   - [PriceAlert.cs](Models/PriceAlert.cs) - Main alert entity
   - [AlertNotification.cs](Models/AlertNotification.cs) - Notification entity
   - [AlertDtos.cs](Models/DTOS/AlertDtos.cs) - Data transfer objects

2. **Controllers**
   - [AlertController.cs](Controllers/API/AlertController.cs) - RESTful API endpoints

3. **Services**
   - [PriceAlertCheckService.cs](Services/PriceAlertCheckService.cs) - Background price checking

4. **Pages**
   - [Pages/Alerts/Index.cshtml](Pages/Alerts/Index.cshtml) - UI dashboard
   - [Pages/Alerts/Index.cshtml.cs](Pages/Alerts/Index.cshtml.cs) - Page logic

5. **Database**
   - [Migrations/20260113120000_AddPriceAlertSystem.cs](Data/Migrations/20260113120000_AddPriceAlertSystem.cs) - Database schema

### Modified Files

1. **Program.cs** - Registered `PriceAlertCheckService` and switched to PostgreSQL
2. **appsettings.json** - Updated connection string for PostgreSQL
3. **Warframe Utils .NET.csproj** - Added Npgsql.EntityFrameworkCore.PostgreSQL package
4. **ApplicationDbContext.cs** - Added `PriceAlerts` and `AlertNotifications` DbSets
5. **WarframeMarketApiService.cs** - Added `GetItemPrice()` and `SearchAndGetPrice()` helper methods

## Database Schema

### PriceAlerts Table
- `Id` (Primary Key)
- `UserId` (Foreign Key to AspNetUsers)
- `ItemName` (Required, max 500 chars)
- `ItemId` (Optional item identifier)
- `AlertPrice` (Decimal threshold)
- `CurrentPrice` (Latest cached price)
- `IsActive` (Enable/disable flag)
- `IsTriggered` (Alert triggered flag)
- `TriggeredAt` (Timestamp when triggered)
- `IsAcknowledged` (User acknowledged flag)
- `CreatedAt` / `UpdatedAt` / `LastCheckedAt` (Timestamps)

### AlertNotifications Table
- `Id` (Primary Key)
- `UserId` (Foreign Key to AspNetUsers)
- `PriceAlertId` (Foreign Key to PriceAlerts)
- `Message` (Notification text)
- `TriggeredPrice` (Price when triggered)
- `IsRead` / `ReadAt` (Read status)
- `CreatedAt` (Notification timestamp)

## API Endpoints

All endpoints require authentication:

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/alert` | GET | Get all user's alerts |
| `/api/alert/{id}` | GET | Get specific alert |
| `/api/alert` | POST | Create new alert |
| `/api/alert/{id}` | PUT | Update alert |
| `/api/alert/{id}` | DELETE | Delete alert |
| `/api/alert/{id}/acknowledge` | POST | Acknowledge triggered alert |
| `/api/alert/notifications/unread` | GET | Get unread notifications |
| `/api/alert/notifications/{id}/read` | POST | Mark notification as read |

## User Interface

The dashboard is accessible at `/Pages/Alerts/`:

- **Active Alerts Section**: Shows all non-triggered alerts
- **Triggered Alerts Section**: Shows alerts that have been triggered
- **Notifications Section**: Displays unread notifications at the top
- **Create Alert Modal**: Simple form to add new alerts
- **Edit Alert Modal**: Update existing alerts
- **Auto-refresh**: Page refreshes every 30 seconds

### Features:
- Real-time price updates
- Quick create/edit/delete actions
- Visual status badges
- Responsive Bootstrap design
- Easy-to-use modals

## Installation & Setup

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL 12+

### Quick Start

1. **Install PostgreSQL** (if not already installed)
   - Windows: [Download here](https://www.postgresql.org/download/windows/)
   - Remember the password you set for the `postgres` user

2. **Create Database**
   ```sql
   psql -U postgres
   CREATE DATABASE warframe_utils;
   \q
   ```

3. **Update Connection String**
   Edit `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=warframe_utils;Username=postgres;Password=YOUR_PASSWORD"
     }
   }
   ```

4. **Apply Migrations**
   ```bash
   cd "Warframe Utils .NET"
   dotnet ef database update
   ```

5. **Run Application**
   ```bash
   dotnet run
   ```

6. **Access the App**
   - Open `https://localhost:5001`
   - Register a new account
   - Navigate to "Price Alerts" to start creating alerts

## Background Service Details

The `PriceAlertCheckService`:

- **Check Interval**: Every 5 minutes (configurable)
- **Operations Per Check**:
  1. Fetches all active, non-triggered alerts
  2. Queries Warframe Market API for current prices
  3. Compares prices against alert thresholds
  4. Creates notifications when triggered
  5. Updates alert status in database

- **Error Handling**:
  - Gracefully handles API failures
  - Logs errors without stopping other checks
  - Continues processing remaining alerts

- **Logging**: 
  - Detailed logs via Microsoft.Extensions.Logging
  - Check application output for debug information

## Testing Recommendations

1. **Create Multiple Alerts**
   - Test with different items
   - Set various price thresholds

2. **Monitor Background Service**
   - Check application logs for price checks
   - Verify LastCheckedAt timestamps update

3. **Test Price Triggering**
   - The service checks real market prices
   - Set low thresholds to see quick triggers
   - Or wait for actual price drops

4. **User Experience**
   - Test authentication flow
   - Verify notifications appear
   - Test modal interactions

## Troubleshooting

### PostgreSQL Connection Issues
- Verify PostgreSQL is running: `psql --version`
- Check connection string matches your setup
- Ensure database exists: `psql -U postgres -l`

### Migration Errors
- Verify `warframe_utils` database exists
- Check PostgreSQL username/password are correct
- Run `dotnet build` to verify no compilation errors

### Background Service Not Running
- Verify application is running (service only works while app runs)
- Check logs for `PriceAlertCheckService` entries
- Ensure database migrations are applied

### Price Not Updating
- Verify item name matches Warframe Market exactly
- Check Warframe Market API is accessible
- Review application logs for API errors

## Future Enhancements

### Potential Features
1. **Email Notifications** - Alert users via email
2. **Discord/Telegram Integration** - Send alerts to messaging apps
3. **Price History** - Track price changes over time
4. **Advanced Filters** - Rank, quality, rarity filtering
5. **Batch Operations** - Create multiple alerts at once
6. **Admin Dashboard** - Monitor system health and usage
7. **Notification Preferences** - Customize alert frequency
8. **Multiple Price Sources** - Support other trading APIs

### Performance Improvements
1. Implement Redis caching for API responses
2. Add pagination to alerts list
3. Optimize database queries with better indexes
4. Implement rate limiting for API

### Production Hardening
1. Email confirmation for new accounts
2. Rate limiting on alert creation
3. Alert limits per user
4. Backup and disaster recovery procedures
5. HTTPS enforcement
6. Security headers
7. Input validation and sanitization

## File Locations Quick Reference

- **Models**: `Models/PriceAlert.cs`, `Models/AlertNotification.cs`
- **API**: `Controllers/API/AlertController.cs`
- **Service**: `Services/PriceAlertCheckService.cs`
- **UI**: `Pages/Alerts/Index.cshtml`
- **Database**: `Data/ApplicationDbContext.cs`
- **Migrations**: `Data/Migrations/`
- **Setup Guide**: `ALERT_SYSTEM_SETUP.md`

## Support & Documentation

For detailed setup instructions, refer to [ALERT_SYSTEM_SETUP.md](ALERT_SYSTEM_SETUP.md)

## Implementation Statistics

- **Lines of Code**: ~1500+ lines
- **New Database Tables**: 2 (PriceAlerts, AlertNotifications)
- **API Endpoints**: 8 endpoints
- **Database Indexes**: 4 indexes
- **Files Created**: 7
- **Files Modified**: 5
- **Components**: Models, Controllers, Services, Views, Migrations

## Version Information

- **Target Framework**: .NET 8.0
- **Database**: PostgreSQL 12+
- **Entity Framework Core**: 8.0.15
- **Npgsql EF Provider**: 8.0.4

---

**Implementation Date**: January 13, 2026
**Status**: ✅ Complete and Ready for Testing
