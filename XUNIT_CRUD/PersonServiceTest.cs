using System.Threading.Tasks;
using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDtos;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;
using FluentAssertions;
using RepositoryContracts;
using Repository;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Serilog;

namespace XUNIT_CRUD
{
    public class PersonServiceTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ILogger<PersonService> _logger;
        private readonly Mock<ILogger<PersonService>> _loggerMock;
        private readonly IDiagnosticContext _diagnosticsContext;
        private readonly Mock<IDiagnosticContext> _diagnosticsContextMock;
        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly IFixture _fixture;
        private readonly IPersonRepository _personRepository;
        private IPersonService _personsService;
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            #region inmemory DbContext mocking
            // var Persons = new List<Person>();
            // var Countries = new List<Country>();
            // var MockDbContext = new DbContextMock<MyDbContext>(new DbContextOptionsBuilder<MyDbContext>().Options);
            // MockDbContext.CreateDbSetMock(x => x.Persons, Persons);
            // MockDbContext.CreateDbSetMock(x => x.Countries, Countries);
            // _personRepository = new PersonRepository(MockDbContext.Object);
            // _personsService = new PersonService(_personRepository);
            #endregion
            _loggerMock = new Mock<ILogger<PersonService>>();
            _logger = _loggerMock.Object;
            _diagnosticsContextMock = new Mock<IDiagnosticContext>();
            _diagnosticsContext = _diagnosticsContextMock.Object;
            #region  mocking Repository
            _mockPersonRepository = new Mock<IPersonRepository>();
            _personRepository = _mockPersonRepository.Object;
            _personsService = new PersonService(_personRepository, _logger, _diagnosticsContext);
            #endregion
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson
        [Fact]
        public async Task AddPerson_nullPerson_ToThrowArgumentNullException()
        {
            AddPersonRequest? person = null;
            Func<Task> action = (async () =>
            {
                var res = await _personsService.AddPerson(person);

            });
            await action.Should().ThrowAsync<ArgumentNullException>();


        }
        [Fact]
        public async Task AddPerson_PersonModelValidation_ToThrowArgumentException()
        {
            // AddPersonRequest? person = new AddPersonRequest() { Name = null };
            AddPersonRequest? person = _fixture.Build<AddPersonRequest>().With(x => x.Name, null as string).Create();
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var res = await _personsService.AddPerson(person);

            });


        }
        [Fact]
        public async Task AddPerson_ProperPersonDetails_ToBeSuccessful()
        {
            // AddPersonRequest? person = new AddPersonRequest() { Name = "ali", Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "dummy@gmailcom", Gender = Entities.Gender.Male, RecieveNewsLetters = true };
            AddPersonRequest? personReq = _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create();
            //AddPersonRequest? person = new AddPersonRequest() { Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "du", Gender = Entities.Gender.male, RecieveNewsLetters = true };
            Person personMock = personReq.ToPerson();
            PersonResponse expectedPersonres = personMock.ToPersonResponse();
            _mockPersonRepository.Setup(x => x.AddPerson(It.IsAny<Person>())).ReturnsAsync(personMock);
            var res = await _personsService.AddPerson(personReq);
            // var ActualPeopleList = await _personsService.GetAllPeople();
            Assert.True(res.Id != Guid.Empty);
            expectedPersonres.Should().Be(res);
            // Assert.NotEmpty(ActualPeopleList);
            // Assert.Contains(res, ActualPeopleList);
            // ActualPeopleList.Should().Contain(res);
        }

        #endregion

        #region GetPersonById

        [Fact]
        public async Task GetPersonById_nullId_ToBeNull()
        {
            var personRes = await _personsService.GetPersonByID(null);
            Assert.Null(personRes);
        }
        [Fact]
        public async Task GetPersonById_ProperPersonReturned_ToBeSuccessful()
        {
            var req = _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create();
            var person = req.ToPerson();
            var Expected_personRes = person.ToPersonResponse();


            // new AddPersonRequest()
            // {
            //     Name = "ali",
            //     Email = "alo@gmail.com",
            //     Gender = Gender.Male
            // };

            // var personRes = await _personsService.AddPerson(personReq);

            Assert.NotNull(Expected_personRes.Id);
            _mockPersonRepository.Setup(x => x.GetPersonWithCountryById(It.IsAny<Guid>())).ReturnsAsync(person);
            var actualReturnedPersonFromTheMethod = await _personsService.GetPersonByID(Expected_personRes.Id);
            Assert.Equal(Expected_personRes, actualReturnedPersonFromTheMethod);

        }
        [Fact]
        public async Task GetPersonById_NonExistingPerson_ToBenull()
        {
            _mockPersonRepository.Setup(x => x.GetPersonWithCountryById(It.IsAny<Guid>())).ReturnsAsync(null as Person);
            var PersonRes = await _personsService.GetPersonByID(Guid.NewGuid());
            Assert.Null(PersonRes);
        }
        #endregion

        #region GetAllPeople
        [Fact]
        public async Task GetAllPeople_empty_ToBeEmpty()
        {
            var lst = new List<Person>();
            _mockPersonRepository.Setup(x => x.GetAllPersons()).ReturnsAsync(lst);
            var peopleList = await _personsService.GetAllPeople();
            Assert.Empty(peopleList);
        }
        [Fact]
        public async Task GetAllPeople_ContainsProperPeople_ToBeSuccessful()
        {
            // var listOfPersonReqs = new List<AddPersonRequest>()
            // {
            //     //  new AddPersonRequest() {Name="ali", Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true }
            //     // , new AddPersonRequest() { Name="ahmed",Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true }
            //             _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create(),
            //             _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc2@gmail.com").Create()
            // };
            // var ListOfPersonResponses = new List<PersonResponse>();

            // _testOutputHelper.WriteLine("Expected");
            // foreach (var perReq in listOfPersonReqs)
            // {
            //     var res = await _personsService.AddPerson(perReq);
            //     ListOfPersonResponses.Add(res);
            //     _testOutputHelper.WriteLine(res.ToString());
            // }
            var persons = new List<Person>()
            {
                  _fixture.Build<Person>().With(x => x.Email, "abc1@gmail.com").With(x => x.Country, null as Country).Create(),
                  _fixture.Build<Person>().With(x => x.Email, "abc2@gmail.com").With(x => x.Country, null as Country).Create(),
                  _fixture.Build<Person>().With(x => x.Email, "abc3@gmail.com").With(x => x.Country, null as Country).Create(),

            };
            var personResLst = persons.Select(x => x.ToPersonResponse());
            _mockPersonRepository.Setup(x => x.GetAllPersons()).ReturnsAsync(persons);

            var actualReturnedPersonResList = await _personsService.GetAllPeople();
            // _testOutputHelper.WriteLine("Actual");
            // foreach (var personRes in ListOfPersonResponses)
            // {
            //     _testOutputHelper.WriteLine(personRes.ToString());
            //     Assert.Contains(personRes, actualReturnedPersonResList);
            // }
            personResLst.Should().BeEquivalentTo(actualReturnedPersonResList);
        }
        #endregion

        #region GetFilteredPeople
        [Fact]
        public async Task GetFilteredPeople_EmptySearchKey_ToBeSuccessful()
        {
            // var listOfPersonReqs = new List<AddPersonRequest>()
            // {

            //     //  new AddPersonRequest() {Name="ali", Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true }
            //     // , new AddPersonRequest() { Name="ahmed",Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true }
            //                                         _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create(),
            //                                         _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc2@gmail.com").Create()


            // };
            // var ListOfPersonResponses = new List<PersonResponse>();

            // _testOutputHelper.WriteLine("Expected");
            // foreach (var perReq in listOfPersonReqs)
            // {
            //     var res = await _personsService.AddPerson(perReq);
            //     ListOfPersonResponses.Add(res);
            //     _testOutputHelper.WriteLine(res.ToString());
            // }

            var persons = new List<Person>()
            {
                  _fixture.Build<Person>().With(x => x.Email, "abc1@gmail.com").With(x => x.Country, null as Country).Create(),
                  _fixture.Build<Person>().With(x => x.Email, "abc2@gmail.com").With(x => x.Country, null as Country).Create(),
                  _fixture.Build<Person>().With(x => x.Email, "abc3@gmail.com").With(x => x.Country, null as Country).Create(),

            };
            var personResLst = persons.Select(x => x.ToPersonResponse());
            _mockPersonRepository.Setup(x => x.GetAllPersons()).ReturnsAsync(persons);
            var actualReturnedPersonResList = await _personsService.GetFilteredPeople(nameof(PersonResponse.Name), "");
            // _testOutputHelper.WriteLine("Actual");
            // foreach (var personRes in ListOfPersonResponses)
            // {
            //     _testOutputHelper.WriteLine(personRes.ToString());
            //     Assert.Contains(personRes, actualReturnedPersonResList);
            // }
            actualReturnedPersonResList.Should().BeEquivalentTo(personResLst);

        }
        [Fact]
        public async Task GetFilteredPeople_ContainsProperPeople_ToBeSuccessful()
        {
            var searchKey = "female";
            // var listOfPersonReqs = new List<AddPersonRequest>()
            // {

            //     //  new AddPersonRequest() {Name="ali", Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true }
            //     // , new AddPersonRequest() { Name="ahmed",Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true }
            //     // , new AddPersonRequest() { Name="mona",Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Female, RecieveNewsLetters = true }
            //     // , new AddPersonRequest() { Name="soha",Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Female, RecieveNewsLetters = true }
            //             _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create(),
            //             _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create(),
            //             _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create(),
            //             _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create()


            // };
            // var ListOfPersonResponses = new List<PersonResponse>();

            // foreach (var perReq in listOfPersonReqs)
            // {
            //     var res = await _personsService.AddPerson(perReq);
            //     ListOfPersonResponses.Add(res);
            // }
            // _testOutputHelper.WriteLine("Expected");
            //_testOutputHelper.WriteLine($"{ListOfPersonResponses[0].Gender.ToString()}");
            //_testOutputHelper.WriteLine($"{ListOfPersonResponses[0].Gender}");
            //var expectedList = ListOfPersonResponses.Where(x => x.Name.Contains(searchKey, StringComparison.OrdinalIgnoreCase));
            // var expectedList = ListOfPersonResponses.Where(x => x.Gender.ToString().Contains(searchKey, StringComparison.OrdinalIgnoreCase));
            // foreach (var perRes in expectedList)
            // {
            //     _testOutputHelper.WriteLine(perRes.ToString());
            // }
            var persons = new List<Person>()
            {
                  _fixture.Build<Person>().With(x => x.Email, "abc1@gmail.com").With(x => x.Country, null as Country).Create(),
                  _fixture.Build<Person>().With(x => x.Email, "abc2@gmail.com").With(x => x.Country, null as Country).Create(),
                  _fixture.Build<Person>().With(x => x.Email, "abc3@gmail.com").With(x => x.Country, null as Country).Create(),

            };
            var ExpectedpersonResLst = persons.Select(x => x.ToPersonResponse());
            _mockPersonRepository.Setup(x => x.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
            var actualReturnedPersonResList = await _personsService.GetFilteredPeople(nameof(PersonResponse.Gender), searchKey);
            ExpectedpersonResLst.Should().BeEquivalentTo(actualReturnedPersonResList);
        }
        #endregion


        #region GetSortedPersons


        [Fact]
        public async Task GetSortedPersons_ProperSort_ToBeSuccessful()
        {
            var searchKey = "Female";
            // var listOfPersonReqs = new List<AddPersonRequest>()
            // {

            //     //  new AddPersonRequest() {Name="khlid", Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true }
            //     // , new AddPersonRequest() { Name="ahmed",Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true }
            //     // , new AddPersonRequest() { Name="mona",Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Female, RecieveNewsLetters = true }
            //     // , new AddPersonRequest() { Name="soha",Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Female, RecieveNewsLetters = true }
            //                                         _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create(),
            //             _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create(),
            //             _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create(),
            //             _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create()


            // };
            // var ListOfPersonResponses = new List<PersonResponse>();

            // foreach (var perReq in listOfPersonReqs)
            // {
            //     var res = await _personsService.AddPerson(perReq);
            //     ListOfPersonResponses.Add(res);
            // }
            // _testOutputHelper.WriteLine("Expected");
            // var expectedList = ListOfPersonResponses.OrderByDescending(x => x.Name).ToList();
            // foreach (var perRes in expectedList)
            // {
            //     _testOutputHelper.WriteLine(perRes.ToString());
            // }

            var persons = new List<Person>()
            {
                  _fixture.Build<Person>().With(x => x.Email, "abc1@gmail.com").With(x => x.Country, null as Country).Create(),
                  _fixture.Build<Person>().With(x => x.Email, "abc2@gmail.com").With(x => x.Country, null as Country).Create(),
                  _fixture.Build<Person>().With(x => x.Email, "abc3@gmail.com").With(x => x.Country, null as Country).Create(),

            };
            var expectedList = persons.Select(x => x.ToPersonResponse()).OrderByDescending(x => x.Name).ToList();
            _mockPersonRepository.Setup(x => x.GetAllPersons()).ReturnsAsync(persons);
            var allPersons = await _personsService.GetAllPeople();
            var actualReturnedPersonResList = _personsService.GetSortedPersons(allPersons, nameof(PersonResponse.Name), SortingOrderEnum.Descending);
            _testOutputHelper.WriteLine("Actual");

            for (int i = 0; i < actualReturnedPersonResList.Count; i++)
            {
                _testOutputHelper.WriteLine(actualReturnedPersonResList[i].ToString());
                Assert.Equal(actualReturnedPersonResList[i], expectedList[i]);

            }
        }



        #endregion


        #region UpdatePerson

        [Fact]
        public void UpdatePerson_NullPersonUpdate_ToThrowArgumentNullException()
        {
            PersonUpdateRequest? personupdateReq = null;

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var res = await _personsService.UpdatePerson(personupdateReq);

            });

        }
        [Fact]
        public async Task UpdatePerson_Validate_toThrowArgumentException()
        {
            // var personAddReq = new AddPersonRequest() { Name = "khlid", Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true };
            // var personAddReq = _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create();

            // var res = await _personsService.AddPerson(personAddReq);
            // var personupdateReq = res.ToPersonUpdateRequest();
            // personupdateReq.Name = null;

            var personres = _fixture.Build<PersonResponse>().With(x => x.Email, "abc@gmail.com").Create();
            var personUpdatereq = personres.ToPersonUpdateRequest();
            personUpdatereq.Name = null;





            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var res = await _personsService.UpdatePerson(personUpdatereq);

            });


        }


        [Fact]
        public async Task UpdatePerson_InvalidPersonId_ToThrowArgumentException()
        {
            // var personUpdateReq = new PersonUpdateRequest() { Id = Guid.NewGuid() };
            var personUpdateReq = _fixture.Build<PersonUpdateRequest>().With(x => x.Email, "abc@gmail.com").Create();
            personUpdateReq.Id = Guid.Empty;
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {

                await _personsService.UpdatePerson(personUpdateReq);
            });


        }
        [Fact]
        public async Task UpdatePerson_ProperUpdate_ToBeSuccessful()
        {
            // var personreq = new AddPersonRequest() { Name = "khlid", Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true };
            // var personreq = _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create();
            // var personResfromAdd = await _personsService.AddPerson(personreq);
            // _testOutputHelper.WriteLine("before Update");
            // _testOutputHelper.WriteLine(personResfromAdd.ToString());
            // var updatedPersonReq = personResfromAdd.ToPersonUpdateRequest();
            // updatedPersonReq.Name = "mazin";
            // updatedPersonReq.Email = "zoz@gmail.com";
            // var personResActual = await _personsService.GetPersonByID(personResfromAdd.Id);
            var personUpdateReq = _fixture.Build<PersonUpdateRequest>().With(x => x.Email, "abc@gmail.com").Create();
            personUpdateReq.Name = "ali";
            var person = personUpdateReq.ToPerson();
            var ExpectedPersonRes = person.ToPersonResponse();

            _mockPersonRepository.Setup(x => x.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);
            _mockPersonRepository.Setup(x => x.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);
            _mockPersonRepository.Setup(x => x.GetPersonWithCountryById(It.IsAny<Guid>())).ReturnsAsync(person);
            var personResFromUpdate = await _personsService.UpdatePerson(personUpdateReq);
            // _testOutputHelper.WriteLine("after Update");
            // _testOutputHelper.WriteLine(personResActual.ToString());
            // Assert.True(personResActual.Name == personResFromUpdate.Name && personResActual.Email == personResFromUpdate.Email);
            ExpectedPersonRes.Should().BeEquivalentTo(personResFromUpdate);


        }

        #endregion



        #region DeletePerson

        [Fact]
        public async Task DeletePerson_nullId_ToBeFalse()
        {
            var res = await _personsService.DeletePerson(null);
            Assert.False(res);
        }
        [Fact]
        public async Task DeletePerson_NotExisting_toBeFalse()
        {

            var id = Guid.NewGuid();
            _mockPersonRepository.Setup(x => x.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(null as Person);
            var res = await _personsService.DeletePerson(id);
            Assert.False(res);
        }
        [Fact]
        public async Task DeletePerson_properDeletion_ToBeSuccessful()
        {
            // var personAddReq = new AddPersonRequest() { Name = "khlid", Address = "cairo", CountryId = Guid.NewGuid(), DateOfBirth = DateTime.Parse("2001-01-19"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true };
            // var personAddReq = _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create();
            // var personRes = await _personsService.AddPerson(personAddReq);
            var person = _fixture.Build<Person>().With(x => x.Email, "abc@gmail.com").With(x => x.Country, null as Country).Create();
            _mockPersonRepository.Setup(x => x.DeletePerson(It.IsAny<Person>())).ReturnsAsync(true);
            _mockPersonRepository.Setup(x => x.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);
            var res = await _personsService.DeletePerson(person.Id);
            Assert.True(res);
            // var TryGettingDeletedPerson = await _personsService.GetPersonByID(person.Id);
            // Assert.Null(TryGettingDeletedPerson);
        }
        #endregion
    }

}
