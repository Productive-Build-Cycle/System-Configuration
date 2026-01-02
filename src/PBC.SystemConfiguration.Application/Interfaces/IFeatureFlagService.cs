using PBC.SystemConfiguration.Application.Dtos.FeatureFlag;

namespace PBC.SystemConfiguration.Application.Interfaces;

public interface IFeatureFlagService
{
    Task<IEnumerable<FeatureFlagDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FeatureFlagDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<FeatureFlagDto> CreateAsync(CreateFeatureFlagDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateFeatureFlagDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}