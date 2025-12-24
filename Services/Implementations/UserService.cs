using AvvalOnline.Shop.Api.Messaging.User;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Model.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AvvalOnline.Shop.Api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ShopDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(ShopDbContext shopDbContext)
        {
            _db = shopDbContext;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<CreateUserRes> CreateUserAsync(CreateUserReq req)
        {
            var existEntity = await _db.Users.SingleOrDefaultAsync(x => x.Username == req.Entity.Username.Trim());
            if (existEntity is not null)
            {
                return new CreateUserRes { Message = "کاربر با این نام کاربری وجود دارد" };
            }

            var entity = new User
            {
                Address = req.Entity.Address,
                Email = req.Entity.Email,
                FirstName = req.Entity.FirstName,
                IdentifierCode = req.Entity.IdentifierCode,
                LastName = req.Entity.LastName,
                NationalCode = req.Entity.NationalCode,
                PhoneNumber = req.Entity.PhoneNumber,
                Username = req.Entity.Username,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            entity.PasswordHash = _passwordHasher.HashPassword(entity, req.Entity.Password);

            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();

            return new CreateUserRes { IsSuccess = true, Message = "Success" };
        }

        public async Task<DeleteUserRes> DeleteUserAsync(DeleteUserReq req)
        {
            var user = await _db.Users.FindAsync(req.Id);
            if (user is null)
                return new DeleteUserRes { IsSuccess = false, Message = "کاربر یافت نشد" };

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return new DeleteUserRes { IsSuccess = true, Message = "کاربر حذف شد" };
        }

        public async Task<GetAllUsersRes> GetAllUsersAsync(GetAllUsersReq req)
        {
            var query = _db.Users.AsQueryable();
            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            return new GetAllUsersRes
            {
                Entities = users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Username = u.Username,
                    NationalCode = u.NationalCode,
                    Address = u.Address
                }).ToList(),
                TotalCount = totalCount,
                Page = req.Page,
                PageSize = req.PageSize,
                IsSuccess = true,
                Message = "Success"
            };
        }

        public async Task<GetUserByIdRes> GetUserByIdAsync(GetUserByIdReq req)
        {
            var user = await _db.Users.FindAsync(req.Id);
            if (user is null)
                return new GetUserByIdRes { IsSuccess = false, Message = "کاربر یافت نشد" };

            return new GetUserByIdRes
            {
                IsSuccess = true,
                Entity = new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Username = user.Username,
                    NationalCode = user.NationalCode,
                    Address = user.Address
                },
                Message = "Success"
            };
        }

        public async Task<UpdateUserRes> UpdateUserAsync(UpdateUserReq req)
        {
            var user = await _db.Users.FindAsync(req.Entity.Id);
            if (user is null)
                return new UpdateUserRes { IsSuccess = false, Message = "کاربر یافت نشد" };

            user.FirstName = req.Entity.FirstName;
            user.LastName = req.Entity.LastName;
            user.Email = req.Entity.Email;
            user.PhoneNumber = req.Entity.PhoneNumber;
            user.Address = req.Entity.Address;
            user.NationalCode = req.Entity.NationalCode;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return new UpdateUserRes { IsSuccess = true, Message = "Success" };
        }

        public async Task<AssignRoleToUserRes> AssignRoleToUserAsync(AssignRoleToUserReq req)
        {
            var dto = req.Entity;
            var user = await _db.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (user is null)
                return new AssignRoleToUserRes { IsSuccess = false, Message = "کاربر یافت نشد" };

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);
            if (role is null)
                return new AssignRoleToUserRes { IsSuccess = false, Message = "نقش یافت نشد" };

            if (!user.UserRoles.Any(ur => ur.RoleId == dto.RoleId))
            {
                user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                await _db.SaveChangesAsync();
            }

            return new AssignRoleToUserRes { IsSuccess = true, Message = "نقش اضافه شد" };
        }


        public async Task<GetUserRolesRes> GetUserRolesAsync(GetUserRolesReq req)
        {
            var roles = await _db.UserRoles
                .Where(ur => ur.UserId == req.Id) // چون RequestByIdBase<int> هست
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            return new GetUserRolesRes
            {
                Entities = roles,   // چون از ResponseListBase<string> ارث‌بری می‌کنه
                IsSuccess = true,
                Message = "Success"
            };

        }


        public async Task<RemoveRoleFromUserRes> RemoveRoleFromUserAsync(RemoveRoleFromUserReq req)
        {
            var dto = req.Entity;
            var userRole = await _db.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
            if (userRole is null)
                return new RemoveRoleFromUserRes { IsSuccess = false, Message = "نقش یافت نشد" };

            _db.UserRoles.Remove(userRole);
            await _db.SaveChangesAsync();

            return new RemoveRoleFromUserRes { IsSuccess = true, Message = "نقش حذف شد" };
        }

    }
}
