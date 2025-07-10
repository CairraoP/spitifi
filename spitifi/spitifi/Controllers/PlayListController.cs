using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;
using spitifi.Models;
using spitifi.Models.DbModels;

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
                .Include(p => p.Musicas).ThenInclude(m => m.Album)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (playList == null)
            {
                return NotFound();
            }

            return View(playList);
        }

        // GET: PlayList/Create
        [Authorize(Roles = "Artista, Administrador")]
        public IActionResult Create()
        {
            
            var musicDataForSelectList = _context.Musica
                .Select(m => new { Id = m.Id, Name = m.Nome })
                .ToList();

            
            ViewBag.MusicaList = new MultiSelectList(musicDataForSelectList, "Id", "Name");

            ViewBag.ListaMusicas = _context.Musica.OrderBy(m => m.Nome).ToList();

            return View(new PlayList());
        }
        

        // POST: PlayList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Artista, Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            PlayList playList,
            List<int> selectedMusicas, // Matches select name
            IFormFile fotoPlaylist)
        {
            bool haImagem = false;
            string nomeImagem = "";
            string fotoDeletePlaylist = "";

            if (fotoPlaylist == null)
            {
                ModelState.AddModelError("Foto", "Não foi inserida nenhuma foto");
                return View();
            }

            if (string.IsNullOrEmpty(playList.Nome))
            {
                ModelState.AddModelError("Nome", "Não foi inserido nenhum Nome para a Playlist");
                return View();
            }

            if (ModelState.IsValid)
            {
                // utilizador atual
                var utilizador = await _context.Utilizadores
                    .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

                // definir criador para playlist
                playList.DonoFK = utilizador.Id;

                // processar as músicas escolhidas
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
                playList.Foto = "imagens/" + nomeImagem;
            }

            
            try
            {
                // se existe uma imagem para escrever no disco
                if (haImagem)
                {
                    // vai construir o path para o diretório onde são guardadas as imagens
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/imagens");

                    // antes de escrevermos o ficheiro, vemos se o diretório existe
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    // atualizamos o Path para incluir o nome da imagem
                    filePath = Path.Combine(filePath, nomeImagem);

                    fotoDeletePlaylist = filePath;

                    // escreve a imagem
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fotoPlaylist.CopyToAsync(fileStream);
                    }

                    _context.Add(playList);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Algo correu mal, por favor tente novamente");

                if (System.IO.File.Exists(fotoDeletePlaylist))
                {
                    System.IO.File.Delete(fotoDeletePlaylist);
                }

                throw;
            }

            return View(playList);
        }

        // GET: PlayList/Edit/5
        [Authorize(Roles = "Artista, Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playList = await _context.PlayList
                .Include(pl => pl.Musicas)
                .FirstOrDefaultAsync(pl => pl.Id == id);
            
            if (playList == null)
            {
                return NotFound();
            }

            ViewBag.ListaMusicas = _context.Musica.OrderBy(m => m.Nome).ToList();
            
            return View(playList);
        }

        // POST: PlayList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Artista, Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] PlayList playList, int[]  musicasSelecionadas)
        {
            if (id != playList.Id)
            {
                return NotFound();
            }
            
            var playlistValidacaoAuthorization = await _context.PlayList.Include(a => a.Dono).FirstAsync(a => a.Id == id);

            if (playlistValidacaoAuthorization?.Dono?.IdentityUser == null)
            {
                // utilizador não pôde ser verificado
                return RedirectToAction(nameof(Index));
            }
            
            // validar se quem tenta alterar a playlist é o dono or admin
            if (playlistValidacaoAuthorization.Dono.IdentityUser != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Administrador") )
            {
                return Forbid(); 
            }
            
            var playlist = await _context.PlayList.Where(pl => pl.Id == id)
                                                .Include(pl => pl.Musicas)
                                                .FirstOrDefaultAsync();
            
            var oldPlaylist = playlist.Musicas.Select(m => m.Id).ToList();
            
            //avaliar se o utilizador retirou ou adicionou musicas à nossa playlist
            var adicionadas = musicasSelecionadas.Except(oldPlaylist);
            var retiradas = oldPlaylist.Except(musicasSelecionadas.ToList());
            
            // se alguma musica foi adicionada ou retirada
            // é necessário alterar a lista de musicas na playlist 
            
            if (adicionadas.Any() || retiradas.Any()) {

                if (retiradas.Any()) {
                    // retirar a Category 
                    foreach (int oldMusica in retiradas) {
                        var musicToRemove = playlist.Musicas.FirstOrDefault(c => c.Id == oldMusica);
                        playlist.Musicas.Remove(musicToRemove);
                    }
                }
                if (adicionadas.Any()) {
                    // adicionar a Category 
                    foreach (int newMusic in adicionadas) {
                        var musicToAdd = await _context.Musica.FirstOrDefaultAsync(pl => pl.Id == newMusic);
                        playlist.Musicas.Add(musicToAdd);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /* a EF só permite a manipulação de um único objeto de um mesmo tipo
                     *  por esse motivo, como estamos a usar o objeto 'lesson'
                     *  temos de o atualizar com os dados que vêm da View
                     */
                    playlist.Nome = playList.Nome;
                    
                    //atualizar o nosso contexto
                    _context.Update(playlist);
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
        [Authorize(Roles = "Artista, Administrador")]
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
        [HttpPost, ActionName("Delete")] // Respond to view HTTP POST and map to asp-action "Delete"
        [Authorize(Roles = "Artista, Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var playList = await _context.PlayList.Include(a => a.Dono).FirstAsync(a => a.Id == id);
            
            if (playList?.Dono?.IdentityUser == null)
            {
                // utilizador não pôde ser verificado
                return RedirectToAction(nameof(Index));
            }
            
            // validar se quem tenta alterar a playlist é o dono or admin
            if (playList.Dono.IdentityUser != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Administrador") )
            {
                return Forbid(); 
            }
            
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