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
    public class LocationsController : ControllerBase
    {
        private readonly DBContext _context;

        public LocationsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationsModel>>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationsModel>> GetLocationsModel(Guid id)
        {
            var locationsModel = await _context.Locations.FindAsync(id);

            if (locationsModel == null)
            {
                return NotFound();
            }

            return locationsModel;
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocationsModel(Guid id, LocationsModel locationsModel)
        {
            if (id != locationsModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(locationsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationsModelExists(id))
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

        // POST: api/Locations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LocationsModel>> PostLocationsModel(LocationsModel locationsModel)
        {
            _context.Locations.Add(locationsModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocationsModel", new { id = locationsModel.ID }, locationsModel);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationsModel(Guid id)
        {
            var locationsModel = await _context.Locations.FindAsync(id);
            if (locationsModel == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(locationsModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationsModelExists(Guid id)
        {
            return _context.Locations.Any(e => e.ID == id);
        }
    }
}
