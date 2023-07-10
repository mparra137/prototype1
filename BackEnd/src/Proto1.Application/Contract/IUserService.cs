using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Proto1.Application.DTOs;
using Proto1.Domain.Identity;

namespace Proto1.Application.Contract;

public interface IUserService
{
    Task<UserCreateDto> CreateAccountAsync(UserCreateDto userDto);
    Task<SignInResult> CheckUserPasswordAsync(string userName ,string password);
    Task<UserCreateDto> GetUserByUsernameAsync(string userName);

    Task<List<UserCreateDto>> GetUsersAsync();

    Task<bool> AssignRole(string userName, string roleName);
}
