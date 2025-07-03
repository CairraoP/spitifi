using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;

namespace spitifi.Models.ApiModels
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MusicController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devolve as 6 músicas mais gostadas
        /// </summary>
        /// <remarks>
        /// Endpoint de livre utilização. Não requer token de autenticação
        /// </remarks>
        /// <returns>
        /// <para>200 OK: Quando lista de músicas forem retornadas</para>
        /// </returns>
        /// <response code="200">Ranking de músicas</response>
        [HttpGet]
        [Route("top6")]
        public async Task<IActionResult> GetTop6Musicas()
        {
            var topMusicas = await _context.Musica
                .Include(m => m.Album)
                .Include(m => m.Dono)
                .Select(m => new 
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    AlbumNome = m.Album.Titulo,
                    ArtistaNome = m.Dono.Username,
                    AlbumFoto = m.Album.Foto, 
                    Likes = m.ListaGostos.Count
                })
                .OrderByDescending(m => m.Likes)
                .Take(6)
                .ToListAsync();

            return Ok(topMusicas);
        }
    }
}