using System.ComponentModel.DataAnnotations;

namespace DBA_Backend.Models.Enums
{
    public enum SortingFieldsEnum
    {
        /// <summary>
        /// Id
        /// </summary>
        [Display(Name = "Id")]
        Id = 0,
        [Display(Name = "FullName")]
        FullName = 1,
        [Display(Name = "Street")]
        Street = 2,
        [Display(Name = "BuildingNumber")]
        BuildingNumber = 3,
        [Display(Name = "HomePhoneNumber")]
        HomePhoneNumber = 4,
        [Display(Name = "WorkPhoneNumber")]
        WorkPhoneNumber = 5,
        [Display(Name = "MobilePhoneNumber")]
        MobilePhoneNumber = 6
    }
}
