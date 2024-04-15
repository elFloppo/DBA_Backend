using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBA_Backend.Models.DatabaseModels
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Street))]
        public int StreetId { get; set; }

        public string BuildingNumber { get; set; }
       
        public Street Street { get; set; }
    }
}
