using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DBA_Backend.Models.DatabaseModels
{
    public class Abonent
    {
        public Abonent()
        {
            PhoneNumbers = new List<PhoneNumber>();
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }

        public Address Address { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
    }
}
