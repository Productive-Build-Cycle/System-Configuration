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
        Task<bool> DeleteAsync(int id);
        Task<List<FeatureFlag>> GetAllAsync();
        Task<FeatureFlag> GetByIdAsync(int id);
        Task<FeatureFlag> UpdateAsync(int id, UpdateFeatureFlagDto dto);


    }

}
