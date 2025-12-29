using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PBC.SystemConfiguration.Application.Dtos
{
    public class UpdateFeatureFlagDto
    {
        [Required]
        public int Id { get; set; }  

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }

}
