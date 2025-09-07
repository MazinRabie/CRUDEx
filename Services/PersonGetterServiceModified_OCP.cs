using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDtos;

namespace Services
{
    public class PersonGetterServiceModified_OCP : IPersonGetterService
    {
        private readonly IPersonGetterService _personGetterService;

        public PersonGetterServiceModified_OCP(PersonGetterService personGetterService)
        {
            _personGetterService = personGetterService;
        }

        public async Task<List<PersonResponse>> GetAllPeople()
        {
            // as if we made some changes 
            return await _personGetterService.GetAllPeople();
        }

        public async Task<List<PersonResponse>> GetFilteredPeople(string? searchBy, string? searchKey)
        {
            return await _personGetterService.GetFilteredPeople(searchBy, searchKey);
        }

        public async Task<Person?> GetPerson(Guid? id)
        {
            return await _personGetterService.GetPerson(id);
        }

        public async Task<PersonResponse?>? GetPersonByID(Guid? id)
        {
            return await _personGetterService.GetPersonByID(id);
        }
    }
}