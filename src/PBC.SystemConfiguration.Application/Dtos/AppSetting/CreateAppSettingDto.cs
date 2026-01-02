using PBC.SystemConfiguration.Domain.Enums;

namespace PBC.SystemConfiguration.Application.Dtos.AppSetting;

public class CreateAppSettingDto
{
    public string Key { get; set; }
    public string Value { get; set; }
    public AppSettingType Type { get; set; }
    public string? Description { get; set; }
}