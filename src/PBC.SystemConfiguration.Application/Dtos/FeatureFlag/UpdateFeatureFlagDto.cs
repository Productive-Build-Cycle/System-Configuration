namespace PBC.SystemConfiguration.Application.Dtos.FeatureFlag;

public class UpdateFeatureFlagDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}