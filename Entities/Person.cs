using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Person
    {
        public Person()
        {
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }
        [StringLength(40)]
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        [ForeignKey("Country")]
        public Guid CountryId { get; set; }
        public string Address { get; set; }
        public bool RecieveNewsLetters { get; set; }
        public Country Country { get; set; }

        public override string ToString()
        {
            var props = GetType().GetProperties();
            return string.Join(", ", props.Select(p => $"{p.Name}={p.GetValue(this)}"));
        }


    }
    public enum Gender
    {
        Male,
        Female
    }
}
