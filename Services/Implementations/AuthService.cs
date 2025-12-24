using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.User;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Model.Entites;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AvvalOnline.Shop.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ShopDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;
        IJwtUtils _jwtUtils;

        public AuthService(ShopDbContext db, IJwtUtils jwtUtils)
        {
            _db = db;
            _jwtUtils = jwtUtils;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserDTO> AuthenticateAsync(string username, string password)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Username == username && u.Status == UserStatus.Active);

            if (user == null)
                return null;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verificationResult != PasswordVerificationResult.Success)
                return null;

            user.LastLogin = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return new UserDTO
            {
                Address = user.Address,
                NationalCode = user.NationalCode,
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Username = user.Username
            };
        }

        public async Task<string> GetUserTokenAsync(string username, string password)
        {
            var user = await _db.Users
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user is null)
                return null;

            var pHasher = new PasswordHasher<User>();
            var hashedPass = pHasher.HashPassword(user, password);
            var result = pHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if ((result != PasswordVerificationResult.Success))
            {
                return null;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("firstName", user.FirstName ?? ""),
                new Claim("lastName", user.LastName ?? ""),
                new Claim("phoneNumber", user.PhoneNumber ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // اضافه کردن نقش‌ها به claims
            var roles =await GetUserRolesAsync(user.Id);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // اضافه کردن دسترسی‌ها به claims
            var permissions = await GetUserPermissionsAsync(user.Id);
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var tk = _jwtUtils.GenerateToken(claims);

            return tk;
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            var permissions = await GetUserPermissionsAsync(userId);
            return permissions.Contains(permissionName);
        }

        public async Task<bool> HasRoleAsync(int userId, string roleName)
        {
            var roles = await GetUserRolesAsync(userId);
            return roles.Contains(roleName);
        }

        public async Task<List<string>> GetUserPermissionsAsync(int userId)
        {
            return await _db.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Where(rp => rp.Permission.IsActive)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            return await _db.UserRoles
                .Where(ur => ur.UserId == userId && ur.Role.IsActive)
                .Select(ur => ur.Role.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
                return false;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword);
            if (verificationResult != PasswordVerificationResult.Success)
                return false;

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Status == UserStatus.Active);
            if (user == null)
                return false;

            // تولید کد تأیید و ارسال ایمیل
            var verificationCode = GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            user.VerificationCodeExpiry = DateTime.UtcNow.AddHours(24);
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            // TODO: ارسال ایمیل با کد تأیید
            // await _emailService.SendPasswordResetEmailAsync(user.Email, verificationCode);

            return true;
        }

        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

    }
}
