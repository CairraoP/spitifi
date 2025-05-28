using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;
using spitifi.Data.DbModels;
using spitifi.Models;

namespace spitifi.Controllers
{
    public class AlbumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AlbumController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Album
        public async Task<IActionResult> Index(
            int? pageNumber,   
            int pageSize )
        {
            
            if (pageSize == 0)
            {
                pageSize = 5;
            }
            var applicationDbContext = _context.Musica.
                Include(m => m.Dono).
                Include(m=>m.Album);
            
            // Create base query with includes
            var query = _context.Album.AsQueryable();
    
            // Apply pagination using Page.CreateAsync
            var paginatedResult = await Page<Album>.CreateAsync(
                source: query,
                pageIndex: pageNumber ?? 1,  //página 1 caso pageNumber 0
                pageSize: pageSize
            );
            
            return View(paginatedResult);
            
        }

        // GET: Album/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Musicas)
                .Include(a => a.Dono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Album/Create
        // Confirmar qual o nome da role para um artista
        // TO DO - Alterar/Criar novo metodo de Create para os Users, Este ou fica para os artistas ou altera-se
        [Authorize]
        public IActionResult Create()
        {
            Console.WriteLine("xungas2");
            var userId = _userManager.GetUserId(User);
            ViewData["DonoNome"] = _context.Utilizadores.FirstOrDefault(u => u.IdentityUser == userId).Username;
            return View();
        }

        // POST: Album/Create
        //
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Titulo")] Album album,
            IFormFile fotoAlbum, List<IFormFile> musicasNovas)
        {
            Console.WriteLine("xungas1");
            //ModelState.Remove("DonoFK");
            //variaveis para validações

            var utilizadorAux = _context.Users.First(au => au.UserName == User.Identity.Name);
            var utlizador = _context.Utilizadores.Where(u => u.IdentityUser == utilizadorAux.Id);

            bool haImagem = false;
            string nomeImagem = "";
            string nomeMusica = "";

            //var fotoAux = _context.Album.FirstOrDefault(a=>a.Id == album.Id);
            //album.Foto = fotoAux.Foto;

            if (!utlizador.Any())
            {
                ModelState.AddModelError("DonoFK", "Alteração incorreta do Dono");
            }

            if (musicasNovas == null)
                ModelState.AddModelError("FilePath", "Não introduziste pelo menos uma musica/ficheiro");

            //primeiro criaremos o album e a sua foto e depois as musicas
            if (ModelState.IsValid)
            {
                List<string> arrayPathMusicas = new List<string>();
                var fotoDeleteAlbum = "";

                try
                {
                    album.DonoFK = utlizador.First().Id;

                    foreach (var file in musicasNovas)
                    {
                        if (!file.ContentType.StartsWith("audio"))
                        {
                            ModelState.AddModelError("",
                                "Uma ou mais músicas com extensão inválida, use .wav ou .mp3 por favor");
                            var userId = _userManager.GetUserId(User);
                            ViewData["DonoNome"] = _context.Utilizadores.FirstOrDefault(u => u.IdentityUser == userId)
                                .Username;
                            return View();
                        }
                    }

                    if (!(fotoAlbum.ContentType == "image/png" || fotoAlbum.ContentType == "image/jpeg"))
                    {
                        ModelState.AddModelError("", "Formato Inválido. Insira uma foto com formato JPEG ou PNG");
                        var userId = _userManager.GetUserId(User);
                        ViewData["DonoNome"] =
                            _context.Utilizadores.FirstOrDefault(u => u.IdentityUser == userId).Username;
                        return View();
                    }

                    {
                        haImagem = true;
                        // gerar nome imagem
                        Guid g = Guid.NewGuid();
                        // atrás do nome adicionamos a pasta onde a escrevemos
                        nomeImagem = g.ToString();
                        string extensaoImagem = Path.GetExtension(fotoAlbum.FileName).ToLowerInvariant();
                        nomeImagem += extensaoImagem;
                        // guardar o nome do ficheiro na BD
                        album.Foto = "imagens/" + nomeImagem;
                    }

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
                        //guardar o caminho da imagem para caso algo corra mal, possamos eliminar a foto da bd no nosso bloco de catch
                        fotoDeleteAlbum = filePath;

                        // escreve a imagem
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await fotoAlbum.CopyToAsync(fileStream);
                        }

                        //Criação das músicas
                        foreach (var formFile in musicasNovas)
                        {
                            // gerar nome imagem
                            Guid g = Guid.NewGuid();
                            // atrás do nome adicionamos a pasta onde a escrevemos
                            nomeMusica = g.ToString();
                            string extensaoMusica = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                            nomeMusica += extensaoMusica;
                            // vai construir o path para o diretório onde são guardadas as imagens
                            var filePathMusica = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/musicas");

                            // antes de escrevermos o ficheiro, vemos se o diretório existe
                            if (!Directory.Exists(filePathMusica))
                                Directory.CreateDirectory(filePathMusica);

                            // atualizamos o Path para incluir o nome da imagem
                            filePathMusica = Path.Combine(filePathMusica, nomeMusica);

                            // escreve a imagem
                            using (var fileStream = new FileStream(filePathMusica, FileMode.Create))
                            {
                                await formFile.CopyToAsync(fileStream);
                            }

                            //adicionar ao array para eliminar as musicas no bloco catch caso algo falhe na criação
                            arrayPathMusicas.Add(filePathMusica);

                            var novaMusica = new Musica
                                { DonoFK = album.DonoFK, Nome = formFile.FileName, FilePath = "musicas/" + nomeMusica };

                            album.Musicas.Add(novaMusica);

                            //usar o using para executar o bloco de código para executar aquela ação, é despejado assim que executado e não espera até ao final do controller
                            using (var fileStream = new FileStream(filePathMusica, FileMode.Create))
                            {
                                await formFile.CopyToAsync(fileStream);
                            }
                        }
                    }

                    _context.Add(album);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    Console.WriteLine("erro create");
                    ModelState.AddModelError("", "Algo correu mal, por favor tente novamente");

                    if (System.IO.File.Exists(fotoDeleteAlbum))
                    {
                        System.IO.File.Delete(fotoDeleteAlbum);
                    }

                    foreach (var musicaDelete in arrayPathMusicas)
                    {
                        if (System.IO.File.Exists(musicaDelete))
                        {
                            System.IO.File.Delete(musicaDelete);
                        }
                    }

                    throw;
                }
            }

            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Username", album.DonoFK);
            return View(album);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Foto")] Album album)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Id))
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

            return View(album);
        }

        // GET: Album/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Album.FindAsync(id);
            if (album != null)
            {
                _context.Album.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Album.Any(e => e.Id == id);
        }
    }
}