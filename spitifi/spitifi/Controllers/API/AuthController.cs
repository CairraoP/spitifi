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
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _tokenService;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, JwtService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("hello")]
        // Identity
        // [Authorize] 
        //
        // JWT
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult Hello()
        {
            return Ok("Hello");
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Authenticate(ApiLoginModel loginRequest)
        {
            /* Autenticação por JWT */
            var identityUser = await _userManager.FindByNameAsync(loginRequest.Email);
            if(identityUser == null)
                return BadRequest("Invalid user or password");
            
            var resultPassword = await _signInManager.CheckPasswordSignInAsync(identityUser, loginRequest.Password, 
                false);
            
            if(!resultPassword.Succeeded)
                return BadRequest("Invalid user or password");
            
            var token = _tokenService.GenerateToken(identityUser);
            
            return Ok(token);
            
            /* autenticação por sessão -> Identity
            
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok(loginRequest.Email);    
            }
            else
            {
                return BadRequest("Erro no Login");
            }
            */
        }
        
        [HttpPost]
        [Route("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return NoContent();
        }
    }
}