using PBC.SystemConfiguration.Domain.Entities;

internal interface IFeatureFlagRepository
{
    Task AddAsync(FeatureFlag featureFlag);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<FeatureFlag>> GetAllAsync();
    Task<FeatureFlag?> GetByIdAsync(Guid id);
    Task UpdateAsync(FeatureFlag existing);
}