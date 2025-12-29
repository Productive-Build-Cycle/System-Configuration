using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBC.SystemConfiguration.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {  get; set; }
    
    public DateTime CreateDate { get; set; } = DateTime.Now;
    
    public DateTime LastUpdateDate { get; set; } = DateTime.Now;
}