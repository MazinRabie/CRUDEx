using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTOs.PersonDtos
{
    public class PersonResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double? age { get; set; }
        public Gender Gender { get; set; }
        [Display(Name = "Country")]
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
        public string Address { get; set; }
        public bool RecieveNewsLetters { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;
            return this.Id == ((PersonResponse)obj).Id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"Id : {Id} , name : {Name} ,email : {Email} , age : {age}";
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                Id = Id,
                Name = Name,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Address = Address,
                Gender = Gender,
                CountryId = CountryId,
                RecieveNewsLetters = RecieveNewsLetters

            };
        }
    }
    public static class PersonEx
    {
        public static PersonResponse? ToPersonResponse(this Person? person)
        {
            if (person == null) return null;
            return new PersonResponse()
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                DateOfBirth = person.DateOfBirth,
                RecieveNewsLetters = person.RecieveNewsLetters,
                age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth).TotalDays / 365, MidpointRounding.AwayFromZero) : null
                ,
                CountryName = person.Country?.CountryName

            };
        }

        public static PersonResponse? MapUpdates(this Person? person, PersonUpdateRequest updateRequest)
        {
            if (person == null) return null;

            person.Name = updateRequest.Name;
            person.Email = updateRequest.Email;
            person.Gender = updateRequest.Gender;
            person.CountryId = updateRequest.CountryId;
            person.Address = updateRequest.Address;
            person.DateOfBirth = updateRequest.DateOfBirth;
            person.RecieveNewsLetters = updateRequest.RecieveNewsLetters;
            return person.ToPersonResponse();
        }

    }



}
