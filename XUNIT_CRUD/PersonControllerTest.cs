using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using Services;
using FluentAssertions;
using CRUDEx.Controllers;
using CRUDEx.SomeInitialData;
using Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTOs.PersonDtos;

namespace XUNIT_CRUD
{
    public class PersonControllerTest
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICountryRepository _countryRepository;

        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly IFixture _fixture;
        private readonly IPersonService _personService;
        private readonly ICountriesService _CountryService;
        private readonly PersonController _personController;
        private readonly SortFlags sortFlags;
        public PersonControllerTest()
        {
            sortFlags = new SortFlags();
            _mockCountryRepository = new Mock<ICountryRepository>();
            _mockPersonRepository = new Mock<IPersonRepository>();
            _personRepository = _mockPersonRepository.Object;
            _countryRepository = _mockCountryRepository.Object;
            _fixture = new Fixture();
            _personService = new PersonService(_personRepository);
            _CountryService = new CountryService(_countryRepository);
            _personController = new PersonController(_personService, _CountryService, sortFlags);

        }
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            var PersonsLst = new List<Person>()
            {
                _fixture.Build<Person>().With(x=>x.Email,"abc@gmail.com").With(x=>x.Country,null as Country).Create(),
                _fixture.Build<Person>().With(x=>x.Email,"abc@gmail.com").With(x=>x.Country,null as Country).Create(),
                _fixture.Build<Person>().With(x=>x.Email,"abc@gmail.com").With(x=>x.Country,null as Country).Create()
            };
            _mockPersonRepository.Setup(x => x.GetAllPersons()).ReturnsAsync(PersonsLst);
            IActionResult res = await _personController.Index();
            ViewResult view = Assert.IsType<ViewResult>(res);
            // var Model = ((ViewResult)res).Model;
            var Model = view.ViewData.Model;
            var personresLst = PersonsLst.Select(x => x.ToPersonResponse());
            personresLst.Should().BeEquivalentTo((List<PersonResponse>)Model);
        }

        [Fact]
        public async Task CreatePersonPost_WithModelErrors_ToThrowArgumentException()
        {
            var Req = _fixture.Build<AddPersonRequest>().With(x => x.Email, null as string).Create();
            var person = Req.ToPerson();

            var action = async () =>
            {
                await _personController.CreatePersonPost(Req);
            };
            action.Should().ThrowAsync<ArgumentException>().WithMessage("please enter a valid email address");



        }

        [Fact]
        public async Task CreatePersonPost_WithoutModelErrors_ToBeSuccessful()
        {
            var Req = _fixture.Build<AddPersonRequest>().With(x => x.Email, "abc@gmail.com").Create();
            var person = Req.ToPerson();

            _mockPersonRepository.Setup(x => x.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);
            IActionResult res = await _personController.CreatePersonPost(Req);
            Assert.IsType<RedirectToActionResult>(res);

        }
    }
}