namespace DBA_Backend.Models.FilterModels
{
    /// <summary>
    /// Модель фильтров для списка абонентов
    /// </summary>
    public class AbonentFiltersModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public string? Street { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string? BuildingNumber { get; set; }

        /// <summary>
        /// Номер телефона (домашний)
        /// </summary>
        public string? HomePhoneNumber { get; set; }

        /// <summary>
        /// Номер телефона (рабочий)
        /// </summary>
        public string? WorkPhoneNumber { get; set; }

        /// <summary>
        /// Номер телефона (мобильный)
        /// </summary>
        public string? MobilePhoneNumber { get; set; }
    }
}
