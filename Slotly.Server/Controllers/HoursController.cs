using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Slotly.Server;

namespace Slotly.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoursController : ControllerBase
    {
        private readonly DBContext _context;

        public HoursController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Hours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationHoursModel>>> GetHours()
        {
            return await _context.Hours.ToListAsync();
        }

        // GET: api/Hours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationHoursModel>> GetLocationHoursModel(Guid id)
        {
            var locationHoursModel = await _context.Hours.FindAsync(id);

            if (locationHoursModel == null)
            {
                return NotFound();
            }

            return locationHoursModel;
        }

        // PUT: api/Hours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocationHoursModel(Guid id, LocationHoursModel locationHoursModel)
        {
            if (id != locationHoursModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(locationHoursModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationHoursModelExists(id))
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

        // POST: api/Hours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LocationHoursModel>> PostLocationHoursModel(LocationHoursModel locationHoursModel)
        {
            _context.Hours.Add(locationHoursModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocationHoursModel", new { id = locationHoursModel.Id }, locationHoursModel);
        }

        // DELETE: api/Hours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationHoursModel(Guid id)
        {
            var locationHoursModel = await _context.Hours.FindAsync(id);
            if (locationHoursModel == null)
            {
                return NotFound();
            }

            _context.Hours.Remove(locationHoursModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationHoursModelExists(Guid id)
        {
            return _context.Hours.Any(e => e.Id == id);
        }
    }
}
