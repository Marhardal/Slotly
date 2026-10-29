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
        public async Task<ActionResult<IEnumerable<BookingModel>>> GetBookings(Guid tenantId)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(booking => booking.AddOns)
                .Where(booking => booking.TenantID == tenantId)
                .OrderBy(booking => booking.Date)
                .ThenBy(booking => booking.Time)
                .ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingModel>> GetBookingModel(Guid id)
        {
            var bookingModel = await _context.Bookings
                .AsNoTracking()
                .Include(booking => booking.AddOns)
                .SingleOrDefaultAsync(booking => booking.ID == id);

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

            var existingBooking = await _context.Bookings.SingleOrDefaultAsync(booking =>
                booking.ID == id && booking.TenantID == bookingModel.TenantID);
            if (existingBooking == null)
            {
                return NotFound();
            }

            if (!await _context.Statuses.AnyAsync(status =>
                    status.ID == bookingModel.statusID && status.Category == Category.Bookings))
            {
                return NotFound($"Booking status with ID '{bookingModel.statusID}' was not found.");
            }

            existingBooking.statusID = bookingModel.statusID;
            existingBooking.updatedOn = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookingModel>> PostBookingModel(BookingModel bookingModel)
        {
            var service = await _context.Services.SingleOrDefaultAsync(service =>
                service.ID == bookingModel.ServiceID && service.TenantID == bookingModel.TenantID);
            if (service == null)
            {
                return NotFound($"Service with ID '{bookingModel.ServiceID}' was not found for this tenant.");
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

            var requestedAddOnIds = bookingModel.AddOnIDs.Distinct().ToArray();
            var addOns = await _context.ServiceAddOns
                .Where(addOn => addOn.ServiceID == service.ID && requestedAddOnIds.Contains(addOn.ID))
                .ToListAsync();
            if (addOns.Count != requestedAddOnIds.Length)
            {
                return BadRequest("One or more selected add-ons do not belong to the selected service.");
            }

            bookingModel.AddOns = addOns;
            bookingModel.Price = service.Price + addOns.Sum(addOn => addOn.Price);
            bookingModel.Duration = service.Duration + addOns.Sum(addOn => addOn.Duration);
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

    }
}
