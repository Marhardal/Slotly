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
    public class ServicesController : ControllerBase
    {
        private readonly DBContext _context;

        public ServicesController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceModel>>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceModel>> GetServiceModel(Guid id)
        {
            var serviceModel = await _context.Services.FindAsync(id);

            if (serviceModel == null)
            {
                return NotFound();
            }

            return serviceModel;
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceModel(Guid id, ServiceModel serviceModel)
        {
            if (id != serviceModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(serviceModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceModelExists(id))
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

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceModel>> PostServiceModel(ServiceModel serviceModel)
        {
            _context.Services.Add(serviceModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceModel", new { id = serviceModel.ID }, serviceModel);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceModel(Guid id)
        {
            var serviceModel = await _context.Services.FindAsync(id);
            if (serviceModel == null)
            {
                return NotFound();
            }

            _context.Services.Remove(serviceModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceModelExists(Guid id)
        {
            return _context.Services.Any(e => e.ID == id);
        }
    }
}
