using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using spitifi.Data;
using spitifi.Models;
using spitifi.Services.AlbumEraser;

namespace spitifi.Controllers;

public class ProcuraController : Controller
{
    
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    public ProcuraController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment,  AlbumEraser albumEraser)
    {
        _context = context;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(string searchTerm)
    {
        var viewModel = new Procura() { TermoDeProcura = searchTerm };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // procurar album
            viewModel.Albums = await _context.Album
                .Include(a => a.Dono)
                .Where(a => a.Titulo.Contains(searchTerm))
                .ToListAsync();

            // procurar musicas
            viewModel.Musicas = await _context.Musica
                .Include(m => m.Album)
                .Include(m => m.Dono)
                .Where(m => m.Nome.Contains(searchTerm))
                .ToListAsync();

            // Search Artists (only users marked as artists)
            // procurar artista
            viewModel.Artistas = await _context.Utilizadores
                .Where(u => u.IsArtista && u.Username.Contains(searchTerm))
                .ToListAsync();
        }

        return View(viewModel);
    }
}