using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBA_Backend.Models.DatabaseModels
{
    public class PhoneNumber
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Abonent))]
        public int AbonentId { get; set; }

        [ForeignKey(nameof(Type))]
        public PhoneNumberTypesEnum TypeId { get; set; }

        public string Number { get; set; }

        [JsonIgnore]
        public Abonent Abonent { get; set; }
        public PhoneNumberType Type { get; set; } 
    }
}
