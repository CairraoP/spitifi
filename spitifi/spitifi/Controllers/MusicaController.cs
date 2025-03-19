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
            var applicationDbContext = _context.Musica.Include(m => m.Dono);
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
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Username");
            return View();
        }

        // POST: Musica/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Album,DonoFK")] Musica musica, IFormFile musicaFile)
        {
            //ModelState.Remove("Dono");
            ModelState.Remove("FilePath");
            
            if(musicaFile== null)
                ModelState.AddModelError("FilePath", "Não introduziste um ficheiro");
            
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(Guid.NewGuid().ToString()+ "-"+ musicaFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/", fileName);
                //usar o using para executar o bloco de código para executar aquela ação, é despejado assim que executado e não espera até ao final do controller
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await musicaFile.CopyToAsync(fileStream);
                }
                musica.FilePath = fileName;
                _context.Add(musica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Id", musica.DonoFK);
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
            
            HttpContext.Session.SetInt32("musicaId", musica.Id);
            
            if (musica == null)
            {
                return NotFound();
            }
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Id", musica.DonoFK);
            return View(musica);
        }

        // POST: Musica/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, [Bind("Id,Nome,Album,FilePath,DonoFK")] Musica musica)
        {
            if (id != musica.Id)
            {
                return NotFound();
            }
            
            var musicaDaSessao= HttpContext.Session.GetInt32("musicaId");
            if (musicaDaSessao != musica.Id)
            {
                ModelState.AddModelError("Id", "Não é permitido fazer tal operação");
                return View();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicaExists(musica.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("Id", "Esta música não existe");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Nome", musica.DonoFK);
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
