using ServiceContracts.DTOs.PersonDtos;
using ServiceContracts.Enums;
namespace ServiceContracts;

public interface IPersonUpdaterService
{
    Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

}

