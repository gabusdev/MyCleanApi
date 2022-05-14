using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.Password.Commands.ResetPassword;
using Application.Identity.Users.Password.Queries.ForgotPasswordQuery;
using Application.Identity.Users.UserCommands.CreateUser;
using Application.Identity.Users.UserCommands.ToggleUserStatus;
using Application.Identity.Users.UserCommands.UpdateUser;
using Application.Identity.Users.UserQueries;

namespace Application.Identity.Users;

public interface IUserService
{

    Task<bool> ExistsWithNameAsync(string name);
    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);

    Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken);
    Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken);
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken);

    Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken);
    Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken);

    Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

    Task<string> CreateAsync(CreateUserCommand request, string origin);
    Task UpdateAsync(UpdateUserRequest request, string userId);

    Task<string> ForgotPasswordAsync(ForgotPasswordQuery request);
    Task ResetPasswordAsync(ResetPasswordCommand request);
    Task ChangePasswordAsync(ChangePasswordCommand request, string userId);
}