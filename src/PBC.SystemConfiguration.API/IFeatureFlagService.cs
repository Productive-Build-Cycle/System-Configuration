
using PBC.SystemConfiguration.Application.Dtos;

namespace PBC.SystemConfiguration.Application.Interfaces
{
    public interface IFeatureFlagService
    {
        Task<List<FeatureFlagDto>> GetAllAsync();

        Task<FeatureFlagDto?> GetByIdAsync(int id);

        Task<FeatureFlagDto> CreateAsync(CreateFeatureFlagDto dto);

        Task<FeatureFlagDto?> UpdateAsync(int id, UpdateFeatureFlagDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
