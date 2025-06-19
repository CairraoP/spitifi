using spitifi.Areas.Identity.Pages.Account;
using spitifi.Models.ApiModels;
using spitifi.Services.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace spitifi.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtService _tokenService;

        public IdentityController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, JwtService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        
        [HttpPost]
        [Route("upadmin")]
        [Authorize(Roles = "a")] // a == admin... should have renamed before this... ohh well..
        public async Task<ActionResult> upgradeToAdmin([FromBody] UserRoleUpdate requestBody)
        {
            // validate input
            if (string.IsNullOrWhiteSpace(requestBody.username))
            {
                return BadRequest("username is required");
            }
            
            // get user
            var user = await _userManager.FindByNameAsync(requestBody.username);
            if (user == null)
            {
                return NotFound("user not found");
            }
            
            // ensure first that the role exists
            // a == admin... should have renamed before this... ohh well..
            if (!await _roleManager.RoleExistsAsync("a"))
            {
                await _roleManager.CreateAsync(new IdentityRole("a"));
            }

            // add role
            var result = await _userManager.AddToRoleAsync(user, "a");
            
            // result
            if (!result.Succeeded)
            {
                return StatusCode(500, new { Errors = result.Errors });
            }

            return Ok(new { Message = "admin role added successfully" });
        }
        
        [HttpDelete]
        [Route("downadmin")]
        [Authorize(Roles = "a")] // a == admin... should have renamed before this... ohh well..
        public async Task<ActionResult> downgradeFromAdmin([FromBody] UserRoleUpdate requestBody)
        {
            // validate input
            if (string.IsNullOrWhiteSpace(requestBody.username))
            {
                return BadRequest("username is required");
            }
            
            // get user
            var user = await _userManager.FindByNameAsync(requestBody.username);
            if (user == null)
            {
                return NotFound("user not found");
            }
            
            // validate if user not an admin
            // a == admin... should have renamed before this... ohh well..
            if (!await _userManager.IsInRoleAsync(user, "a"))
            {
                return BadRequest("user isn't an admin");
            }

            // remove role
            var result = await _userManager.RemoveFromRoleAsync(user, "a");
            
            // result
            if (!result.Succeeded)
            {
                return StatusCode(500, new { Errors = result.Errors });
            }

            return Ok(new { Message = "admin role removed successfully" });
        }
        
        [HttpPut]
        [Route("isartist")]
        [Authorize(Roles = "a")] // a == admin... should have renamed before this... ohh well..
        public async Task<ActionResult> registerAsArtist([FromBody] UserRoleUpdate requestBody)
        {
            // validate input
            if (string.IsNullOrWhiteSpace(requestBody.username))
            {
                return BadRequest("username is required");
            }
            
            // get user
            var user = await _userManager.FindByNameAsync(requestBody.username);
            if (user == null)
            {
                return NotFound("user not found");
            }
            
            // ensure first that the role exists
            // ar == artist... should have renamed before this... ohh well..
            if (!await _roleManager.RoleExistsAsync("ar"))
            {
                await _roleManager.CreateAsync(new IdentityRole("ar"));
            }

            // add role
            var result = await _userManager.AddToRoleAsync(user, "ar");
            
            // result
            if (!result.Succeeded)
            {
                return StatusCode(500, new { Errors = result.Errors });
            }

            return Ok(new { Message = "artist role added successfully" });
        }
    }
}