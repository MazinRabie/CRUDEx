using Entities;

namespace ServiceContracts.DTOs.CountryDtos

{
    public class AddCountryRequest
    {
        public string? CountryName { get; set; }
        public Country ToCountry()
        {
            return new Country()
            {
                CountryName = CountryName
            };
        }

    }
}
