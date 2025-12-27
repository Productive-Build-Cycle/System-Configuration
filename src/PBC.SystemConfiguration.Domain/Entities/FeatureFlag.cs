using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PBC.SystemConfiguration.Domain.Entities
{
    public class FeatureFlag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;
    }
}
