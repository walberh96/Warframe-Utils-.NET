# Warframe Utils Alert System - Setup Guide

This guide walks you through setting up the new Price Alert System for local development and testing.

## Prerequisites

- .NET 8.0 SDK ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- PostgreSQL 12+ ([Download](https://www.postgresql.org/download/))
- Visual Studio Code or Visual Studio 2022
- Git

## Step 1: Install PostgreSQL Locally

### Windows

1. Download PostgreSQL from [https://www.postgresql.org/download/windows/](https://www.postgresql.org/download/windows/)
2. Run the installer
3. During installation:
   - Keep default port `5432`
   - Remember the password you set for the `postgres` user (you'll need it)
   - Choose to install pgAdmin (optional but helpful for managing the database)
4. Verify installation by opening Command Prompt and running:
   ```cmd
   psql --version
   ```

### macOS

Using Homebrew:
```bash
brew install postgresql
brew services start postgresql
```

### Linux (Ubuntu/Debian)

```bash
sudo apt-get update
sudo apt-get install postgresql postgresql-contrib
sudo systemctl start postgresql
```

## Step 2: Create the Database

1. Open a terminal/command prompt and connect to PostgreSQL:
   ```cmd
   psql -U postgres
   ```

2. Create a new database:
   ```sql
   CREATE DATABASE warframe_utils;
   ```

3. Verify the database was created:
   ```sql
   \l
   ```
   (You should see `warframe_utils` in the list)

4. Exit psql:
   ```sql
   \q
   ```

## Step 3: Update Connection String

1. Open `appsettings.json` in your project root
2. Update the `DefaultConnection` with your PostgreSQL credentials:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=warframe_utils;Username=postgres;Password=YOUR_PASSWORD"
     },
     ...
   }
   ```

   Replace `YOUR_PASSWORD` with the password you set during PostgreSQL installation.

## Step 4: Create and Apply Database Migration

1. Open Terminal in VS Code or your project directory
2. Install Entity Framework Core CLI (if not already installed):
   ```bash
   dotnet tool install --global dotnet-ef
   ```

3. Create a new migration for the alert system:
   ```bash
   dotnet ef migrations add AddPriceAlertSystem
   ```

   You should see output like:
   ```
   Done. To undo this action, use 'ef migrations remove'
   ```

4. Apply the migration to create the database tables:
   ```bash
   dotnet ef database update
   ```

   You should see output showing the migration being applied.

5. Verify the tables were created (optional):
   ```bash
   psql -U postgres -d warframe_utils -c "\dt"
   ```

   You should see tables like `PriceAlerts`, `AlertNotifications`, etc.

## Step 5: Run the Application

1. In the terminal, navigate to your project directory:
   ```bash
   cd "Warframe Utils .NET"
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

   The application will start on `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP)

4. Open your browser and navigate to:
   ```
   https://localhost:5001
   ```

## Step 6: Test the Alert System

### Create a Test Account

1. Click the **Register** link in the top-right corner
2. Enter an email and password
3. Confirm your email (in development, check the application output or skip if email confirmation is disabled)

### Create a Price Alert

1. Log in with your test account
2. Navigate to **Price Alerts** (link in the navigation menu)
3. Click **New Alert**
4. Enter:
   - **Item Name**: A Warframe item like "Serration", "Excalibur Prime", or "Energy Nexus"
   - **Alert Price**: A reasonable price threshold (e.g., 100 platinum)
5. Click **Create Alert**

### Monitor Alerts

- The alert will appear in the "Active Alerts" section
- The background service checks prices every 5 minutes
- When a price drops to or below your alert threshold, you'll see:
  - A notification at the top of the page
  - The alert will move to the "Triggered Alerts" section
  - A notification in the database (visible in the notifications section)

## API Reference

The Alert System provides RESTful API endpoints at `/api/alert`:

### Get All User Alerts
```
GET /api/alert
Authorization: Required
Response: List of PriceAlertDto
```

### Get Single Alert
```
GET /api/alert/{id}
Authorization: Required
Response: PriceAlertDto
```

### Create Alert
```
POST /api/alert
Authorization: Required
Body: {
  "itemName": "string",
  "itemId": "string (optional)",
  "alertPrice": number
}
Response: PriceAlertDto
```

### Update Alert
```
PUT /api/alert/{id}
Authorization: Required
Body: {
  "itemName": "string (optional)",
  "alertPrice": number (optional),
  "isActive": boolean (optional)
}
Response: PriceAlertDto
```

### Delete Alert
```
DELETE /api/alert/{id}
Authorization: Required
Response: 204 No Content
```

### Acknowledge Alert
```
POST /api/alert/{id}/acknowledge
Authorization: Required
Response: PriceAlertDto
```

### Get Unread Notifications
```
GET /api/alert/notifications/unread
Authorization: Required
Response: List of AlertNotificationDto
```

### Mark Notification as Read
```
POST /api/alert/notifications/{notificationId}/read
Authorization: Required
Response: AlertNotificationDto
```

## Database Schema

### PriceAlerts Table
- `Id` (int, primary key)
- `UserId` (string, foreign key to AspNetUsers)
- `ItemName` (string)
- `ItemId` (string, optional)
- `AlertPrice` (decimal)
- `CurrentPrice` (decimal, nullable)
- `IsActive` (boolean)
- `IsTriggered` (boolean)
- `TriggeredAt` (datetime, nullable)
- `IsAcknowledged` (boolean)
- `CreatedAt` (datetime)
- `UpdatedAt` (datetime)
- `LastCheckedAt` (datetime, nullable)

### AlertNotifications Table
- `Id` (int, primary key)
- `UserId` (string, foreign key to AspNetUsers)
- `PriceAlertId` (int, foreign key to PriceAlerts)
- `Message` (string)
- `TriggeredPrice` (decimal)
- `IsRead` (boolean)
- `CreatedAt` (datetime)
- `ReadAt` (datetime, nullable)

## Troubleshooting

### Connection String Issues
**Error**: `FATAL: role "postgres" does not exist`
- Solution: Use the correct PostgreSQL username (usually `postgres`)

**Error**: `No password supplied but password authentication is required`
- Solution: Update your connection string with the correct password

### Migration Issues
**Error**: `The migration 'AddPriceAlertSystem' has already been applied`
- Solution: The migration has already been applied. You can safely continue.

**Error**: `Failed to compile Razor page`
- Solution: Check that you have all NuGet packages installed with `dotnet restore`

### Background Service Not Running
**Issue**: Alerts are created but not being triggered
- Ensure the application is running (background services only work while the app is active)
- Check the application logs for errors from `PriceAlertCheckService`
- Verify your PostgreSQL connection is working

### Item Price Not Found
**Issue**: Current price shows as "Checking..." or null
- The Warframe Market API may not have the exact item name
- Try using the exact name from the Warframe Market website
- Check application logs for API errors

## Development Tips

### Enable Detailed Logging
In `appsettings.Development.json`, change the logging level:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

### Manually Test Price Checking
You can manually trigger a price check by:
1. Editing the `PriceAlertCheckService.cs` and changing `_checkInterval` to a shorter duration (e.g., 10 seconds)
2. Restarting the application

### Reset Database
To start fresh:
```bash
# Remove the migration
dotnet ef migrations remove

# Drop the database
psql -U postgres -c "DROP DATABASE warframe_utils;"

# Recreate the database
psql -U postgres -c "CREATE DATABASE warframe_utils;"

# Recreate the migration
dotnet ef migrations add AddPriceAlertSystem

# Apply the migration
dotnet ef database update
```

## Next Steps

### Production Deployment
Before deploying to production:
1. Change email confirmation requirement (currently disabled in development)
2. Implement email notifications to alert users via email
3. Add rate limiting to the API
4. Set up HTTPS with proper certificates
5. Configure PostgreSQL with proper backups and security
6. Implement more sophisticated price checking (handle API rate limits)
7. Add admin dashboard for monitoring alerts

### Feature Enhancements
Consider adding:
- Email notifications when alerts trigger
- Discord/Telegram webhook notifications
- Price history charts and analytics
- Multiple price sources (not just Warframe Market)
- User preferences for check frequency
- Alert templates for common items

## Support

For issues or questions:
1. Check the application logs for detailed error messages
2. Verify your PostgreSQL connection string
3. Ensure all NuGet packages are installed
4. Check the Warframe Market API is accessible
5. Review the troubleshooting section above

Good luck with your Price Alert System!
