using DBA_Backend.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace DBA_Backend
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Данные для генерации
            var streets = new Street[]
            {
                new Street { Id = 1, Name = "Рабочая" }, new Street { Id = 2, Name = "Победы" },
                new Street { Id = 3, Name = "Новая" }, new Street { Id = 4, Name = "Первомайская" },
                new Street { Id = 5, Name = "Молодежная" }, new Street { Id = 6, Name = "Мира" },
                new Street { Id = 7, Name = "Сибирская" }, new Street { Id = 8, Name = "Ленина" },
                new Street { Id = 9, Name = "Озерная" }, new Street { Id = 10, Name = "Восточная" },
                new Street { Id = 11, Name = "Вишневая" }, new Street { Id = 12, Name = "Песчаная" }
            };

            var buildingNumbers = new string[]
            {
                "1", "777", "45a", "13",
                "666", "23", "17", "88",
                "7б", "63", "11", "9"
            };

            var surnames = new string[]
            {
                "Родионов", "Федоров", "Дмитриев", "Попов",
                "Зайцев", "Елисеев", "Денисов", "Андреев",
                "Тихонов", "Гусев", "Чистяков", "Егоров"
            };

            var names = new string[]
            {
                "Александр", "Даниил", "Арсений", "Марк",
                "Михаил", "Константин", "Савва", "Платон",
                "Фёдор", "Кирилл", "Артемий", "Олег"
            };

            var patronymics = new string[]
            {
                "Артёмович", "Родионович", "Ярославович", "Алексеевич",
                "Владиславович", "Андреевич", "Григорьевич", "Павлович",
                "Максимович", "Борисович", "Никитич", "Романович"
            };

            var phoneNumbers = new string[]
            {
                "284-07-30", "6(187)530-56-79", "196(0790)189-25-23", "978(4337)840-70-26",
                "174(79)586-11-98", "+6(819)930-34-84", "41(1608)443-05-40", "57(49)683-71-85",
                "49(3133)974-28-07", "+436(856)302-58-71", "264-44-52", "0(954)297-21-30"
            };
            #endregion

            AddEntity(new PhoneNumberType { Id = PhoneNumberTypesEnum.Home, TypeName = "Домашний" });
            AddEntity(new PhoneNumberType { Id = PhoneNumberTypesEnum.Work, TypeName = "Рабочий" });
            AddEntity(new PhoneNumberType { Id = PhoneNumberTypesEnum.Mobile, TypeName = "Мобильный" });

            foreach (var street in streets)
                AddEntity(street);

            var rnd = new Random(666);
            for (var i = 0; i < 100; i++) 
            {                
                Func<int, Address> generateAddress = x => 
                { 
                    var index = rnd.Next(0, 12); 
                    return new Address { Id = i + 1, StreetId = x, BuildingNumber = buildingNumbers[index] }; 
                };
                Func<int, Abonent> generateAbonent = x => 
                { 
                    var index1 = rnd.Next(0, 12);
                    var index2 = rnd.Next(0, 12);
                    var index3 = rnd.Next(0, 12);
                    return new Abonent { Id = i + 1, AddressId = x, Surname = surnames[index1], Name = names[index2], Patronymic = patronymics[index3] }; 
                };
                Func<int, PhoneNumber> generatePhoneNumber = x => 
                { 
                    var index1 = rnd.Next(0, 3);
                    var index2 = rnd.Next(0, 12);
                    return new PhoneNumber { Id = i + 1, AbonentId = x, TypeId = (PhoneNumberTypesEnum)index1, Number = phoneNumbers[index2] }; 
                };

                var street = streets[rnd.Next(0, 12)];
                var address = generateAddress(street.Id);
                var abonent = generateAbonent(address.Id);
                var phoneNumber = generatePhoneNumber(abonent.Id);
                
                AddEntity(address);
                AddEntity(abonent);
                AddEntity(phoneNumber);
            }

            void AddEntity<T>(T entity) where T : class { modelBuilder.Entity<T>().HasData(entity); }
        }

        public DbSet<Abonent> Abonents { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
    }
}
