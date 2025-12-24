using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.User;

public interface IUserService
{
    Task<GetUserByIdRes> GetUserByIdAsync(GetUserByIdReq req);
    Task<GetAllUsersRes> GetAllUsersAsync(GetAllUsersReq req);
    Task<CreateUserRes> CreateUserAsync(CreateUserReq req);
    Task<UpdateUserRes> UpdateUserAsync(UpdateUserReq req);
    Task<DeleteUserRes> DeleteUserAsync(DeleteUserReq req);
    Task<AssignRoleToUserRes> AssignRoleToUserAsync(AssignRoleToUserReq req);
    Task<RemoveRoleFromUserRes> RemoveRoleFromUserAsync(RemoveRoleFromUserReq req);
    Task<GetUserRolesRes> GetUserRolesAsync(GetUserRolesReq req);
}


