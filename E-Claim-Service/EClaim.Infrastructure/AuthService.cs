using EClaim.Domain.DTOs;
using EClaim.Domain.Entities;
using EClaim.Domain.Enums;
using EClaim.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EClaim.Infrastructure
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                throw new Exception("User already exists.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsEmailVerified = false 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return GenerateToken(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials.");

            return GenerateToken(user);
        }

        private AuthResponseDto GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("role", user.Role.ToString()),
            new Claim("name", user.FullName),
            new Claim("userId", user.Id.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                UserId= user.Id
            };
        }

        public async Task<bool> ConfirmEmail(string email, string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("Invalid credentials.");

            user.IsEmailVerified=true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
