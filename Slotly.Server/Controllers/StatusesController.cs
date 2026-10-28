using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Slotly.Server.Seeds;

namespace Slotly.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly DBContext _context;

        public StatusesController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusModel>>> GetStatuses()
        {
            return await _context.Statuses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StatusModel>> GetStatus(Guid id)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return status;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(Guid id, StatusModel status)
        {
            if (id != status.ID)
            {
                return BadRequest();
            }

            var existingStatus = await _context.Statuses.FindAsync(id);
            if (existingStatus == null)
            {
                return NotFound();
            }

            if (existingStatus.Category != status.Category &&
                await _context.Bookings.AnyAsync(booking => booking.statusID == id))
            {
                return Conflict("A status assigned to bookings cannot change category.");
            }

            _context.Entry(status).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Statuses.AnyAsync(existingStatus => existingStatus.ID == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<StatusModel>> PostStatus(StatusModel status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStatus), new { id = status.ID }, status);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(Guid id)
        {
            var status = await _context.Statuses.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            if (id == StatusSeed.PendingId)
            {
                return Conflict("The default Pending status cannot be deleted.");
            }

            if (await _context.Bookings.AnyAsync(booking => booking.statusID == id))
            {
                return Conflict("A status assigned to bookings cannot be deleted.");
            }

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
