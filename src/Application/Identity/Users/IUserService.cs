using Application.Common.Pagination;
using Application.Identity.Users.Commands.CreateUser;
using Application.Identity.Users.Commands.SetRoles;
using Application.Identity.Users.Commands.ToggleUserStatus;
using Application.Identity.Users.Commands.UpdateUser;
using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.Password.Commands.ResetPassword;
using Application.Identity.Users.Password.Queries.ForgotPasswordQuery;
using Application.Identity.Users.Queries;

namespace Application.Identity.Users;

public interface IUserService
{

    Task<bool> ExistsWithNameAsync(string name, string? exceptId = null);
    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);

    Task<UserDetailsDto?> GetByIdAsync(string userId, CancellationToken cancellationToken);
    Task<List<UserDetailsDto>> GetAsync(CancellationToken cancellationToken);
    Task<PagedList<UserDetailsDto>> GetPagedListAsync(PaginationParams paginationParams, CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken);
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken);

    Task<bool> HasRoleAsync(string userId, string role, CancellationToken cancellationToken);
    Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken);
    Task<string> AssignRolesAsync(string userId, SetUserRolesRequest request, CancellationToken cancellationToken);
    Task<double> GetSecurityLevel(string userId, CancellationToken cancellationToken);

    Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

    Task<string> CreateAsync(CreateUserCommand request, string origin);
    Task UpdateAsync(UpdateUserRequest request, string userId);
    Task DeleteAsync(string userId, CancellationToken cancellationToken);

    Task<string> ForgotPasswordAsync(ForgotPasswordQuery request, string origin);
    Task ResetPasswordAsync(ResetPasswordCommand request);
    Task ChangePasswordAsync(ChangePasswordCommand request, string userId);

    Task<string> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken);
}