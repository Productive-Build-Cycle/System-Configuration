using PBC.SystemConfiguration.Application.Dtos.AppSetting;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Entities;
using PBC.SystemConfiguration.Domain.Interfaces;

namespace PBC.SystemConfiguration.Application.Services;

public class AppSettingService(IAppSettingRepository repository) : IAppSettingService
{
    public async Task<IEnumerable<AppSettingDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new AppSettingDto
        {
            Key = e.Key,
            Value = e.Value,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        });
    }

    public async Task<AppSettingDto?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var entity = await repository.FindOneAsync(x => x.Key == key, cancellationToken);
        if (entity == null) return null;

        return new AppSettingDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public async Task<AppSettingDto> CreateAsync(CreateAppSettingDto dto, CancellationToken cancellationToken = default)
    {
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
            Type =  entity.Type,
            Description = entity.Description
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateAppSettingDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        entity.Key = dto.Key;
        entity.Description = dto.Description;

        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        repository.Remove(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}