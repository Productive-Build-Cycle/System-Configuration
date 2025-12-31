using PBC.SystemConfiguration.Application.Dtos;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Domain.Entities;

public class FeatureFlagService : IFeatureFlagService
{
    private readonly IFeatureFlagRepository _repository;

    public FeatureFlagService(IFeatureFlagRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FeatureFlag>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<FeatureFlag?> GetByIdAsync(Guid id)
        => await _repository.GetByIdAsync(id);

    public async Task CreateAsync(FeatureFlag featureFlag)
        => await _repository.AddAsync(featureFlag);

    public async Task UpdateAsync(Guid id, FeatureFlag featureFlag)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new Exception("FeatureFlag not found");

        existing.Name = featureFlag.Name;
        existing.IsEnabled = featureFlag.IsEnabled;

        await _repository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(Guid id)
        => await _repository.DeleteAsync(id);

    public Task<FeatureFlag> CreateAsync(CreateFeatureFlagDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    Task<List<FeatureFlag>> IFeatureFlagService.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<FeatureFlag> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    Task IFeatureFlagService.GetByIdAsync(Guid id)
    {
        return GetByIdAsync(id);
    }

    public Task GetByIdAsync(object id)
    {
        throw new NotImplementedException();
    }

    public Task<FeatureFlag> UpdateAsync(int id, UpdateFeatureFlagDto dto)
    {
        throw new NotImplementedException();
    }
}
