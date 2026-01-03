using System.ComponentModel.DataAnnotations;

namespace PBC.SystemConfiguration.Domain.Entities;

public class FeatureFlag : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
}