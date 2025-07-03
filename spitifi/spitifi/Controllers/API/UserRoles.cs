using spitifi.Models.ApiModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace spitifi.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
    public class UserRoles : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRoles(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        
        /// <summary>
        /// Adiciona o role de administrador ao utilizador especificado
        /// </summary>
        /// <remarks>
        /// Requer que o utilizador esteja logado e tenha role de administrador
        /// Requer token JWT com claim de Administrado presente
        /// 
        /// Exemplo:
        /// 
        ///     POST /upadmin
        ///     {
        ///         "Username": "examplo"
        ///     }
        /// 
        /// </remarks>
        /// <param name="requestBody">Objeto UserRoleUpdate contendo o username</param>
        /// <returns>
        /// <para>200 OK: Retorna mensagem de sucesso quando a operação é concluída</para>
        /// <para>400 Bad Request: Quando o username não foi especificado</para>
        /// <para>400 Bad Request: Quando a operação falhou</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// <para>404 Not Found: Quando o utilizador especificado não existe</para>
        /// </returns>
        /// <response code="200">Administrador adicionado</response>
        /// <response code="400">Username não especificado</response>
        /// <response code="400">Operação falhou</response>
        /// <response code="401">Token de autenticação não presente</response>
        /// <response code="403">API client não é um administrador</response>
        /// <response code="404">Utilizador não encontrado</response>
        [HttpPost]
        [Route("upadmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> UpgradeToAdmin([FromBody] UserRoleUpdate requestBody)
        {
            // validate input
            if (string.IsNullOrWhiteSpace(requestBody.Username))
            {
                return BadRequest("Username não especificado");
            }
            
            // get user
            var user = await _userManager.FindByNameAsync(requestBody.Username);
            if (user == null)
            {
                return NotFound("Utilizador não encontrado");
            }
            
            // add role
            var result = await _userManager.AddToRoleAsync(user, "Administrador");
            
            // result
            if (!result.Succeeded)
            {
                return BadRequest("Operação falhou");
            }

            return Ok(new { Message = "Administrado adicionado" });
        }
        
        /// <summary>
        /// Retira o role de administrador ao utilizador especificado
        /// </summary>
        /// <remarks>
        /// Requer que o utilizador esteja logado e tenha role de administrador
        /// Requer token JWT com claim de Administrado presente
        /// 
        /// Exemplo:
        /// 
        ///     POST /downadmin
        ///     {
        ///         "Username": "examplo"
        ///     }
        /// 
        /// </remarks>
        /// <param name="requestBody">Objeto UserRoleUpdate contendo o username</param>
        /// <returns>
        /// <para>200 OK: Retorna mensagem de sucesso quando a operação é concluída</para>
        /// <para>400 Bad Request: Quando o username não foi especificado</para>
        /// <para>400 Bad Request: Quando o utilizador especificado não é um administrador</para>
        /// <para>400 Bad Request: Quando a operação falhou</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// <para>404 Not Found: Quando o utilizador especificado não existe</para>
        /// </returns>
        /// <response code="200">Administrador removido</response>
        /// <response code="400">Username não especificado</response>
        /// <response code="400">Utilizador não é um administrador</response>
        /// <response code="400">Operação falhou</response>
        /// <response code="401">Token de autenticação não presente</response>
        /// <response code="403">API client não é um administrador</response>
        /// <response code="404">Utilizador não encontrado</response>
        [HttpDelete]
        [Route("downadmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> DowngradeFromAdmin([FromBody] UserRoleUpdate requestBody)
        {
            // validate input
            if (string.IsNullOrWhiteSpace(requestBody.Username))
            {
                return BadRequest("Username não especificado");
            }
            
            // get user
            var user = await _userManager.FindByNameAsync(requestBody.Username);
            if (user == null)
            {
                return NotFound("Utilizador não encontrado");
            }
            
            // validate if user not an admin
            if (!await _userManager.IsInRoleAsync(user, "Administrador"))
            {
                return BadRequest("Utilizador não é um administrador");
            }

            // remove role
            var result = await _userManager.RemoveFromRoleAsync(user, "Administrador");
            
            // result
            if (!result.Succeeded)
            {
                return BadRequest("Operação falhou");
            }

            return Ok(new { Message = "Administrador removido" });
        }
        
        /// <summary>
        /// Adiciona role de artista ao utilizador especificado
        /// </summary>
        /// <remarks>
        /// Requer que o utilizador esteja logado e tenha role de administrador
        /// Requer token JWT com claim de Administrado presente
        /// 
        /// Exemplo:
        /// 
        ///     POST /isartist
        ///     {
        ///         "Username": "examplo"
        ///     }
        /// 
        /// </remarks>
        /// <param name="requestBody">Objeto UserRoleUpdate contendo o username</param>
        /// <returns>
        /// <para>200 OK: Retorna mensagem de sucesso quando a operação é concluída</para>
        /// <para>400 Bad Request: Quando o username não foi especificado</para>
        /// <para>400 Bad Request: Quando o utilizador especificado não é um administrador</para>
        /// <para>400 Bad Request: Quando a operação falhou</para>
        /// <para>401 Unauthorized: Quando o request não contem um token de autenticação</para>
        /// <para>403 Forbidden: Quando o API Client não possui role de admin</para>
        /// <para>404 Not Found: Quando o utilizador especificado não existe</para>
        /// </returns>
        /// <response code="200">Artista adicionado</response>
        /// <response code="400">Username não especificado</response>
        /// <response code="400">Operação falhou</response>
        /// <response code="401">Token de autenticação não presente</response>
        /// <response code="403">API client não é um administrador</response>
        /// <response code="404">Utilizador não encontrado</response>
        [HttpPut]
        [Route("isartist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> RegisterAsArtist([FromBody] UserRoleUpdate requestBody)
        {
            // validate input
            if (string.IsNullOrWhiteSpace(requestBody.Username))
            {
                return BadRequest("Username não especificado");
            }
            
            // get user
            var user = await _userManager.FindByNameAsync(requestBody.Username);
            if (user == null)
            {
                return NotFound("Utilizador não encontrado");
            }

            // add role
            var result = await _userManager.AddToRoleAsync(user, "Artista");
            
            // result
            if (!result.Succeeded)
            { 
                return BadRequest("Operação falhou");
            }

            return Ok(new { Message = "Artista adicionado" });
        }
    }
}