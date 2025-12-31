using System;
using System.Collections.Generic;
using System.Text;

namespace PBC.SystemConfiguration.Application.Interfaces
{
    using PBC.SystemConfiguration.Application.Dtos;
    using PBC.SystemConfiguration.Domain.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFeatureFlagService
    {
        Task<FeatureFlag> CreateAsync(CreateFeatureFlagDto dto);
        Task CreateAsync(FeatureFlag featureFlag);
        Task<bool> DeleteAsync(int id);
        Task DeleteAsync(Guid id);
        Task<List<FeatureFlag>> GetAllAsync();
        Task<FeatureFlag> GetByIdAsync(int id);
        Task GetByIdAsync(Guid id);
        Task GetByIdAsync(object id);
        Task<FeatureFlag> UpdateAsync(int id, UpdateFeatureFlagDto dto);
        Task UpdateAsync(Guid id, FeatureFlag featureFlag);
    }

}
