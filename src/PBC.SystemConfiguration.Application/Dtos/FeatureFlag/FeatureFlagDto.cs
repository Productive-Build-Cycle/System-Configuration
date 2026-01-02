namespace PBC.SystemConfiguration.Application.Dtos.FeatureFlag;

public class FeatureFlagDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public string? Description { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
}