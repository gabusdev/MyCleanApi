﻿using Application.Identity.Users.UserCommands.CreateUser;
using Application.Identity.Users.UserCommands.UpdateUser;
using Shared.Authorization;

namespace Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task<string> CreateAsync(CreateUserCommand request, string origin)
        {
            var user = request.Adapt<ApplicationUser>();

            user.IsActive = true;

            var result = await _userManager.CreateAsync(user, request.Password);

            // If Create failed throw exception
            if (!result.Succeeded)
            {
                throw new ValidationException(_localizer["validation.errors"], result.GetErrors());
            }

            // Add Roles to new user
            await _userManager.AddToRoleAsync(user, ApiRoles.Basic);

            return user.Id;
        }

        public async Task UpdateAsync(UpdateUserRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new NotFoundException(_localizer["identity.usernotfound"]);

            ChangeUserData(user, request);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new ValidationException(_localizer["validation.errors"], result.GetErrors());
            }

        }

        public async Task DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new NotFoundException(_localizer["identity.usernotfound"]);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new ConflictException(_localizer["entity.delete.failed", userId]);
        }

        private static void ChangeUserData(ApplicationUser user, UpdateUserRequest updateRequest)
        {
            user.FirstName = updateRequest.FirstName ?? default;
            user.LastName = updateRequest.LastName ?? default;
            user.Email = updateRequest.Email ?? default;
            user.UserName = updateRequest.UserName ?? default;
            user.PhoneNumber = updateRequest.PhoneNumber ?? default;
        }
    }
}
