using System;
using System.Collections.Generic;
using System.Text;

namespace PBC.SystemConfiguration.Application.Interfaces
{
    using Microsoft.EntityFrameworkCore;
    using PBC.SystemConfiguration.Application.Dtos;
    using PBC.SystemConfiguration.Domain.Entities;
    using PBC.SystemConfiguration.Infrastructure.Persistence;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class FeatureFlagService : IFeatureFlagService
    {
        private readonly ProgramDbContext _context;

        public FeatureFlagService(ProgramDbContext context)
        {
            _context = context;
        }

        public async Task<List<FeatureFlag>> GetAllAsync()
        {
            return await _context.FeatureFlags.ToListAsync();
        }

        public async Task<FeatureFlag> GetByIdAsync(int id)
        {
            return await _context.FeatureFlags.FindAsync(id);
        }

        public async Task<FeatureFlag> CreateAsync(CreateFeatureFlagDto dto)
        {
            var feature = new FeatureFlag
            {
                Name = dto.Name,
                Description = dto.Description,
                CreateDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow
            };

            _context.FeatureFlags.Add(feature);
            await _context.SaveChangesAsync();
            return feature;
        }

        public async Task<FeatureFlag> UpdateAsync(int id, UpdateFeatureFlagDto dto)
        {
            var feature = await _context.FeatureFlags.FindAsync(id);
            if (feature == null) return null;

            feature.Name = dto.Name;
            feature.Description = dto.Description;
            feature.LastUpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return feature;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var feature = await _context.FeatureFlags.FindAsync(id);
            if (feature == null) return false;

            _context.FeatureFlags.Remove(feature);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FeatureFlag> CreateAsync(CreateFeatureFlagDto dto)
        {
            
            var exists = await _context.FeatureFlags.AnyAsync(f => f.Name == dto.Name);
            if (exists)
                throw new InvalidOperationException("FeatureFlag with this Name already exists.");

            var feature = new FeatureFlag
            {
                Name = dto.Name,
                Description = dto.Description,
                CreateDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow
            };

            _context.FeatureFlags.Add(feature);
            await _context.SaveChangesAsync();
            return feature;
        }

        public async Task<FeatureFlag> UpdateAsync(int id, UpdateFeatureFlagDto dto)
        {
            var feature = await _context.FeatureFlags.FindAsync(id);
            if (feature == null) return null;

            var exists = await _context.FeatureFlags
                .AnyAsync(f => f.Name == dto.Name && f.Id != id);
            if (exists)
                throw new InvalidOperationException("Another FeatureFlag with this Name already exists.");

            feature.Name = dto.Name;
            feature.Description = dto.Description;
            feature.LastUpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return feature;
        }


    }

}
