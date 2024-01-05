using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proto1.Application.Contract;
using Proto1.Application.Services;
using Proto1.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Proto1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly IRoleService roleService;

        public UserController(IUserService userService, ITokenService tokenService, IRoleService roleService)
        {
            this.tokenService = tokenService;
            this.roleService = roleService;
            this.userService = userService;            
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto){
            try
            {
                var userReturn = await userService.CreateAccountAsync(userCreateDto);
                if (userReturn == null) return NoContent();

                return Ok(userReturn);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error while trying to create user ({ex.Message})");          
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLogin){
            try
            {
                var user = await userService.GetUserByUsernameAsync(userLogin.UserName);
                if (user == null) return Unauthorized("Usuário ou senha inválidos");
                
                var result = await userService.CheckUserPasswordAsync(userLogin.UserName, userLogin.Password);
                if (!result.Succeeded)
                    return Unauthorized("Usuário ou senha inválidos");

                return Ok(new {
                    userName = user.UserName,
                    email = user.Email,
                    token = tokenService.GenerateToken(user).Result
                });
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Server Error: {ex.Message}");
            }
        }        

        [Authorize(Roles ="Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetUsersList(){
            try
            {
                var users = await userService.GetUsersAsync();
                if (users == null) return NoContent();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                
            }
        }

        [Authorize(Roles ="Administrator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id){
            try
            {
                var userData = await userService.GetUserByIdAsync(id);
                if (userData == null) return NoContent();

                return Ok(userData);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles ="Administrator")]
        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRoleToUser(string userName, string roleName){
            try{
                var success = await userService.AssignRole(userName, roleName);
                
                return Ok(new {
                    Ok = success
                });              

            } catch (Exception ex){
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles ="Administrator")]
        [HttpPost("assignListofRoles")]
        public async Task<IActionResult> AssignListOfRoles(string userName, List<RoleUpdate> rolesList){
            try
            {
                await userService.AssignListOfRolesAsync(userName, rolesList);
                return Ok();
            }
            catch (Exception ex) 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);               
            }        
        }


        [Authorize(Roles ="Administrator")]
        [HttpPost("removeRole")]
        public async Task<IActionResult> RemoveRoleFromUser(string userName, string roleName){
            try
            {
                bool success = await userService.RemoveRoleFromUser(userName, roleName);
                
                return Ok(new {
                    Success = success
                });                
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles ="Administrator")]
        [HttpGet("roles")]
        public async Task<IActionResult> Roles(string userName){
            try
            {
                var user = await userService.GetUserByUsernameAsync(userName);
                if (user == null) return NoContent();

                var rolesList = await userService.GetUserRoles(userName);

                return Ok(rolesList);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("checktoken")]
        public IActionResult CheckToken(){
            return Ok(new {
                Ok = "success"
            });
        }
    }
}