using ServiceContracts.DTOs.PersonDtos;
using ServiceContracts.Enums;
namespace ServiceContracts;

public interface IPersonDeleterService
{


    Task<bool> DeletePerson(Guid? id);

}

