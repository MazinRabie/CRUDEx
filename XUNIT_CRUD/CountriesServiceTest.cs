using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDtos;
using Services;
using Moq;
using Moq.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using RepositoryContracts;
using Repository;

namespace XUNIT_CRUD
{
    public class CountriesServiceTest
    {
        private ICountriesService _countriesService;
        private readonly ICountryRepository _countryRepository;

        public CountriesServiceTest()
        {
            // var DbcontextOptions = new DbContextOptionsBuilder<MyDbContext>().Options;
            // var dbContext = new MyDbContext(DbcontextOptions);
            var ListOfCountries = new List<Country>();
            var Persons = new List<Person>();
            // var MockDbContext = new Mock<MyDbContext>();
            // MockDbContext.Setup(x => x.Countries).ReturnsDbSet(ListOfCountries);
            // MockDbContext.Setup(x => x.Persons).ReturnsDbSet(Persons);

            // MockDbContext.Setup(x => x.SaveChanges()).Returns(1);
            // MockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            //              .ReturnsAsync(1);
            // MyDbContext dbContext = MockDbContext.Object;

            var options = new DbContextOptionsBuilder<MyDbContext>().Options;
            var MockDbContext = new DbContextMock<MyDbContext>(options);
            var CountriesDbsetMock = MockDbContext.CreateDbSetMock(x => x.Countries, ListOfCountries);
            var PersonsDbsetMock = MockDbContext.CreateDbSetMock(x => x.Persons, Persons);

            _countryRepository = new CountryRepository(MockDbContext.Object);
            _countriesService = new CountryService(_countryRepository);
        }


        #region AddCountryTest
        // test for null country throw argumentNullEx
        [Fact]
        public async Task AddCountry_NullCountry()
        {

            AddCountryRequest? req = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {

                await _countriesService.AddConuntry(req);
            });



        }
        // test for null country name throw argumentNullEx
        [Fact]
        public async Task AddCountry_NullCountryName()
        {

            AddCountryRequest? req = new AddCountryRequest() { CountryName = null };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {

                await _countriesService.AddConuntry(req);
            });



        }
        // test for duplicaet country throw argumentNullEx
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {

            AddCountryRequest? req1 = new AddCountryRequest() { CountryName = "USA" };
            AddCountryRequest? req2 = new AddCountryRequest() { CountryName = "USA" };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {

                await _countriesService.AddConuntry(req1);
                await _countriesService.AddConuntry(req2);
            });



        }// test for adding country 
        [Fact]
        public async Task AddCountry_ProperCountryDetailsWhenAdding()
        {

            AddCountryRequest? req1 = new AddCountryRequest() { CountryName = "USA" };
            var country_response = await _countriesService.AddConuntry(req1);

            Assert.True(country_response.guid != Guid.Empty);

            var countriesResponseList = await _countriesService.GetAllCountries();
            Assert.Contains(countriesResponseList, c => c.guid == country_response.guid);

        }
        #endregion

        #region GetAllCountries
        [Fact]
        public async Task GetAllCountries_Empty()
        {
            Assert.Empty(await _countriesService.GetAllCountries());
        }
        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {

            List<AddCountryRequest> listOfCountryAddReq = new List<AddCountryRequest>()
            {
                new AddCountryRequest(){CountryName="USA"},
                new AddCountryRequest(){CountryName="UK"},
            };

            var countryResponseListExpected = new List<CountryResponse>();


            foreach (var country in listOfCountryAddReq)
            {
                countryResponseListExpected.Add(await _countriesService.AddConuntry(country));
            }

            var countryResponseListActual = await _countriesService.GetAllCountries();
            foreach (var countryRes in countryResponseListExpected)
            {
                //Assert.Contains(countryResponseListActual, c => c.guid == countryRes.guid);
                Assert.Contains(countryRes, countryResponseListActual);
            }
        }

        #endregion

        #region GetCountryById

        [Fact]
        public async Task GetCountryById_NullID()
        {
            Guid? id = null;
            CountryResponse? countryResponse = await _countriesService.GetCountryById(id);
            Assert.Null(countryResponse);
        }
        [Fact]
        public async Task GetCountryById_ReturnProperCountry()
        {
            var countryReq = new AddCountryRequest() { CountryName = "USA" };
            var countryRes = await _countriesService.AddConuntry(countryReq);
            var returnedCountryResponseById = await _countriesService.GetCountryById(countryRes.guid);
            var NotExistedCountry = await _countriesService.GetCountryById(Guid.NewGuid());
            Assert.Equal(countryRes, returnedCountryResponseById);
            Assert.Null(NotExistedCountry);
        }
        #endregion


    }
}
