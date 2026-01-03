using Microsoft.Extensions.Caching.Memory;
using PBC.SystemConfiguration.Application.Dtos.FeatureFlag;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Entities;
using PBC.SystemConfiguration.Domain.Exceptions;
using PBC.SystemConfiguration.Domain.Interfaces;

namespace PBC.SystemConfiguration.Application.Services;

public class FeatureFlagService(IFeatureFlagRepository repository, IMemoryCache cache) : IFeatureFlagService
{
    private const string CacheKeyPrefix = "featureflag_";
    private const string FeatureFlagName = "Feature Flag";

    public async Task<IEnumerable<FeatureFlagDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new FeatureFlagDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            IsEnabled = e.IsEnabled,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        });
    }

    public async Task<FeatureFlagDto> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{CacheKeyPrefix}{name}";

        return await cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
            entry.SlidingExpiration = TimeSpan.FromMinutes(15);

            var entity = await repository.FindOneAsync(x => x.Name == name, cancellationToken);
            if (entity == null) throw new ObjectNotFoundException(FeatureFlagName);

            return new FeatureFlagDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsEnabled = entity.IsEnabled,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }) ?? throw new ObjectNotFoundException(FeatureFlagName);
    }

    public async Task<FeatureFlagDto> CreateAsync(CreateFeatureFlagDto dto, CancellationToken cancellationToken = default)
    {
        if (await repository.IsExistsAsync(x => x.Name == dto.Name, cancellationToken))
            throw new ObjectAlreadyExistsException(FeatureFlagName, "name");

        var entity = new FeatureFlag
        {
            Name = dto.Name,
            Description = dto.Description,
            IsEnabled = false
        };

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new FeatureFlagDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public async Task UpdateAsync(int id, UpdateFeatureFlagDto dto, CancellationToken cancellationToken = default)
    {
        var isNewNameNotUnique = await repository.IsExistsAsync(x => x.Name == dto.Name && x.Id != id, cancellationToken);
        if (isNewNameNotUnique)
            throw new ObjectAlreadyExistsException(FeatureFlagName, "key");

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) throw new ObjectNotFoundException(FeatureFlagName);

        var oldName = entity.Name;

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.UpdatedAt = DateTime.Now;

        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);

        cache.Remove($"{CacheKeyPrefix}{oldName}");
    }

    public async Task UpdateStatusAsync(string name, UpdateFeatureFlagStatusDto requestDto, CancellationToken cancellationToken = default)
    {
        var entity = await repository.FindOneAsync(x => x.Name == name, cancellationToken);
        if (entity == null) throw new ObjectNotFoundException(FeatureFlagName);

        entity.IsEnabled = requestDto.IsEnabled;
        entity.UpdatedAt = DateTime.Now;

        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);

        cache.Remove($"{CacheKeyPrefix}{name}");
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) throw new ObjectNotFoundException(FeatureFlagName);

        repository.Remove(entity);
        await repository.SaveChangesAsync(cancellationToken);
        
        cache.Remove($"{CacheKeyPrefix}{entity.Name}");
    }
}