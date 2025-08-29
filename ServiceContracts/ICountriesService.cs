
using ServiceContracts.DTOs.CountryDtos;

namespace ServiceContracts
{
    public interface ICountriesService
    {
        /// <summary>
        /// it adds a country and recieves a country request dto
        /// </summary>
        /// <param name="countryRequest"></param>
        /// <returns> country response dto </returns>
        Task<CountryResponse> AddConuntry(AddCountryRequest countryRequest);

        Task<List<CountryResponse>> GetAllCountries();
        Task<CountryResponse>? GetCountryById(Guid? id);

    }
}
