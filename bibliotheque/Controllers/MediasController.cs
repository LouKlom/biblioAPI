using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bibliotheque.Models;

namespace bibliotheque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediasController : ControllerBase
    {
        private readonly MediasContext _context;

        public MediasController(MediasContext context)
        {
            _context = context;
        }

        // GET: api/Medias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medias>>> GetMedias()
        {
            return await _context.Medias.ToListAsync();
        }

        // GET: api/Medias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medias>> GetMedias(int id)
        {
            var medias = await _context.Medias.FindAsync(id);

            if (medias == null)
            {
                return NotFound();
            }

            return medias;
        }

        // PUT: api/Medias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedias(int id, Medias medias)
        {
            if (id != medias.Id)
            {
                return BadRequest();
            }

            _context.Entry(medias).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Medias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Medias>> PostMedias(Medias medias)
        {
            _context.Medias.Add(medias);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedias", new { id = medias.Id }, medias);
        }

        // DELETE: api/Medias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedias(int id)
        {
            var medias = await _context.Medias.FindAsync(id);
            if (medias == null)
            {
                return NotFound();
            }

            _context.Medias.Remove(medias);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MediasExists(int id)
        {
            return _context.Medias.Any(e => e.Id == id);
        }
    }
}
