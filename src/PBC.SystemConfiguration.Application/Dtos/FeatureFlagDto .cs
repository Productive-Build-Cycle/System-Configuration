using System;
using System.Collections.Generic;
using System.Text;

namespace PBC.SystemConfiguration.Application.Dtos
{
    public class FeatureFlagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
