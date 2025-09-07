using ServiceContracts.DTOs.PersonDtos;
using ServiceContracts.Enums;
namespace ServiceContracts;

public interface IPersonAdderService
{
    Task<PersonResponse>? AddPerson(AddPersonRequest? PersonAddRequest);

}

