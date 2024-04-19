using Dapper;
using DBA_Backend.Models.Enums;
using DBA_Backend.Models.FilterModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DBA_Backend.Models.DatabaseModels.Repositories
{
    public interface IAbonentRepository
    {
        Task<IEnumerable<Abonent>> SelectAsync(int? pageNumber, int? pageSize, 
            AbonentFiltersModel filter, SortingFieldsEnum? sortingField, 
            bool? sortByDesc);
    }

    public class AbonentRepository : IAbonentRepository, IDisposable
    {
        private readonly SqliteConnection _connection;

        public AbonentRepository(string connectionString)
        {
            _connection = new SqliteConnection(connectionString);
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseSqlite(_connection).Options;

            var context = new ApplicationDBContext(options);
        }

        public async Task<IEnumerable<Abonent>> SelectAsync(int? pageNumber = null, int? pageSize = null, 
            AbonentFiltersModel filter = null, SortingFieldsEnum? sortingField = null, 
            bool? sortByDesc = null)
        {
            string filterQuery = BuildFilter(filter);
            string sortQueryField = BuildSorter(sortingField);
            
            var query = @$"SELECT * FROM Abonents A 
                        INNER JOIN Addresses Ad ON A.AddressId = Ad.Id 
                        INNER JOIN Streets S ON Ad.StreetId = S.Id
                        INNER JOIN PhoneNumbers PN ON PN.AbonentId = A.Id
                        INNER JOIN PhoneNumberTypes PNT ON PN.TypeId = PNT.Id                        
                        {filterQuery}                                                    
                        {(sortQueryField != null ? $"ORDER BY {sortQueryField}{(sortByDesc == true ? " DESC" : "")}" : "")}                       
                        {(pageNumber != null && pageSize != null ? $"LIMIT {(pageNumber - 1) * pageSize}, {pageSize}" : "")}";

            return await _connection.QueryAsync<Abonent, Address, Street, PhoneNumber, PhoneNumberType, Abonent>(query, (abonent, address, street, phoneNumber, phoneNumberType) =>
            {
                address.Street = street;
                abonent.Address = address;
                phoneNumber.Type = phoneNumberType;
                abonent.PhoneNumbers.Add(phoneNumber);
                return abonent;
            });
        }

        private string BuildFilter(AbonentFiltersModel filter)
        {
            if (filter == null)
                return null;

            var filterQueryBuilder = new StringBuilder();

            if (filter?.Id != null) filterQueryBuilder.Append($"WHERE A.Id = {filter.Id}\n");
            if (filter?.FullName != null) filterQueryBuilder.Append($"{(filterQueryBuilder.Length > 0 ? "AND" : "WHERE")} REPLACE(A.Surname || A.Name || A.Patronymic, ' ', '') LIKE '%{filter.FullName.Replace(" ", "")}%'\n");
            if (filter?.Street != null) filterQueryBuilder.Append($"{(filterQueryBuilder.Length > 0 ? "AND" : "WHERE")} REPLACE(S.Name, ' ', '') LIKE '%{filter.Street.Replace(" ", "")}%'\n");
            if (filter?.BuildingNumber != null) filterQueryBuilder.Append($"{(filterQueryBuilder.Length > 0 ? "AND" : "WHERE")} REPLACE(Ad.BuildingNumber, ' ', '') LIKE '%{filter.BuildingNumber.Replace(" ", "")}%'\n");
            if (filter?.HomePhoneNumber != null) filterQueryBuilder.Append($"{(filterQueryBuilder.Length > 0 ? "AND" : "WHERE")} PNT.Id = {(int)PhoneNumberTypesEnum.Home} AND REPLACE(PN.Number, ' ', '') LIKE '%{filter.HomePhoneNumber.Replace(" ", "")}%'\n");
            if (filter?.WorkPhoneNumber != null) filterQueryBuilder.Append($"{(filterQueryBuilder.Length > 0 ? "AND" : "WHERE")} PNT.Id = {(int)PhoneNumberTypesEnum.Work} AND REPLACE(PN.Number, ' ', '') LIKE '%{filter.WorkPhoneNumber.Replace(" ", "")}%'\n");
            if (filter?.MobilePhoneNumber != null) filterQueryBuilder.Append($"{(filterQueryBuilder.Length > 0 ? "AND" : "WHERE")} PNT.Id = {(int)PhoneNumberTypesEnum.Mobile} AND REPLACE(PN.Number, ' ' , '') LIKE '%{filter.MobilePhoneNumber.Replace(" ", "")}%'\n");

            return filterQueryBuilder.ToString();
        }

        private string BuildSorter(SortingFieldsEnum? sortingField)
        {
            return sortingField switch
            {
                SortingFieldsEnum.Id => "A.Id",
                SortingFieldsEnum.FullName => "A.Surname || A.Name || A.Patronymic",
                SortingFieldsEnum.Street => "S.Name",
                SortingFieldsEnum.BuildingNumber => "Ad.BuildingNumber",
                SortingFieldsEnum.HomePhoneNumber => $"NOT PNT.Id = {(int)PhoneNumberTypesEnum.Home}, PN.Number",
                SortingFieldsEnum.WorkPhoneNumber => $"NOT PNT.Id = {(int)PhoneNumberTypesEnum.Work}, PN.Number",
                SortingFieldsEnum.MobilePhoneNumber => $"NOT PNT.Id = {(int)PhoneNumberTypesEnum.Mobile}, PN.Number",
                _ => null
            };
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
