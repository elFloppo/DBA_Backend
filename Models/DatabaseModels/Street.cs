using System.ComponentModel.DataAnnotations;

namespace DBA_Backend.Models.DatabaseModels
{
    public class Street
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
