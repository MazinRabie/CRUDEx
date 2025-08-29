using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Entities
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }
        public MyDbContext()
        {

        }


        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<Country>().ToTable("Countries");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            var countriesText = File.ReadAllText("Countries.json");
            List<Country> Countries = JsonSerializer.Deserialize<List<Country>>(countriesText, options)!;

            var personsText = File.ReadAllText("Persons.json");
            List<Person> Persons = JsonSerializer.Deserialize<List<Person>>(personsText, options)!;

            foreach (var country in Countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }
            foreach (var person in Persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }

            modelBuilder.Entity<Person>().HasOne(x => x.Country).WithMany(x => x.Persons).HasForeignKey(x => x.CountryId);

        }
    }
}
