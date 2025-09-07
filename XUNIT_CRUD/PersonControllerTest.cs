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
using Microsoft.Extensions.Logging;
using Serilog;

namespace XUNIT_CRUD
{
    public class PersonControllerTest
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly Mock<ILogger<PersonAdderService>> _adderloggerMock;
        private readonly Mock<ILogger<PersonUpdaterService>> _updaterloggerMock;
        private readonly Mock<ILogger<PersonSorterService>> _sorterloggerMock;
        private readonly Mock<ILogger<PersonDeleterService>> _deleterloggerMock;
        private readonly Mock<ILogger<PersonGetterService>> _getterloggerMock;
        private readonly ILogger<PersonController> _loggerPersonController;
        private readonly Mock<ILogger<PersonController>> _loggerPersonControllerMock;
        private readonly IDiagnosticContext _diagnosticsContext;
        private readonly Mock<IDiagnosticContext> _diagnosticsContextMock;

        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly IFixture _fixture;
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonUpdaterService _personUpdaterService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonSorterService _personSorterService;
        private readonly Mock<IPersonAdderService> _MockpersonAdderService;
        private readonly Mock<IPersonGetterService> _MockpersonGetterService;
        private readonly Mock<IPersonUpdaterService> _MockpersonUpdaterService;
        private readonly Mock<IPersonDeleterService> _MockpersonDeleterService;
        private readonly Mock<IPersonSorterService> _MockpersonSorterService;
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
            // _logger = _loggerMock.Object;
            _loggerPersonControllerMock = new Mock<ILogger<PersonController>>();
            _loggerPersonController = _loggerPersonControllerMock.Object;
            _diagnosticsContextMock = new Mock<IDiagnosticContext>();
            _diagnosticsContext = _diagnosticsContextMock.Object;

            _fixture = new Fixture();
            // _personService = new PersonService(_personRepository, _logger, _diagnosticsContext);
            _CountryService = new CountryService(_countryRepository);
            _adderloggerMock = new Mock<ILogger<PersonAdderService>>();
            _updaterloggerMock = new Mock<ILogger<PersonUpdaterService>>(); ;
            _sorterloggerMock = new Mock<ILogger<PersonSorterService>>(); ;
            _deleterloggerMock = new Mock<ILogger<PersonDeleterService>>(); ;
            _getterloggerMock = new Mock<ILogger<PersonGetterService>>(); ;
            // _personAdderService = new PersonAdderService(_personRepository, adderloggerMock.Object, _diagnosticsContext);
            // _personGetterService = new PersonGetterService(_personRepository, getterloggerMock.Object, _diagnosticsContext);
            // _personUpdaterService = new PersonUpdaterService(_personRepository, updaterloggerMock.Object, _diagnosticsContext, _personGetterService);
            // _personSorterService = new PersonSorterService(_personRepository, sorterloggerMock.Object, _diagnosticsContext);
            // _personDeleterService = new PersonDeleterService(_personRepository, deleterloggerMock.Object, _diagnosticsContext);
            _personAdderService = new Mock<IPersonAdderService>().Object;
            _MockpersonGetterService = new Mock<IPersonGetterService>();
            _personGetterService = _MockpersonGetterService.Object;
            _personUpdaterService = new Mock<IPersonUpdaterService>().Object;
            _personSorterService = new Mock<IPersonSorterService>().Object;
            _personDeleterService = new Mock<IPersonDeleterService>().Object;
            _personController = new PersonController(_CountryService, sortFlags, _loggerPersonController, _personAdderService, _personGetterService, _personUpdaterService, _personDeleterService, _personSorterService);
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
            var personResponses = PersonsLst.Select(x => x.ToPersonResponse()).ToList();
            _MockpersonGetterService.Setup(x => x.GetAllPeople()).ReturnsAsync(personResponses);

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