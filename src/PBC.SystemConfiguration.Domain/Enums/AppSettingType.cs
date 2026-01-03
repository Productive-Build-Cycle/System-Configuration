namespace PBC.SystemConfiguration.Domain.Enums;

public enum AppSettingType
{
    String = 1,
    Number = 2,
    Boolean = 3,
    Json = 4 // For complex feature flag rules or objects
}