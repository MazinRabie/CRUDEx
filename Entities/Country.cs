using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Country
    {
        public Country()
        {
            this.guid = Guid.NewGuid();
        }
        public string? CountryName { get; set; }
        [Key]
        public Guid guid { get; set; }
        public ICollection<Person> Persons { get; set; }

    }
}
