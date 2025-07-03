using spitifi.Models.ApiModels;
using spitifi.Services.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace spitifi.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _tokenService;

        public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, JwtService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Testa autenticação e autorização JWT para utilizador administrador
        /// </summary>
        /// <remarks>
        /// Requer token JWT com claim de Administrado presente
        /// 
        /// </remarks>
        /// <returns>
        /// <para>200 OK: Retorna mensagem de sucesso quando a operação é concluída</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// </returns>
        /// <response code="200">Retorna string "Hello"</response>
        /// <response code="401">Token de autenticação não presente</response>
        /// <response code="403">API client não é um administrador</response>
        [HttpGet]
        [Route("test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public ActionResult Test()
        {
            return Ok("Hello");
        }
        
        /// <summary>
        /// Efetua autenticação de um utilizador Validando as credenciais de login e retorna um token de autenticação para chamadas à API
        /// </summary>
        /// <remarks>
        /// 
        /// Exemplo:
        /// 
        ///     POST /login
        ///     {
        ///         "Username": "baaaaaaaaah",
        ///         "Password": "notSoVerySecurePassword"
        ///     }
        /// 
        /// </remarks>
        /// <returns>
        /// <para>200 OK: Retorna token de autenticação</para>
        /// <para>400 Bad Request: Credenciais de login inválidas</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// </returns>
        /// <response code="200">Returns JWT authentication token</response>
        /// <response code="400">Invalid user or password</response>
        /// <response code="401">Token de autenticação não presente</response>
        /// <response code="403">API client não é um administrador</response>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Authenticate(ApiLoginModel loginRequest)
        {
            /* Autenticação por JWT */
            var identityUser = await _userManager.FindByNameAsync(loginRequest.Username);
            if(identityUser == null)
            {
                return BadRequest("Invalid user or password");
            }
            var resultPassword = await _signInManager.CheckPasswordSignInAsync(identityUser, loginRequest.Password, 
                false);
            
            if(!resultPassword.Succeeded)
            {
                return BadRequest("Invalid user or password");
            }
            
            var token = _tokenService.GenerateToken(identityUser);
            
            return Ok(new { Token = token });
        }
        
        /// <summary>
        /// Remove sessão do utilizador atual (limpa cookies de autenticação)
        /// </summary>
        /// <remarks>
        /// Observação: para tokens JWT, o cliente deve apagar o token localmente, porque tecnicamente permanecem válidos até a data de expiração.        /// </remarks>
        /// <returns>204 No Content: Sessão eliminada</returns>
        /// <para>400 Bad Request: Quando a operação falhou</para>
        /// <response code="204"></response>
        /// <response code="400">Operação falhou</response>
        [HttpPost]
        [Route("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
            
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("Operação falhou");
            }
            
        }
    }
}