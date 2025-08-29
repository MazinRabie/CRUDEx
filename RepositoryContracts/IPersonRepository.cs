using System.Linq.Expressions;
using Entities;
// using ServiceContracts.Enums;

namespace RepositoryContracts;

public interface IPersonRepository
{

    Task<Person?> AddPerson(Person person);
    Task<List<Person?>> GetAllPersons();
    Task<Person?> GetPersonById(Guid? id);

    // Task<List<Person>> GetFilteredPersons(Expression<Func<Person, string, bool>> predicate);
    // List<Person> GetSortedPersons(List<Person> Person, string? sortBy, SortingOrderEnum sortingOrder);
    Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);
    Task<Person> UpdatePerson(Person? person);
    Task<bool> DeletePerson(Person person);
    Task<Person?> GetPersonWithCountryById(Guid? id);



}
