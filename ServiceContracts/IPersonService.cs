using ServiceContracts.DTOs.PersonDtos;
using ServiceContracts.Enums;
namespace ServiceContracts;

public interface IPersonService
{
    Task<PersonResponse>? AddPerson(AddPersonRequest? PersonAddRequest);
    Task<List<PersonResponse>> GetAllPeople();
    Task<PersonResponse?>? GetPersonByID(Guid? id);
    Task<List<PersonResponse>> GetFilteredPeople(string? searchBy, string? searchKey);
    List<PersonResponse> GetSortedPersons(List<PersonResponse> personResponses, string? sortBy, SortingOrderEnum sortingOrder);
    Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

    Task<bool> DeletePerson(Guid? id);

}

