using PBC.SystemConfiguration.Application.Dtos.AppSetting;

namespace PBC.SystemConfiguration.Application.Interfaces;

public interface IAppSettingService
{
    Task<IEnumerable<AppSettingDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AppSettingDto> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<AppSettingDto> CreateAsync(CreateAppSettingDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateAppSettingDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}