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
    public class TenantController : ControllerBase
    {
        private readonly DBContext _context;

        public TenantController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Tenant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TenantModel>>> GetTenants()
        {
            return await _context.Tenants.ToListAsync();
        }

        // GET: api/Tenant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantModel>> GetTenantModel(Guid id)
        {
            var tenantModel = await _context.Tenants.FindAsync(id);

            if (tenantModel == null)
            {
                return NotFound();
            }

            return tenantModel;
        }

        // PUT: api/Tenant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenantModel(Guid id, TenantModel tenantModel)
        {
            if (id != tenantModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(tenantModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantModelExists(id))
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

        // POST: api/Tenant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TenantModel>> PostTenantModel(TenantModel tenantModel)
        {
            _context.Tenants.Add(tenantModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTenantModel", new { id = tenantModel.ID }, tenantModel);
        }

        // DELETE: api/Tenant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenantModel(Guid id)
        {
            var tenantModel = await _context.Tenants.FindAsync(id);
            if (tenantModel == null)
            {
                return NotFound();
            }

            _context.Tenants.Remove(tenantModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TenantModelExists(Guid id)
        {
            return _context.Tenants.Any(e => e.ID == id);
        }
    }
}
