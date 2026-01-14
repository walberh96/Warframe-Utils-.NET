#!/usr/bin/env pwsh
# Quick script to check database users

Write-Host "==============================================`n" -ForegroundColor Cyan
Write-Host "Checking Database Users...`n" -ForegroundColor Cyan
Write-Host "==============================================`n" -ForegroundColor Cyan

cd "Warframe Utils .NET"

# Create a temporary script to query users
$script = @"
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warframe_Utils_.NET.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var users = await context.Users.ToListAsync();
    
    if (users.Count == 0)
    {
        Console.WriteLine("❌ NO USERS IN DATABASE");
        Console.WriteLine("The database was reset. Please register a new account.");
    }
    else
    {
        Console.WriteLine($"✅ Found {users.Count} user(s):\n");
        foreach (var user in users)
        {
            Console.WriteLine($"  Email: {user.Email}");
            Console.WriteLine($"  Username: {user.UserName}");
            Console.WriteLine($"  Email Confirmed: {user.EmailConfirmed}");
            Console.WriteLine($"  Locked: {(user.LockoutEnd.HasValue ? "Yes" : "No")}");
            Console.WriteLine();
        }
    }
}
"@

dotnet ef dbcontext info

Write-Host "`n==============================================`n" -ForegroundColor Cyan
Write-Host "To register a new user, go to:" -ForegroundColor Yellow
Write-Host "  http://localhost:5089/Identity/Account/Register`n" -ForegroundColor Green
Write-Host "==============================================`n" -ForegroundColor Cyan

cd ..
