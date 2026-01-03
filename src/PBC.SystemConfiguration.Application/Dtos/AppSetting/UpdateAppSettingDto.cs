using PBC.SystemConfiguration.Domain.Enums;

namespace PBC.SystemConfiguration.Application.Dtos.AppSetting;

public class UpdateAppSettingDto
{
    public required string Key { get; set; }
    public required string Value { get; set; }
    public AppSettingType Type { get; set; }
    public  string? Description { get; set; }
}