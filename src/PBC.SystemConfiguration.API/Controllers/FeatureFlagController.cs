using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBC.SystemConfiguration.Application.Dtos;
using PBC.SystemConfiguration.Domain.Entities;
using PBC.SystemConfiguration.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBC.SystemConfiguration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureFlagsController : ControllerBase
    {
        private readonly ProgramDbContext _context;

        public FeatureFlagsController(ProgramDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<FeatureFlagDto>>> GetAll()
        {
            var features = await _context.FeatureFlags.ToListAsync();
            return Ok(features.Select(f => new FeatureFlagDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                CreateDate = f.CreateDate,
                LastUpdateDate = f.LastUpdateDate
            }).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeatureFlagDto>> GetById(int id)
        {
            var feature = await _context.FeatureFlags.FindAsync(id);
            if (feature == null) return NotFound();

            return Ok(new FeatureFlagDto
            {
                Id = feature.Id,
                Name = feature.Name,
                Description = feature.Description,
                CreateDate = feature.CreateDate,
                LastUpdateDate = feature.LastUpdateDate
            });
        }

        [HttpPost]
        public async Task<ActionResult<FeatureFlagDto>> Create(CreateFeatureFlagDto dto)
        {
            
            var exists = await _context.FeatureFlags.AnyAsync(f => f.Name == dto.Name);
            if (exists)
                return BadRequest(new { message = "FeatureFlag with this Name already exists." });

            var feature = new FeatureFlag
            {
                Name = dto.Name,
                Description = dto.Description,
                CreateDate = System.DateTime.UtcNow,
                LastUpdateDate = System.DateTime.UtcNow
            };

            _context.FeatureFlags.Add(feature);
            await _context.SaveChangesAsync();

            var result = new FeatureFlagDto
            {
                Id = feature.Id,
                Name = feature.Name,
                Description = feature.Description,
                CreateDate = feature.CreateDate,
                LastUpdateDate = feature.LastUpdateDate
            };

            return CreatedAtAction(nameof(GetById), new { id = feature.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FeatureFlagDto>> Update(int id, UpdateFeatureFlagDto dto)
        {
            var feature = await _context.FeatureFlags.FindAsync(id);
            if (feature == null) return NotFound();

            
            var exists = await _context.FeatureFlags
                .AnyAsync(f => f.Name == dto.Name && f.Id != id);
            if (exists)
                return BadRequest(new { message = "Another FeatureFlag with this Name already exists." });

            feature.Name = dto.Name;
            feature.Description = dto.Description;
            feature.LastUpdateDate = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new FeatureFlagDto
            {
                Id = feature.Id,
                Name = feature.Name,
                Description = feature.Description,
                CreateDate = feature.CreateDate,
                LastUpdateDate = feature.LastUpdateDate
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var feature = await _context.FeatureFlags.FindAsync(id);
            if (feature == null) return NotFound();

            _context.FeatureFlags.Remove(feature);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
