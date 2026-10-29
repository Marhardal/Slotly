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
    public class ServiceAddonsController : ControllerBase
    {
        private readonly DBContext _context;

        public ServiceAddonsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/ServiceAddons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceAddOnsModel>>> GetServiceAddOns(Guid? tenantId)
        {
            var addOns = _context.ServiceAddOns.AsNoTracking();
            if (tenantId.HasValue)
            {
                addOns = addOns.Where(addOn =>
                    _context.Services.Any(service =>
                        service.ID == addOn.ServiceID && service.TenantID == tenantId.Value));
            }

            return await addOns.ToListAsync();
        }

        // GET: api/ServiceAddons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceAddOnsModel>> GetServiceAddOnsModel(Guid id)
        {
            var serviceAddOnsModel = await _context.ServiceAddOns.FindAsync(id);

            if (serviceAddOnsModel == null)
            {
                return NotFound();
            }

            return serviceAddOnsModel;
        }

        // PUT: api/ServiceAddons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceAddOnsModel(Guid id, ServiceAddOnsModel serviceAddOnsModel)
        {
            if (id != serviceAddOnsModel.ID)
            {
                return BadRequest();
            }

            var existingAddOn = await _context.ServiceAddOns.FindAsync(id);
            if (existingAddOn == null)
            {
                return NotFound();
            }

            if (!await _context.Services.AnyAsync(service => service.ID == serviceAddOnsModel.ServiceID))
            {
                return NotFound($"Service with ID '{serviceAddOnsModel.ServiceID}' was not found.");
            }

            if (await _context.Bookings.AnyAsync(booking => booking.AddOns.Any(addOn => addOn.ID == id)) &&
                (existingAddOn.Name != serviceAddOnsModel.Name ||
                 existingAddOn.Price != serviceAddOnsModel.Price ||
                 existingAddOn.Duration != serviceAddOnsModel.Duration))
            {
                return Conflict("An add-on attached to an existing booking cannot change its price or duration.");
            }

            _context.Entry(serviceAddOnsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceAddOnsModelExists(id))
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

        // POST: api/ServiceAddons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceAddOnsModel>> PostServiceAddOnsModel(ServiceAddOnsModel serviceAddOnsModel)
        {
            if (!await _context.Services.AnyAsync(service => service.ID == serviceAddOnsModel.ServiceID))
            {
                return NotFound($"Service with ID '{serviceAddOnsModel.ServiceID}' was not found.");
            }

            _context.ServiceAddOns.Add(serviceAddOnsModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceAddOnsModel", new { id = serviceAddOnsModel.ID }, serviceAddOnsModel);
        }

        // DELETE: api/ServiceAddons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceAddOnsModel(Guid id)
        {
            var serviceAddOnsModel = await _context.ServiceAddOns.FindAsync(id);
            if (serviceAddOnsModel == null)
            {
                return NotFound();
            }

            if (await _context.Bookings.AnyAsync(booking =>
                    booking.AddOns.Any(addOn => addOn.ID == id)))
            {
                return Conflict("An add-on attached to an existing booking cannot be deleted.");
            }

            _context.ServiceAddOns.Remove(serviceAddOnsModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceAddOnsModelExists(Guid id)
        {
            return _context.ServiceAddOns.Any(e => e.ID == id);
        }
    }
}
