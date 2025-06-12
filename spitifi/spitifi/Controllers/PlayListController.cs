using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;
using spitifi.Data.DbModels;
using spitifi.Models;

namespace spitifi.Controllers
{
    public class PlayListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PlayListController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PlayList
        public async Task<IActionResult> Index( int? pageNumber,   
            int pageSize )
        {
            if (pageSize == 0)
            {
                pageSize = 5;
            }
            var applicationDbContext = _context.PlayList
                .Include(m => m.Dono).ToList();
            
            // Create base query with includes
            var query = _context.PlayList.AsQueryable();
    
            // Apply pagination using Page.CreateAsync
            var paginatedResult = await Page<PlayList>.CreateAsync(
                source: query,
                pageIndex: pageNumber ?? 1,  //página 1 caso pageNumber 0
                pageSize: pageSize
            );
            
            return View(paginatedResult);
        }

        // GET: PlayList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playList = await _context.PlayList
                .Include(p => p.Dono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playList == null)
            {
                return NotFound();
            }

            return View(playList);
        }

        // GET: PlayList/Create
        public IActionResult Create()
        {
            // Fetch music data from database
            var musicDataForSelectList = _context.Musica
                .Select(m => new { Id = m.Id, Name = m.Nome })
                .ToList();

            // Create SelectList for dropdown
            ViewBag.MusicaList = new MultiSelectList(musicDataForSelectList, "Id", "Name");

            return View();
        }

        // POST: PlayList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            PlayList playList,
            List<int> selectedMusicas, // Matches select name
            IFormFile fotoPlaylist)
        {
            bool haImagem = false;
            string nomeImagem = "";
            
            if (ModelState.IsValid)
            {
                // Get current Identity user
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser == null)
                {
                    // Handle unauthenticated user
                    return Challenge(); // Redirects to login page
                }

                // Find matching Utilizadores record
                var utilizador = await _context.Utilizadores
                    .FirstOrDefaultAsync(u => u.IdentityUser == currentUser.Id);

                if (utilizador == null)
                {
                    // Create new Utilizadores record
                    utilizador = new Utilizadores
                    {
                        IdentityUser = currentUser.Id,
                        Username = currentUser.UserName,
                        IsArtista = false
                    };

                    _context.Utilizadores.Add(utilizador);
                    await _context.SaveChangesAsync();
                }

                // Set playlist owner
                playList.DonoFK = utilizador.Id;

                // Process selected songs
                if (selectedMusicas != null && selectedMusicas.Any())
                {
                    var selectedMusicasList = await _context.Musica
                        .Where(m => selectedMusicas.Contains(m.Id))
                        .ToListAsync();

                    playList.Musicas = selectedMusicasList;
                }
              
                haImagem = true;
                // gerar nome imagem
                Guid g = Guid.NewGuid();
                // atrás do nome adicionamos a pasta onde a escrevemos
                nomeImagem = g.ToString();
                string extensaoImagem = Path.GetExtension(fotoPlaylist.FileName).ToLowerInvariant();
                nomeImagem += extensaoImagem;
                // guardar o nome do ficheiro na BD
                playList.Foto = "playlist/" + nomeImagem;
            }

            // se existe uma imagem para escrever no disco
            if (haImagem)
            {
                // vai construir o path para o diretório onde são guardadas as imagens
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/playlist");

                // antes de escrevermos o ficheiro, vemos se o diretório existe
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                // atualizamos o Path para incluir o nome da imagem
                filePath = Path.Combine(filePath, nomeImagem);
    

                // escreve a imagem
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fotoPlaylist.CopyToAsync(fileStream);
                }


                _context.Add(playList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(playList);
        }

        // GET: PlayList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playList = await _context.PlayList.FindAsync(id);
            if (playList == null)
            {
                return NotFound();
            }

            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Username", playList.DonoFK);
            return View(playList);
        }

        // POST: PlayList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DonoFK")] PlayList playList)
        {
            if (id != playList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayListExists(playList.Id))
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

            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Username", playList.DonoFK);
            return View(playList);
        }

        // GET: PlayList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playList = await _context.PlayList
                .Include(p => p.Dono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playList == null)
            {
                return NotFound();
            }

            return View(playList);
        }

        // POST: PlayList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playList = await _context.PlayList.FindAsync(id);
            if (playList != null)
            {
                _context.PlayList.Remove(playList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayListExists(int id)
        {
            return _context.PlayList.Any(e => e.Id == id);
        }
    }
}