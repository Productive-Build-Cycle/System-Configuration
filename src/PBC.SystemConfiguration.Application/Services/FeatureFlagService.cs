using PBC.SystemConfiguration.Application.Dtos.FeatureFlag;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Entities;
using PBC.SystemConfiguration.Domain.Exceptions;
using PBC.SystemConfiguration.Domain.Interfaces;

namespace PBC.SystemConfiguration.Application.Services;

public class FeatureFlagService(IFeatureFlagRepository repository) : IFeatureFlagService
{
    public async Task<IEnumerable<FeatureFlagDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new FeatureFlagDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        });
    }

    public async Task<FeatureFlagDto> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var entity = await repository.FindOneAsync(x => x.Name == name, cancellationToken);
        if (entity == null) throw new ObjectNotFoundException("Feature Flag");

        return new FeatureFlagDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public async Task<FeatureFlagDto> CreateAsync(CreateFeatureFlagDto dto, CancellationToken cancellationToken = default)
    {
        if (await repository.IsExistsAsync(x => x.Name == dto.Name, cancellationToken))
            throw new ObjectAlreadyExistsException("Feature Flag", "name");
        
        var entity = new FeatureFlag
        {
            Name = dto.Name,
            Description = dto.Description
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
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) throw new ObjectNotFoundException("Feature Flag");

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        
        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) throw new ObjectNotFoundException("Feature Flag");

        repository.Remove(entity);
        await repository.SaveChangesAsync(cancellationToken);
    }
}