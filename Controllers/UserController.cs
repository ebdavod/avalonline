using AvvalOnline.Shop.Api.Attributes;
using AvvalOnline.Shop.Api.Messaging.User;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvvalOnline.Shop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<LoginRes> Login(LoginReq req)
        {
            var user = await _authService.AuthenticateAsync(req.Entity.Username, req.Entity.Password);

            if (user is null)
                return new LoginRes { Message = "کاربر یافت نشد" };

            var token = await _authService.GetUserTokenAsync(req.Entity.Username, req.Entity.Password);
            if (string.IsNullOrEmpty(token))
                return new LoginRes { Message = "توکن معتبر نیست" };

            return new LoginRes
            {
                Entity = new UserLoginResultVm
                {
                    Id = user.Id,
                    Username = req.Entity.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = token
                },
                IsSuccess = true,
                Message = "Success"
            };
        }

        [HttpPost]
        public async Task<RegisterRes> Register(RegisterReq req)
        {
            var createUserRes = await _userService.CreateUserAsync(new CreateUserReq
            {
                Entity = new UserCreateDTO
                {
                    Address = req.Entity.Address,
                    Email = req.Entity.Email,
                    FirstName = req.Entity.FirstName,
                    IdentifierCode = req.Entity.IdentifierCode,
                    LastName = req.Entity.LastName,
                    NationalCode = req.Entity.NationalCode,
                    Password = req.Entity.Password,
                    PhoneNumber = req.Entity.PhoneNumber,
                    Username = req.Entity.PhoneNumber,
                }
            });
            return new RegisterRes
            {
                IsSuccess = createUserRes.IsSuccess,
                Message = createUserRes.Message
            };
        }

        [HttpGet]
        [CustomAuthorize(["Admin"])]
        public async Task<GetAllUsersRes> GetAll(int index, int size)
        {
            return await _userService.GetAllUsersAsync(new GetAllUsersReq
            {
                Page = index,
                PageSize = size
            });
        }

        [HttpGet]
        [CustomAuthorize("Admin", "User")]
        public async Task<GetUserByIdRes> GetById(int id)
        {
            return await _userService.GetUserByIdAsync(new GetUserByIdReq { Id = id });
        }

        [HttpPost]
        [CustomAuthorize(["Admin"])]
        public async Task<DeleteUserRes> Delete(DeleteUserReq req)
        {
            return await _userService.DeleteUserAsync(req);
        }

        [HttpPost]
        [CustomAuthorize(["Admin", "User"])]
        public async Task<UpdateUserRes> Update(UpdateUserReq req)
        {
            return await _userService.UpdateUserAsync(req);
        }

        [HttpPost]
        [CustomAuthorize(["Admin"])]
        public async Task<AssignRoleToUserRes> AssignRole(AssignRoleToUserReq req)
        {
            return await _userService.AssignRoleToUserAsync(req);
        }

        [HttpPost]
        [CustomAuthorize(["Admin"])]
        public async Task<RemoveRoleFromUserRes> RemoveRole(RemoveRoleFromUserReq req)
        {
            return await _userService.RemoveRoleFromUserAsync(req);
        }

        [HttpGet]
        [CustomAuthorize(["Admin", "User"])]
        public async Task<GetUserRolesRes> GetRoles(int id)
        {
            return await _userService.GetUserRolesAsync(new GetUserRolesReq { Id = id });
        }
    }
}
