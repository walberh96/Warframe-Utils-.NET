// Quick admin tool to check database users
// Run with: dotnet run --project "Warframe Utils .NET" --check-users

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Warframe_Utils_.NET.Data;

public class CheckUsers
{
    public static async Task CheckDatabaseUsers(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        
        Console.WriteLine("==============================================");
        Console.WriteLine("DATABASE USERS CHECK");
        Console.WriteLine("==============================================");
        
        var users = await context.Users.ToListAsync();
        
        if (users.Count == 0)
        {
            Console.WriteLine("❌ NO USERS FOUND IN DATABASE");
            Console.WriteLine("The database was recently reset. You need to register a new account.");
        }
        else
        {
            Console.WriteLine($"✅ Found {users.Count} user(s):");
            Console.WriteLine();
            
            foreach (var user in users)
            {
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Username: {user.UserName}");
                Console.WriteLine($"Email Confirmed: {user.EmailConfirmed}");
                Console.WriteLine($"Lockout End: {user.LockoutEnd?.ToString() ?? "Not locked"}");
                Console.WriteLine("---");
            }
        }
        
        Console.WriteLine("==============================================");
    }
}
