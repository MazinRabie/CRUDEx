using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTOs.PersonDtos
{
    public class AddPersonRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "please enter a valid email address")]
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public Guid CountryId { get; set; }
        public string Address { get; set; }
        public bool RecieveNewsLetters { get; set; }
        public Person ToPerson()
        {
            return new Person()
            {
                Name = Name,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                CountryId = CountryId,
                Address = Address,
                RecieveNewsLetters = RecieveNewsLetters

            };
        }
    }

}
