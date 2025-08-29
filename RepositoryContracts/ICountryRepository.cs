using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities;

namespace RepositoryContracts
{
    public interface ICountryRepository
    {
        Task<Country?> AddCountry(Country country);
        Task<List<Country?>?> GetAllCountries();
        Task<Country?> GetCountryById(Guid id);
        Task<Country?> GetCountryByExp(Expression<Func<Country, bool>> predicate);
    }
}