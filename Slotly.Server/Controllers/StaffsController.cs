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
    public class StaffsController : ControllerBase
    {
        private readonly DBContext _context;

        public StaffsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Staffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationStaffsModel>>> GetStaffs()
        {
            return await _context.Staffs.ToListAsync();
        }

        // GET: api/Staffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationStaffsModel>> GetLocationStaffsModel(Guid id)
        {
            var locationStaffsModel = await _context.Staffs.FindAsync(id);

            if (locationStaffsModel == null)
            {
                return NotFound();
            }

            return locationStaffsModel;
        }

        // PUT: api/Staffs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocationStaffsModel(Guid id, LocationStaffsModel locationStaffsModel)
        {
            if (id != locationStaffsModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(locationStaffsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationStaffsModelExists(id))
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

        // POST: api/Staffs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LocationStaffsModel>> PostLocationStaffsModel(LocationStaffsModel locationStaffsModel)
        {
            _context.Staffs.Add(locationStaffsModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocationStaffsModel", new { id = locationStaffsModel.Id }, locationStaffsModel);
        }

        // DELETE: api/Staffs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationStaffsModel(Guid id)
        {
            var locationStaffsModel = await _context.Staffs.FindAsync(id);
            if (locationStaffsModel == null)
            {
                return NotFound();
            }

            _context.Staffs.Remove(locationStaffsModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationStaffsModelExists(Guid id)
        {
            return _context.Staffs.Any(e => e.Id == id);
        }
    }
}
