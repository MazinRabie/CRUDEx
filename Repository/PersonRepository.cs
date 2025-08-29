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
    public class PersonRepository : IPersonRepository
    {
        private readonly MyDbContext db;
        public PersonRepository(MyDbContext db)
        {
            this.db = db;
        }

        public async Task<Person?> AddPerson(Person person)
        {
            await db.Persons.AddAsync(person);
            await db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePerson(Person person)
        {
            db.Remove(person);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Person?>> GetAllPersons()
        {
            return await db.Persons.Include(x => x.Country).ToListAsync();

        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await db.Persons.Include(z => z.Country).Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid? id)
        {
            return await db.Persons.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Person?> GetPersonWithCountryById(Guid? id)
        {
            return await db.Persons.Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Person> UpdatePerson(Person? person)
        {
            // var personFromDb = await GetPersonById(person.Id);
            // personFromDb.Name = person.Name;
            // personFromDb.Email = person.Email;
            // personFromDb.Gender = person.Gender;
            // personFromDb.CountryId = person.CountryId;
            // personFromDb.Address = person.Address;
            // personFromDb.DateOfBirth = person.DateOfBirth;
            // personFromDb.RecieveNewsLetters = person.RecieveNewsLetters;

            db.Persons.Update(person);
            await db.SaveChangesAsync();
            return person;
        }
    }
}