using PBC.SystemConfiguration.Domain.Entities;
using PBC.SystemConfiguration.Domain.Interfaces;
using PBC.SystemConfiguration.Infrastructure.Persistence.DbContext;

namespace PBC.SystemConfiguration.Infrastructure.Persistence.Repositories;

public class FeatureFlagRepository(ProgramDbContext context) : Repository<FeatureFlag>(context), IFeatureFlagRepository
{
}