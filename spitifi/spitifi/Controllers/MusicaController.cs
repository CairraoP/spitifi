using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using spitifi.Data;
using spitifi.Data.DbModels;

namespace spitifi.Controllers
{
    public class MusicaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusicaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Musica
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Musica.Include(m => m.Dono).Include(m=>m.Album);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Musica/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musica = await _context.Musica
                .Include(m => m.Dono)
                .Include(m => m.Album)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (musica == null)
            {
                return NotFound();
            }

            return View(musica);
        }

        // GET: Musica/Create
        public IActionResult Create()
        {
            ViewData["AlbumFK"] = new SelectList(_context.Album, "Id", "Titulo");
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Username");
            return View();
        }

        // POST: Musica/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,AlbumFK,DonoFK")] Musica musica,[Bind("Titulo")] Album album, List<IFormFile> musicaNova, IFormFile fotoAlbum)
        {
            ModelState.Remove("FilePath");
            
            if (ModelState.IsValid)
            {
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Username", musica.DonoFK);
            return View(musica);
        }

        // GET: Musica/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musica = await _context.Musica.FindAsync(id);
            
            if (musica == null)
            {
                return NotFound();
            }
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Username", musica.DonoFK);
            return View(musica);
        }

        // POST: Musica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, [Bind("Id,Nome,Album,FilePath,DonoFK")] Musica musica, IFormFile musicaNova, IFormFile FotoAlbum)
        {
            string nomeAudio = "";
            
            if (id != musica.Id)
            {
                return NotFound();
            }
     
            var musicaAux = _context.Musica.AsNoTracking().FirstOrDefault(m => m.Id == musica.Id);
            musica.FilePath = musicaAux.FilePath;
            
            var fotoAux = _context.Musica.AsNoTracking().FirstOrDefault(m => m.Id == musica.Id);
            // musica.FotoAlbum = fotoAux.FilePath;

            if (id != musica.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musica);
                    await _context.SaveChangesAsync();
                    // se a imagem for diferente da default, vamos apagá-la do disco ao apagar a entrada da BD

                    if (musicaNova == null)
                    {
                    }
                    else
                    {
                        if (!musica.FilePath.Contains("smb_coin.wav"))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot",
                                musica.FilePath);
                            // se o ficheiro/imagem existir apagamos
                            if (System.IO.File.Exists(oldFilePath))
                                System.IO.File.Delete(oldFilePath);
                        }

                        if (!(musicaNova.ContentType == "audio/wav" || musicaNova.ContentType == "audio/mp3"))
                        {
                            ModelState.AddModelError("Ficheiro",
                                "Ficheiro com extensão inválida, use wav ou mp3 por favor");
                            //Futuramente adicionar a PLayList
                            /*ViewData["CategoriaFk"] = new SelectList(_context.Musica, "Id", "Categoria",
                                fotografia.CategoriaFk);*/
                            ViewData["DonoFk"] = new SelectList(_context.Utilizadores, "Id", "Nome", musica.DonoFK);
                            return View(musica);
                        }
                        
                        //criar novo código unico
                        Guid g = Guid.NewGuid();
                        //buscar o novo codigo
                        nomeAudio = g.ToString();
                        //buscar o nome da foto (que ficou associado ao objeto do IFormFile)
                        string restoDoNome = Path.GetExtension(musicaNova.FileName).ToLowerInvariant();
                        nomeAudio += restoDoNome;
                        musica.FilePath = "imagens/" + nomeAudio;


                        // vai construir o path para o diretório onde são guardadas as imagens
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/imagens");

                        // antes de escrevermos o ficheiro, vemos se o diretório existe
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);

                        // atualizamos o Path para incluir o nome da imagem
                        filePath = Path.Combine(filePath, nomeAudio);

                        // escreve a imagem
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await musicaNova.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(musica);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                }

                return RedirectToAction(nameof(Index));
            }

  
            ViewData["DonoFk"] = new SelectList(_context.Utilizadores, "Id", "Nome", musica.DonoFK);
            return View(musica);
        }
        // GET: Musica/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musica = await _context.Musica
                .Include(m => m.Dono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (musica == null)
            {
                return NotFound();
            }

            return View(musica);
        }

        // POST: Musica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musica = await _context.Musica.FindAsync(id);
            if (musica != null)
            {
                _context.Musica.Remove(musica);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusicaExists(int id)
        {
            return _context.Musica.Any(e => e.Id == id);
        }
    }
}
