using System.Threading.Tasks;
using CRUDEx.Filters.ActionFilters;
using CRUDEx.Filters.AuthorizationFilters;
using CRUDEx.Filters.ExceptionFilters;
using CRUDEx.Filters.ResourceFilters;
using CRUDEx.Filters.ResultFilters;
using CRUDEx.Filters.SkipFilter;
using CRUDEx.SomeInitialData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDtos;
using ServiceContracts.Enums;

namespace CRUDEx.Controllers
{
    [TypeFilter(typeof(AddResponseHeaderActionFilter), Arguments = new object[] { "Controller_Header", "Provided via Paramterized Filter", 1 })]
    [TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonAlwaysRunResultFilter))]
    public class PersonController : Controller

    {
        private readonly SortFlags _sortFlags;
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonService personService, ICountriesService countriesService, SortFlags sortFlags, ILogger<PersonController> logger)
        {
            _personService = personService;
            _countriesService = countriesService;
            _sortFlags = sortFlags;
            _logger = logger;
        }
        [Route("/")]
        [Route("People/index")]
        [TypeFilter(typeof(PersonListActionFilter), Order = 4)]
        [TypeFilter(typeof(AddResponseHeaderActionFilter), Arguments = new object[] { "CustomHeader", "Provided via Paramterized Filter", 2 })]
        [TypeFilter(typeof(PersonListResultFilter), Order = 3)]
        // [SkipFilter]
        public async Task<IActionResult> Index()
        {

            _logger.LogInformation("hitting the index action");
            var people = await _personService.GetAllPeople();
            return View(model: people);
        }
        [Route("/SortPersons")]
        [TypeFilter(typeof(SortPersonActionFilter))]
        public async Task<IActionResult> SortPersons(string sortBy)
        {
            SortingOrderEnum sortOrder;
            //var x = ViewBag["NameSortAsc"];
            if (_sortFlags.GetProp(sortBy) == true)
            {
                sortOrder = SortingOrderEnum.Ascending;

            }
            else
            {
                sortOrder = SortingOrderEnum.Descending;


            }
            _sortFlags.SetProp(sortBy);

            var persons = await _personService.GetAllPeople();
            var SortedModel = _personService.GetSortedPersons(persons, sortBy, sortOrder);
            _logger.LogInformation($"sorting people with {sortBy}");
            return View(model: SortedModel, viewName: "index");


        }

        [HttpPost]
        public async Task<IActionResult> Search([FromForm] string searchBy, string searchKey)
        {

            var filtered = await _personService.GetFilteredPeople(searchBy, searchKey);
            return View(model: filtered, viewName: "index");
        }

        [HttpGet]
        [TypeFilter(typeof(AddTokenAuthorizationToCookiesFilter))]
        public async Task<IActionResult> UpdatePersonGet(Guid id)
        {
            var person = (await _personService.GetPersonByID(id))?.ToPersonUpdateRequest();
            if (person == null) return RedirectToAction(nameof(Index));
            var countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = new SelectList(
                countries,
                "guid",         // must match exactly your property name
                "CountryName",  // for dropdown text
                person?.CountryId // preselect value
            );
            return View("UpdatePerson", person);
        }

        [HttpPost]
        // [TypeFilter(typeof(CreateAndEditPersonActionFilter), Arguments = new object[] { "Update" })]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> UpdatePersonPost(PersonUpdateRequest personReq)
        {
            var person = await _personService.UpdatePerson(personReq);
            return RedirectToAction("index");

        }

        [HttpGet]

        public async Task<IActionResult> DeletePerson(Guid id)
        {
            await _personService.DeletePerson(id);
            return RedirectToAction("index");
        }

        [HttpGet]
        // [TypeFilter(typeof(FeatureDisableResourceFilter), Arguments = new object[] { true })]
        // [TypeFilter(typeof(ThrowExceptionFilter))]
        public async Task<IActionResult> CreatePersonGet()
        {
            //if (personReq == null) personReq = new AddPersonRequest();
            var countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "guid", "CountryName");
            return View("CreatePersonView", new AddPersonRequest());
        }
        [HttpPost]
        [TypeFilter(typeof(CreateAndEditPersonActionFilter), Arguments = new object[] { "Create" })]
        public async Task<IActionResult> CreatePersonPost(AddPersonRequest personReq)
        {

            await _personService.AddPerson(personReq);
            return RedirectToAction("index");

        }

    }
}
