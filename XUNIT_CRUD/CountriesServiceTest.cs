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
using AutoFixture;
using System.Linq.Expressions;
using FluentAssertions;

namespace XUNIT_CRUD
{
    public class CountriesServiceTest
    {
        private ICountriesService _countriesService;
        private readonly ICountryRepository _countryRepository;
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly IFixture _fixture;


        public CountriesServiceTest()
        {
            // var DbcontextOptions = new DbContextOptionsBuilder<MyDbContext>().Options;
            // var dbContext = new MyDbContext(DbcontextOptions);
            // var ListOfCountries = new List<Country>();
            // var Persons = new List<Person>();
            // var MockDbContext = new Mock<MyDbContext>();
            // MockDbContext.Setup(x => x.Countries).ReturnsDbSet(ListOfCountries);
            // MockDbContext.Setup(x => x.Persons).ReturnsDbSet(Persons);

            // MockDbContext.Setup(x => x.SaveChanges()).Returns(1);
            // MockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            //              .ReturnsAsync(1);
            // MyDbContext dbContext = MockDbContext.Object;

            // var options = new DbContextOptionsBuilder<MyDbContext>().Options;
            // var MockDbContext = new DbContextMock<MyDbContext>(options);
            // var CountriesDbsetMock = MockDbContext.CreateDbSetMock(x => x.Countries, ListOfCountries);
            // var PersonsDbsetMock = MockDbContext.CreateDbSetMock(x => x.Persons, Persons);

            // _countryRepository = new CountryRepository(MockDbContext.Object);

            _mockCountryRepository = new Mock<ICountryRepository>();
            _countryRepository = _mockCountryRepository.Object;
            _countriesService = new CountryService(_countryRepository);
            _fixture = new Fixture();
        }


        #region AddCountryTest
        // test for null country throw argumentNullEx
        [Fact]
        public async Task AddCountry_NullCountry_ToThrowArgumentNullException()
        {

            AddCountryRequest? req = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {

                await _countriesService.AddConuntry(req);
            });



        }
        // test for null country name throw argumentNullEx
        [Fact]
        public async Task AddCountry_NullCountryName_ToThrowArgumentException()
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

            // AddCountryRequest? req1 = new AddCountryRequest() { CountryName = "USA" };
            // AddCountryRequest? req2 = new AddCountryRequest() { CountryName = "USA" };
            var countryRequest = _fixture.Build<AddCountryRequest>().With(x => x.CountryName, "USA").Create();
            var Country = countryRequest.ToCountry();
            _mockCountryRepository.Setup(x => x.GetCountryByExp(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(Country);

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {

                await _countriesService.AddConuntry(countryRequest);
                // await _countriesService.AddConuntry(req2);
            });



        }// test for adding country 
        [Fact]
        public async Task AddCountry_ProperCountryDetailsWhenAdding_ToBeSuccessful()
        {

            AddCountryRequest? req1 = _fixture.Build<AddCountryRequest>().With(x => x.CountryName, "USA").Create();
            var expectedCountry = req1.ToCountry();
            _mockCountryRepository.Setup(x => x.GetCountryByExp(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(null as Country);
            _mockCountryRepository.Setup(x => x.AddCountry(It.IsAny<Country>())).ReturnsAsync(expectedCountry);
            var country_response = await _countriesService.AddConuntry(req1);

            Assert.True(country_response.guid != Guid.Empty);
            country_response.Should().Be(expectedCountry.ToCountryResponse());

            // var countriesResponseList = await _countriesService.GetAllCountries();
            // Assert.Contains(countriesResponseList, c => c.guid == country_response.guid);

        }
        #endregion

        #region GetAllCountries
        [Fact]
        public async Task GetAllCountries_Empty_ToBeEmpty()
        {
            var EmptyList = new List<Country>();
            _mockCountryRepository.Setup(x => x.GetAllCountries()).ReturnsAsync(EmptyList);
            Assert.Empty(await _countriesService.GetAllCountries());
        }
        [Fact]
        public async Task GetAllCountries_AddFewCountries_ToBeSuccessful()
        {

            // List<AddCountryRequest> listOfCountryAddReq = new List<AddCountryRequest>()
            // {
            //     new AddCountryRequest(){CountryName="USA"},
            //     new AddCountryRequest(){CountryName="UK"},
            // };

            // var countryResponseListExpected = new List<CountryResponse>();


            // foreach (var country in listOfCountryAddReq)
            // {
            //     countryResponseListExpected.Add(await _countriesService.AddConuntry(country));
            // }
            var expectedCountryLst = new List<Country>()
            {
                _fixture.Build<Country>().With(x=>x.Persons,null as List<Person>).Create(),
                _fixture.Build<Country>().With(x=>x.Persons,null as List<Person>).Create(),
                _fixture.Build<Country>().With(x=>x.Persons,null as List<Person>).Create()
            };
            var CountryResLst = expectedCountryLst.Select(x => x.ToCountryResponse());
            _mockCountryRepository.Setup(x => x.GetAllCountries()).ReturnsAsync(expectedCountryLst);
            var countryResponseListActual = await _countriesService.GetAllCountries();
            CountryResLst.Should().BeEquivalentTo(countryResponseListActual);
            // foreach (var countryRes in countryResponseListExpected)
            // {
            //     //Assert.Contains(countryResponseListActual, c => c.guid == countryRes.guid);
            //     Assert.Contains(countryRes, countryResponseListActual);
            // }
        }

        #endregion

        #region GetCountryById

        [Fact]
        public async Task GetCountryById_NullID_ToBeNull()
        {
            Guid? id = null;
            CountryResponse? countryResponse = await _countriesService.GetCountryById(id);
            Assert.Null(countryResponse);
        }
        [Fact]
        public async Task GetCountryById_ReturnProperCountry_toBeSuccessful()
        {
            // var countryReq = new AddCountryRequest() { CountryName = "USA" };
            // var countryRes = await _countriesService.AddConuntry(countryReq);
            var country = _fixture.Build<Country>().With(x => x.CountryName, "USA").With(x => x.Persons, null as List<Person>).Create();
            var CountryRes = country.ToCountryResponse();
            _mockCountryRepository.Setup(x => x.GetCountryById(It.IsAny<Guid>())).ReturnsAsync(country);
            var returnedCountryResponseById = await _countriesService.GetCountryById(CountryRes.guid);
            // var NotExistedCountry = await _countriesService.GetCountryById(Guid.NewGuid());
            Assert.Equal(CountryRes, returnedCountryResponseById);
            // Assert.Null(NotExistedCountry);
        }
        #endregion


    }
}
