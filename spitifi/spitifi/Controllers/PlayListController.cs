using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using spitifi.Data;
using spitifi.Data.DbModels;

namespace spitifi.Controllers
{
    public class PlayListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlayList
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PlayList.Include(p => p.Dono);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Username");
            return View();
        }

        // POST: PlayList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DonoFK")] PlayList playList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Username", playList.DonoFK);
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
