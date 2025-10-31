using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProjectAPI.Models;
using ProjectAPI.DTOs;

namespace ProjectAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<bool> UserExists(string email);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = CreateToken(user);
            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username
            };
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
                throw new Exception("User not found.");

            if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Wrong password.");

            var token = CreateToken(user);
            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username
            };
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}