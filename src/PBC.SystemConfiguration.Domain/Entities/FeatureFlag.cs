using System.ComponentModel.DataAnnotations;

namespace PBC.SystemConfiguration.Domain.Entities;

public class FeatureFlag : BaseEntity
{
    [Required] [StringLength(100)] public string Name { get; set; }
    public string Description { get; set; }
}