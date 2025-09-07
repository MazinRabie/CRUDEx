using ServiceContracts.DTOs.PersonDtos;
using ServiceContracts.Enums;
namespace ServiceContracts;

public interface IPersonSorterService
{

    List<PersonResponse> GetSortedPersons(List<PersonResponse> personResponses, string? sortBy, SortingOrderEnum sortingOrder);


}

