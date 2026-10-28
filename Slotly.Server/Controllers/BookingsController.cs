using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Slotly.Server;
using Slotly.Server.Seeds;

namespace Slotly.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly DBContext _context;

        public BookingsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingModel>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingModel>> GetBookingModel(Guid id)
        {
            var bookingModel = await _context.Bookings.FindAsync(id);

            if (bookingModel == null)
            {
                return NotFound();
            }

            return bookingModel;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookingModel(Guid id, BookingModel bookingModel)
        {
            if (id != bookingModel.ID)
            {
                return BadRequest();
            }

            if (!await _context.Services.AnyAsync(service => service.ID == bookingModel.ServiceID))
            {
                return NotFound($"Service with ID '{bookingModel.ServiceID}' was not found.");
            }

            if (!await _context.Statuses.AnyAsync(status =>
                    status.ID == bookingModel.statusID && status.Category == Category.Bookings))
            {
                return NotFound($"Booking status with ID '{bookingModel.statusID}' was not found.");
            }

            _context.Entry(bookingModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingModelExists(id))
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

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookingModel>> PostBookingModel(BookingModel bookingModel)
        {
            if (!await _context.Services.AnyAsync(service => service.ID == bookingModel.ServiceID))
            {
                return NotFound($"Service with ID '{bookingModel.ServiceID}' was not found.");
            }

            if (bookingModel.statusID == Guid.Empty)
            {
                var pendingStatus = await _context.Statuses
                    .SingleOrDefaultAsync(status =>
                        status.ID == StatusSeed.PendingId && status.Category == Category.Bookings);

                if (pendingStatus == null)
                {
                    return Problem(
                        detail: "The default Pending booking status is missing.",
                        statusCode: StatusCodes.Status500InternalServerError);
                }

                bookingModel.statusID = pendingStatus.ID;
            }
            else if (!await _context.Statuses.AnyAsync(status =>
                         status.ID == bookingModel.statusID && status.Category == Category.Bookings))
            {
                return NotFound($"Booking status with ID '{bookingModel.statusID}' was not found.");
            }

            _context.Bookings.Add(bookingModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookingModel", new { id = bookingModel.ID }, bookingModel);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingModel(Guid id)
        {
            var bookingModel = await _context.Bookings.FindAsync(id);
            if (bookingModel == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(bookingModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingModelExists(Guid id)
        {
            return _context.Bookings.Any(e => e.ID == id);
        }
    }
}
