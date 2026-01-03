using Microsoft.Extensions.Caching.Memory;
using PBC.SystemConfiguration.Application.Dtos.AppSetting;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Entities;
using PBC.SystemConfiguration.Domain.Exceptions;
using PBC.SystemConfiguration.Domain.Interfaces;

namespace PBC.SystemConfiguration.Application.Services;

public class AppSettingService(IAppSettingRepository repository, IMemoryCache cache) : IAppSettingService
{
    private const string CacheKeyPrefix = "appsetting_";
    private const string AppSettingName = "App Setting";

    public async Task<IEnumerable<AppSettingDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new AppSettingDto
        {
            Id = e.Id,
            Key = e.Key,
            Value = e.Value,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        });
    }

    public async Task<AppSettingDto> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{CacheKeyPrefix}{key}";

        return await cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
            entry.SlidingExpiration = TimeSpan.FromMinutes(15);

            var entity = await repository.FindOneAsync(x => x.Key == key, cancellationToken);
            if (entity == null) throw new ObjectNotFoundException(AppSettingName);

            return new AppSettingDto
            {
                Id = entity.Id,
                Key = entity.Key,
                Value = entity.Value,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Type = entity.Type
            };
        }) ?? throw new ObjectNotFoundException(AppSettingName);
    }

    public async Task<AppSettingDto> CreateAsync(CreateAppSettingDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Type == 0)
            throw new InvalidFieldException(nameof(dto.Type));

        if (await repository.IsExistsAsync(x => x.Key == dto.Key, cancellationToken))
            throw new ObjectAlreadyExistsException(AppSettingName, "key");

        var entity = new AppSetting
        {
            Key = dto.Key,
            Value = dto.Value,
            Type = dto.Type,
            Description = dto.Description,
        };

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new AppSettingDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Value = entity.Value,
            Type = entity.Type,
            Description = entity.Description
        };
    }

    public async Task UpdateAsync(int id, UpdateAppSettingDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Type == 0)
            throw new InvalidFieldException(nameof(dto.Type));

        var isNewKeyNotUnique = await repository.IsExistsAsync(x => x.Key == dto.Key && x.Id != id, cancellationToken);
        if (isNewKeyNotUnique)
            throw new ObjectAlreadyExistsException(AppSettingName, "key");

        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) throw new ObjectNotFoundException(AppSettingName);

        // Capture old key for invalidation
        var oldKey = entity.Key;

        entity.Key = dto.Key;
        entity.Value = dto.Value;
        entity.Type = dto.Type;
        entity.Description = dto.Description;
        entity.UpdatedAt = DateTime.Now;

        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);

        cache.Remove($"{CacheKeyPrefix}{oldKey}");
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) throw new ObjectNotFoundException(AppSettingName);

        repository.Remove(entity);
        await repository.SaveChangesAsync(cancellationToken);

        cache.Remove($"{CacheKeyPrefix}{entity.Key}");
    }
}