using System.ComponentModel.DataAnnotations.Schema;

namespace PBC.SystemConfiguration.Domain.Entities
{
    public class AppSetting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; } 
        public DateTime LastUpdateDate { get; set; }
    }
}