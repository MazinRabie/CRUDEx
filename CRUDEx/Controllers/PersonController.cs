using System.Threading.Tasks;
using CRUDEx.SomeInitialData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDtos;
using ServiceContracts.Enums;

namespace CRUDEx.Controllers
{
    public class PersonController : Controller

    {
        private readonly SortFlags _sortFlags;
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonController(IPersonService personService, ICountriesService countriesService, SortFlags sortFlags)
        {
            _personService = personService;
            _countriesService = countriesService;
            _sortFlags = sortFlags;
        }
        [Route("/")]
        [Route("People/index")]
        public async Task<IActionResult> Index()
        {
            var people = await _personService.GetAllPeople();
            return View(model: people);
        }
        [Route("/SortPersons")]
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
            return View(model: SortedModel, viewName: "index");


        }

        [HttpPost]
        public async Task<IActionResult> Search([FromForm] string searchBy, string searchKey)
        {

            var filtered = await _personService.GetFilteredPeople(searchBy, searchKey);
            return View(model: filtered, viewName: "index");
        }

        [HttpGet]

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

        public async Task<IActionResult> UpdatePersonPost(PersonUpdateRequest personUpdate)
        {
            var person = await _personService.UpdatePerson(personUpdate);
            return RedirectToAction("index");

        }

        [HttpGet]

        public async Task<IActionResult> DeletePerson(Guid id)
        {
            await _personService.DeletePerson(id);
            return RedirectToAction("index");
        }

        [HttpGet]

        public async Task<IActionResult> CreatePersonGet()
        {
            //if (personReq == null) personReq = new AddPersonRequest();
            var countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = new SelectList(countries, "guid", "CountryName");
            return View("CreatePersonView", new AddPersonRequest());
        }
        [HttpPost]

        public async Task<IActionResult> CreatePersonPost(AddPersonRequest personReq)
        {
            await _personService.AddPerson(personReq);
            return RedirectToAction("index");

        }

    }
}
