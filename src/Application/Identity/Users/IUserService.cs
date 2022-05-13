using System.Security.Claims;
using Application.Identity.Users.Password;
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

    Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken);


    Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

    Task<string> CreateAsync(CreateUserCommand request, string origin);
    Task UpdateAsync(UpdateUserRequest request, string userId);

    Task<string> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task ResetPasswordAsync(ResetPasswordRequest request);
    Task ChangePasswordAsync(ChangePasswordRequest request, string userId);
}