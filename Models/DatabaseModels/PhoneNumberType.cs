using System.ComponentModel.DataAnnotations;

namespace DBA_Backend.Models.DatabaseModels
{
    public class PhoneNumberType
    {
        [Key]
        public PhoneNumberTypesEnum Id { get; set; }
        public string TypeName { get; set; }
    }

    public enum PhoneNumberTypesEnum
    {
        Home,
        Work,
        Mobile
    }
}
