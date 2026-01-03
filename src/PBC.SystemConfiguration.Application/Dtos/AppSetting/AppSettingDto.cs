using PBC.SystemConfiguration.Domain.Enums;

namespace PBC.SystemConfiguration.Application.Dtos.AppSetting;

public class AppSettingDto
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public AppSettingType Type { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}