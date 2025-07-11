using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;
using spitifi.Models.DbModels;
using spitifi.Services.AlbumEraser;

namespace spitifi.Controllers
{
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private AlbumEraser _AlbumEraser;
        private readonly UserManager<IdentityUser> _userManager;

        public UtilizadoresController(ApplicationDbContext context, AlbumEraser albumEraser, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _AlbumEraser = albumEraser;
            _userManager = userManager;
        }

        // GET: Utilizadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilizadores.ToListAsync());
        }

        // GET: Utilizadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var utilizadores = await _context.Utilizadores
                .Include(u => u.Albums).ThenInclude(m => m.Musicas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            return View(utilizadores);
        }

        // GET: Utilizadores/Create
        [Authorize(Roles="Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles="Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username")] Utilizadores utilizador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador);
        }

        // GET: Utilizadores/Edit/5
        [Authorize(Roles="Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var utilizadores = await _context.Utilizadores.FindAsync(id);
            if (utilizadores == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetInt32("utilizadorId", utilizadores.Id);
            return View(utilizadores);
        }

        // POST: Utilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles="Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, [Bind("Id,Username")] Utilizadores utilizador)
        {
            if (id != utilizador.Id)
            {
                return NotFound();
            }
            var utilizadorIdDaSessao = HttpContext.Session.GetInt32("utilizadorId");

            if (utilizadorIdDaSessao != id)
            {
                ModelState.AddModelError("", "Id inválido, por favor não mexa nas ferramentas do browser");
                return View(utilizador);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadoresExists(utilizador.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador);
        }

        // GET: Utilizadores/Delete/5
        [Authorize(Roles="Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            return View(utilizadores);
        }

        // POST: Utilizadores/Delete/5
        [HttpPost, ActionName("Delete")] // Respond to view HTTP POST and map to asp-action "Delete"
        [Authorize(Roles="Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var utilizador = await _context.Utilizadores
                .Include(u => u.Albums)
                  .ThenInclude(u => u.Musicas)
                .Include(u => u.Playlists)
                .FirstOrDefaultAsync(u => u.Id == id);

            var utilizadorIdentity = await _userManager.FindByNameAsync(utilizador.Username);
            
            if (utilizador != null)
            {
                foreach (PlayList playList in utilizador.Playlists)
                {
                    _context.Remove(playList);
                }

                foreach (Album album in utilizador.Albums)
                {
                    await _AlbumEraser.AlbumEraserFunction(album.Id);
                }
                _context.Utilizadores.Remove(utilizador);
                await _context.SaveChangesAsync();
                
                await _userManager.DeleteAsync(utilizadorIdentity);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.Id == id);
        }

    }
}
