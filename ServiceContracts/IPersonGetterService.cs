using Entities;
using ServiceContracts.DTOs.PersonDtos;
using ServiceContracts.Enums;
namespace ServiceContracts;

public interface IPersonGetterService
{
    Task<List<PersonResponse>> GetAllPeople();
    Task<PersonResponse?>? GetPersonByID(Guid? id);
    Task<List<PersonResponse>> GetFilteredPeople(string? searchBy, string? searchKey);
    Task<Person?> GetPerson(Guid? id);

}

