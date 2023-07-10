using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proto1.Application.Contract;
using Proto1.Application.Services;

namespace Proto1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RolesController(IRoleService roleService)
        {
            this.roleService = roleService;            
        }

        [HttpGet]
        public async Task<IActionResult> Get(){
            try{
                var roles = await roleService.ListRoles();
                if (roles == null) return NoContent();

                return Ok(roles);
            }
            catch(Exception ex){
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpPost("createrole/{roleName}")]
        public async Task<IActionResult> CreateRole(string roleName){
            try
            {                
                var isRoleCreated = await roleService.CreateRoleAsync(roleName);
                if (!isRoleCreated) return BadRequest("Role was not created");

                return Ok("Resource Created");
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }   



    }
}