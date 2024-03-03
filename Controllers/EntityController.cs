using KYC360.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace KYC360.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EntityController : ControllerBase
    {
        private readonly Kyc360DbContext _context;

        public EntityController(Kyc360DbContext context)
        {
            _context = context;
        }

        // GET: api/Entity
        [HttpGet]
        public ActionResult<IEnumerable<Entity>> GetEntities()
        {
            return _context.Entities.Include(e => e.Addresses).Include(e => e.Dates).Include(e => e.Names).ToList();
        }

        // GET: api/Entity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entity>> GetEntity(string id)
        {
            var entity = await _context.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        // POST: api/Entity
        [HttpPost]
        public async Task<ActionResult<Entity>> CreateEntity(Entity entity)
        {
            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntity", new { id = entity.Id }, entity);
        }

        // PUT: api/Entity/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntity(string id, Entity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(id))
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

        // DELETE: api/Entity/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(string id)
        {
            var entity = await _context.Entities.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.Entities.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntityExists(string id)
        {
            return _context.Entities.Any(e => e.Id == id);
        }
    }


}
