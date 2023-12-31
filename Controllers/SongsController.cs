﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment_5.Data;
using Assignment_5.Models;

namespace Assignment_5.Controllers
{
    public class SongsController : Controller
    {
        private readonly Assignment_5Context _context;

        public SongsController(Assignment_5Context context)
        {
            _context = context;
        }

        // GET: Songs
        public async Task<IActionResult> Index(string songGenre, string songPerformer)
        {
            if (_context.Song == null)
            {
                return Problem("Entity set 'Assignment_5Context.Song'  is null.");
            }

            // Use LINQ to get list of genre or performers
            IQueryable<string> genreQuery = from s in _context.Song
                                            orderby s.Genre
                                            select s.Genre;

            IQueryable<string> performerQuery = from s in _context.Song
                                                orderby s.Performer
                                                select s.Performer;

            var songs = from s in _context.Song select s;

            if (!string.IsNullOrEmpty(songGenre))
            {
                songs = songs.Where(x => x.Genre == songGenre);
            }

            if (!string.IsNullOrEmpty(songPerformer))
            {
                songs = songs.Where(x => x.Performer == songPerformer);
            }

            var songVM = new SongViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Performer = new SelectList(await performerQuery.Distinct().ToListAsync()),
                Songs = await songs.ToListAsync()
            };

            return View(songVM);
        }

        // GET: Songs/Admin
        public async Task<IActionResult> Admin()
        {
            return _context.Song != null ?
                        View(await _context.Song.ToListAsync()) :
                        Problem("Entity set 'Assignment_5Context.Song'  is null.");
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Song == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Performer,Genre,Price")] Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(song);
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Song == null)
            {
                return NotFound();
            }

            var song = await _context.Song.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Performer,Genre,Price")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.Id))
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
            return View(song);
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Song == null)
            {
                return NotFound();
            }

            var song = await _context.Song
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Song == null)
            {
                return Problem("Entity set 'Assignment_5Context.Song'  is null.");
            }
            var song = await _context.Song.FindAsync(id);
            if (song != null)
            {
                _context.Song.Remove(song);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
          return (_context.Song?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
