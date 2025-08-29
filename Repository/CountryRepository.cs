using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly MyDbContext _db;
        public CountryRepository(MyDbContext db)
        {
            _db = db;
        }

        public async Task<Country?> AddCountry(Country country)
        {
            await _db.Countries.AddAsync(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country?>?> GetAllCountries()
        {
            return await _db.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByExp(Expression<Func<Country, bool>> predicate)
        {
            return await _db.Countries.FirstOrDefaultAsync(predicate);
        }

        public async Task<Country?> GetCountryById(Guid id)
        {
            return await _db.Countries.FirstOrDefaultAsync(x => x.guid == id);
        }
    }
}