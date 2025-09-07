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
    public class PersonDeleterService : IPersonDeleterService
    {
        // private readonly ICountriesService _countriesService;
        // private readonly MyDbContext _db;
        private readonly ILogger<PersonDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticsContext;

        private readonly IPersonRepository _personRepository;


        //public PersonService(ICountriesService countriesService)
        //{
        //    people = new List<Person>();
        //    _countriesService = countriesService;
        //    MockData();
        //}
        // public PersonService(IPersonRepository personRepository, MyDbContext myDbContext)
        public PersonDeleterService(IPersonRepository personRepository, ILogger<PersonDeleterService> logger, IDiagnosticContext diagnosticsContext)
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

