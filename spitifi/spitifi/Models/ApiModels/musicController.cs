using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;

namespace spitifi.Models.ApiModels
{
    [Route("api/[controller]")]
    [ApiController]
    public class musicController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public musicController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("top6")]
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