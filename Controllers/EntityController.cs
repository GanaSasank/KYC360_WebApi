using KYC360.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<ActionResult<IEnumerable<Entity>>> GetEntities(
            [FromQuery(Name = "search")] string search,
            [FromQuery(Name = "gender")] string gender,
            [FromQuery(Name = "startDate")] DateTime? startDate,
            [FromQuery(Name = "endDate")] DateTime? endDate,
            [FromQuery(Name = "countries")] List<string> countries,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 10,
            [FromQuery(Name = "sortBy")] string sortBy = "Id",
            [FromQuery(Name = "sortOrder")] string sortOrder = "asc")
        {
            IQueryable<Entity> query = _context.Entities
                .Include(e => e.Addresses)
                .Include(e => e.Dates)
                .Include(e => e.Names);

            // Apply filters based on query parameters
            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(e => e.Gender == gender);
            }

            if (startDate != null)
            {
                query = query.Where(e => e.Dates.Any(d => d.EventDate >= startDate));
            }

            if (endDate != null)
            {
                query = query.Where(e => e.Dates.Any(d => d.EventDate <= endDate));
            }

            if (countries != null && countries.Any())
            {
                query = query.Where(e => e.Addresses.Any(a => countries.Contains(a.Country)));
            }

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                query = query.Where(e =>
                    e.Addresses.Any(a =>
                        a.Country.ToLower().Contains(search) ||
                        a.AddressLine.ToLower().Contains(search)
                    ) ||
                    e.Names.Any(n =>
                        n.FirstName.ToLower().Contains(search) ||
                        n.MiddleName.ToLower().Contains(search) ||
                        n.Surname.ToLower().Contains(search)
                    )
                );
            }

            // Apply sorting
            switch (sortBy.ToLower())
            {
                case "gender":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(e => e.Gender) : query.OrderByDescending(e => e.Gender);
                    break;
                case "startdate":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(e => e.Dates.Min(d => d.EventDate)) : query.OrderByDescending(e => e.Dates.Min(d => d.EventDate));
                    break;
                // Add more cases for additional sorting options
                default:
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(e => e.Id) : query.OrderByDescending(e => e.Id);
                    break;
            }

            // Apply pagination
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var entities = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Entities = entities
            };

            return Ok(result);
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
