using EClaim.Domain.Entities;
using EClaim.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EClaim.Infrastructure
{
    public class DbSeeder
    {
        public static async Task SeedUsersAsync(AppDbContext context)
        {
            // Check if user already exists by email
            if (!await context.Users.AnyAsync(u => u.Email == "admin@example.com"))
            {
                var admin = new User
                {
                    FullName = "Admin User",
                    Email = "admin@example.com",
                    Phone = "1234567890",
                    Address = "Admin Street",
                    Role = Role.Admin,
                    IsEmailVerified = true,
                    CreatedAt = DateTime.Now,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123")
                };

                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }

            if (!await context.Users.AnyAsync(u => u.Email == "Adjuster@example.com"))
            {
                var user = new User
                {
                    FullName = "Adjuster User",
                    Email = "Adjuster@example.com",
                    Phone = "9876543210",
                    Address = "User Lane",
                    Role =Role.Adjuster,
                    IsEmailVerified = true,
                    CreatedAt = DateTime.Now,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Adjuster@123")
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            if (!await context.Users.AnyAsync(u => u.Email == "Approver@example.com"))
            {
                var user = new User
                {
                    FullName = "Approver User",
                    Email = "Approver@example.com",
                    Phone = "9876543210",
                    Address = "User Lane",
                    Role = Role.Approver,
                    IsEmailVerified = true,
                    CreatedAt = DateTime.Now,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Approver@123")
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
