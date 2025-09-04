using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDtos;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        // private readonly ICountriesService _countriesService;
        // private readonly MyDbContext _db;
        private readonly ILogger<PersonService> _logger;
        private readonly IDiagnosticContext _diagnosticsContext;

        private readonly IPersonRepository _personRepository;
        private readonly Dictionary<string, Func<string, Expression<Func<Person, bool>>>> searchSelector =
                new()
                {
            { "Name",     key => p => p.Name.ToLower().Contains(key.ToLower()) },
            { "Email",    key => p => p.Email.ToLower().Contains(key.ToLower()) },
            { "Address",  key => p => p.Address.ToLower().Contains(key.ToLower()) },
            { "Gender",   key => p => p.Gender.ToString().ToLower().Contains(key.ToLower()) },
            { "Country",  key => p => p.Country.CountryName.ToLower().Contains(key.ToLower()) }
                };


        //public PersonService(ICountriesService countriesService)
        //{
        //    people = new List<Person>();
        //    _countriesService = countriesService;
        //    MockData();
        //}
        // public PersonService(IPersonRepository personRepository, MyDbContext myDbContext)
        public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger, IDiagnosticContext diagnosticsContext)
        {
            // _db = myDbContext;
            _personRepository = personRepository;
            _logger = logger;
            _diagnosticsContext = diagnosticsContext;
            // _countriesService = countriesService;

        }

        // private void MockData()
        // {
        //     var countriesRes = _countriesService.GetAllCountries();


        //     var PersonsAddReqs = new List<AddPersonRequest>()
        //     {

        //           new AddPersonRequest() { Name = "khalid", Address = "cairo", CountryId = countriesRes[0].guid, DateOfBirth = DateTime.Parse("2001-06-2"), Email = "asdin@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true },
        //           new AddPersonRequest() { Name = "ahmed", Address = "alex", CountryId = countriesRes[1].guid, DateOfBirth = DateTime.Parse("2006-01-10"), Email = "asdagdf@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = false },
        //           new AddPersonRequest() { Name = "walid", Address = "damanhour", CountryId = countriesRes[2].guid, DateOfBirth = DateTime.Parse("2001-7-1"), Email = "iodhajs@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true },
        //           new AddPersonRequest() { Name = "raya", Address = "nwieba", CountryId = countriesRes[3].guid, DateOfBirth = DateTime.Parse("1990-01-19"), Email = "pgsdoij@gmil.com", Gender = Entities.Gender.Female, RecieveNewsLetters = true },
        //           new AddPersonRequest() { Name = "wessam", Address = "aswan", CountryId = countriesRes[4].guid, DateOfBirth = DateTime.Parse("2001-01-8"), Email = "oaflnak@gmil.com", Gender = Entities.Gender.Male , RecieveNewsLetters = true },
        //           new AddPersonRequest() { Name = "zahra", Address = "luxor", CountryId = countriesRes[2].guid, DateOfBirth = DateTime.Parse("1988-01-19"), Email = "pfaosfm@gmil.com", Gender = Entities.Gender.Female, RecieveNewsLetters = true },
        //           new AddPersonRequest() { Name = "nariman", Address = "damietta", CountryId = countriesRes[5].guid, DateOfBirth = DateTime.Parse("2005-01-12"), Email = "czxoj@gmil.com", Gender = Entities.Gender.Female, RecieveNewsLetters = false },
        //           new AddPersonRequest() { Name = "ali", Address = "hurghada", CountryId = countriesRes[1].guid, DateOfBirth = DateTime.Parse("2000-01-19"), Email = "pskfljsd@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true },
        //           new AddPersonRequest() { Name = "kareem", Address = "sini", CountryId = countriesRes[0].guid, DateOfBirth = DateTime.Parse("2001-11-15"), Email = "pksmgso@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = true },
        //           new AddPersonRequest() { Name = "medhat", Address = "giza", CountryId = countriesRes[5].guid, DateOfBirth = DateTime.Parse("1991-01-19"), Email = "ajsnf@gmil.com", Gender = Entities.Gender.Male, RecieveNewsLetters = false },
        //           new AddPersonRequest() { Name = "soha", Address = "minofia", CountryId = countriesRes[3].guid, DateOfBirth = DateTime.Parse("1970-01-18"), Email = "ltjsaro@gmil.com", Gender = Entities.Gender.Female, RecieveNewsLetters = true },
        //     };

        //     foreach (var req in PersonsAddReqs)
        //     {
        //         AddPerson(req);
        //     }


        // }
        private async Task<PersonResponse?>? ToPersonResponse(Person? person)
        {
            if (person == null) return null;
            var personRes = person.ToPersonResponse();
            // personRes!.CountryName = (await _db.Persons.Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == person.Id))?.Country?.CountryName!;
            personRes!.CountryName = person.Country?.CountryName!;
            return personRes;
        }
        public async Task<PersonResponse>? AddPerson(AddPersonRequest? PersonAddRequest)
        {
            if (PersonAddRequest == null) throw new ArgumentNullException(nameof(PersonAddRequest));
            //if (PersonAddRequest.Email == null || PersonAddRequest.Name == null || PersonAddRequest.DateOfBirth == null) throw new ArgumentException("invalid person properties");
            ModelValidation.ValidateModel(PersonAddRequest);


            var person = PersonAddRequest.ToPerson();
            var PersonReturned = await _personRepository.AddPerson(person);
            // await _db.Persons.AddAsync(person);
            // await _db.SaveChangesAsync();

            var personRes = await ToPersonResponse(PersonReturned);

            return personRes;
        }

        public async Task<List<PersonResponse>> GetAllPeople()
        {
            _logger.LogInformation("getting all people ");
            // var persons = await _db.Persons.ToListAsync();
            var persons = await _personRepository.GetAllPersons();
            _diagnosticsContext.Set("Persons", persons);
            return persons?.Select(x => x.ToPersonResponse()).ToList()!;
        }
        private async Task<Person?> GetPerson(Guid? id)
        {
            if (id == null) return null;
            // var person = await _db.Persons.FirstOrDefaultAsync(x => x.Id == id);
            var person = await _personRepository.GetPersonById(id);
            return person;
        }
        public async Task<PersonResponse?>? GetPersonByID(Guid? id)
        {
            if (id == null) return null;
            var person = await _personRepository.GetPersonWithCountryById(id);
            return await ToPersonResponse(person);
        }

        public async Task<List<PersonResponse>> GetFilteredPeople(string? searchBy, string? searchKey)
        {

            if (string.IsNullOrEmpty(searchKey)) return await GetAllPeople();
            // var AllPersons = await GetAllPeople();
            // var filtered = AllPersons.Where(p => searchSelector[searchBy](p, searchKey)).ToList();
            // var filtered = await _personRepository.GetFilteredPersons((p, searchKey) => searchSelector[searchBy])
            // .Select(x => x.ToPersonResponse()).ToList();
            var predicate = searchSelector[searchBy](searchKey);

            var filtered = (await _personRepository.GetFilteredPersons(predicate)).Select(x => x.ToPersonResponse()).ToList();

            //(p,key)=>p.Name.Contains(key,StringComparison.OrdinalIgnoreCase) 
            return filtered;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> personResponses, string? sortBy, ServiceContracts.Enums.SortingOrderEnum sortingOrder)
        {
            var listOfRes = new List<PersonResponse>();
            if (sortingOrder == ServiceContracts.Enums.SortingOrderEnum.Ascending)
            {
                switch (sortBy)
                {
                    case (nameof(PersonResponse.Id)):
                        listOfRes = personResponses.OrderBy(x => x.Id).ToList();
                        break;
                    case (nameof(PersonResponse.Name)):
                        listOfRes = personResponses.OrderBy(x => x.Name).ToList();
                        break;
                    case (nameof(PersonResponse.Address)):
                        listOfRes = personResponses.OrderBy(x => x.Address).ToList();
                        break;
                    case (nameof(PersonResponse.Email)):
                        listOfRes = personResponses.OrderBy(x => x.Email).ToList();
                        break;
                    case (nameof(PersonResponse.age)):
                        listOfRes = personResponses.OrderBy(x => x.age).ToList();
                        break;
                    case ("Country"):
                        listOfRes = personResponses.OrderBy(x => x.CountryName).ToList();
                        break;
                    case (nameof(PersonResponse.Gender)):
                        listOfRes = personResponses.OrderBy(x => x.Gender).ToList();
                        break;


                }
            }
            else
            {
                switch (sortBy)
                {
                    case (nameof(PersonResponse.Id)):
                        listOfRes = personResponses.OrderByDescending(x => x.Id).ToList();
                        break;
                    case (nameof(PersonResponse.Name)):
                        listOfRes = personResponses.OrderByDescending(x => x.Name).ToList();
                        break;
                    case (nameof(PersonResponse.Address)):
                        listOfRes = personResponses.OrderByDescending(x => x.Address).ToList();
                        break;
                    case (nameof(PersonResponse.Email)):
                        listOfRes = personResponses.OrderByDescending(x => x.Email).ToList();
                        break;
                    case (nameof(PersonResponse.age)):
                        listOfRes = personResponses.OrderByDescending(x => x.age).ToList();
                        break;
                    case ("Country"):
                        listOfRes = personResponses.OrderByDescending(x => x.CountryName).ToList();
                        break;
                    case (nameof(PersonResponse.Gender)):
                        listOfRes = personResponses.OrderByDescending(x => x.Gender).ToList();
                        break;


                }
            }
            return listOfRes;

        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(personUpdateRequest));
            ModelValidation.ValidateModel(personUpdateRequest);
            var person_from_datastore = await GetPerson(personUpdateRequest?.Id);
            if (person_from_datastore == null) return null;
            person_from_datastore?.MapUpdates(personUpdateRequest);
            // await _db.SaveChangesAsync();
            await _personRepository.UpdatePerson(person_from_datastore);
            return await GetPersonByID(personUpdateRequest.Id);
        }
        public async Task<bool> DeletePerson(Guid? id)
        {
            if (id == null) return false;
            // var person = await _db.Persons.FirstOrDefaultAsync(x => x.Id == id);
            var person = await _personRepository.GetPersonById(id);
            if (person == null) return false;
            // _db.Remove(person);
            // await _db.SaveChangesAsync();
            return await _personRepository.DeletePerson(person);
        }

    }

}

