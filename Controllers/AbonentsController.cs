using DBA_Backend.Models.DatabaseModels;
using DBA_Backend.Models.DatabaseModels.Repositories;
using DBA_Backend.Models.Enums;
using DBA_Backend.Models.FilterModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DBA_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbonentsController : Controller
    {
        private readonly string _connectionString;

        public AbonentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Получение списка с информацией о улицах и кол-ве абонентов, проживающих на этих улицах
        /// </summary>
        /// <returns>Список улиц и кол-ва абонентов, проживающих на этих улицах</returns>
        [HttpGet]
        [Route("AbonentsOnStreetsCount")]
        public async Task<ActionResult> GetAbonentsOnStreetsCount()
        {
            IEnumerable<Abonent> abonents = null;

            using (var abonentsRepository = new AbonentRepository(_connectionString))
                abonents = await abonentsRepository.SelectAsync();

            var result = abonents
                .GroupBy(a => new { StreetId = a.Address.StreetId, StreetName = a.Address.Street.Name })
                .Select(a => new AbonentsOnStreetsCountModel
                {
                    StreetName = a.Key.StreetName,
                    AbonentsCount = a.Count()
                });

            return Ok(JsonConvert.SerializeObject(result));           
        }

        /// <summary>
        /// Получение списка с информацией об абонентах
        /// </summary>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Кол-во записей на странице</param>
        /// <param name="filter">Фильтры</param>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="sortByDesc">Сортировка по убыванию</param>
        /// <returns>Список с информацией об абонентах</returns>
        [HttpGet]
        [Route("Abonents")]
        public async Task<ActionResult> GetAbonents(int? pageNumber = null, int? pageSize = null, 
            [FromQuery]AbonentFiltersModel filter = null, [FromQuery] SortingFieldsEnum? sortField = null,
            bool sortByDesc = false)
        {
            return Ok(JsonConvert.SerializeObject(await GetAbonentsFromRepository(pageNumber, pageSize, filter, sortField, sortByDesc)));
        }

        /// <summary>
        /// Получение кол-ва абонентов
        /// </summary>
        /// <param name="filter">Фильтры</param>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="sortByDesc">Сортировка по убыванию</param>
        /// <returns>Кол-во абонентов</returns>
        [HttpGet]
        [Route("AbonentsCount")]
        public async Task<ActionResult> GetAbonentsCount([FromQuery] AbonentFiltersModel filter = null, 
            [FromQuery] SortingFieldsEnum? sortField = null,
            bool sortByDesc = false)
        {
            return Ok(JsonConvert.SerializeObject((await GetAbonentsFromRepository(filter: filter, sortField: sortField, sortByDesc: sortByDesc)).Count()));
        }

        private async Task<IEnumerable<Abonent>> GetAbonentsFromRepository(int? pageNumber = null, int? pageSize = null,
            [FromQuery] AbonentFiltersModel filter = null, [FromQuery] SortingFieldsEnum? sortField = null,
            bool sortByDesc = false)
        {
            IEnumerable<Abonent> abonents = null;
            using (var abonentsRepository = new AbonentRepository(_connectionString))
                abonents = await abonentsRepository.SelectAsync(pageNumber, pageSize, filter, sortField, sortByDesc);

            return abonents;
        }

        /// <summary>
        /// Поиск абонентов по номеру телефона
        /// </summary>
        /// <param name="phoneNumber">Номер телефона</param>
        /// <returns>Список найденых абонентов</returns>
        [HttpGet]
        [Route("FindAbonentsByPhoneNumber")]
        public async Task<ActionResult> FindAbonentsByPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace(" ", "").ToLower();

            IEnumerable<Abonent> abonents = null;
            using (var abonentsRepository = new AbonentRepository(_connectionString))
                abonents = await abonentsRepository.SelectAsync();

            var resultAbonents = abonents.Where(a => a.PhoneNumbers.Any(n => n.Number.Contains(phoneNumber)));

            return Ok(JsonConvert.SerializeObject(resultAbonents));
        }

        public class AbonentsOnStreetsCountModel
        {
            public string StreetName { get; set; }
            public int AbonentsCount { get; set; }          
        }
    }
}
