﻿using Application.Identity.Users.UserCommands.CreateUser;
using Application.Identity.Users.UserCommands.UpdateUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task<string> CreateAsync(CreateUserCommand request, string origin)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);

            // If Create failed throw exception
            if (!result.Succeeded)
            {
                throw new InternalServerException("Validation Errors Occurred.", result.GetErrors());
            }

            // If param roles is null asign role User to the array of roles
            //if (roles == null) roles = new string[] { Enum.GetName(RoleEnum.User)! };

            // Add Roles to new user
            //await _userManager.AddToRolesAsync(user, roles);

            return user.Id;
        }

        public async Task UpdateAsync(UpdateUserRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            ChangeUserData(user, request);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new InternalServerException("Update profile failed", result.GetErrors());
            }

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
