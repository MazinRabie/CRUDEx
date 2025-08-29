using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDtos;

namespace Services
{
    public class CountryService : ICountriesService

    {
        // private readonly MyDbContext _db;
        private readonly ICountryRepository _countryRepository;
        //public CountryService()
        //{
        //    Countries = new List<Country>();
        //    MocKData();
        //}
        // public CountryService(MyDbContext myDbContext, ICountryRepository countryRepository)
        public CountryService(ICountryRepository countryRepository)
        {
            // _db = myDbContext;
            _countryRepository = countryRepository;
        }

        //private void MocKData()
        //{
        //    var countriesAddReqs = new List<AddCountryRequest>() {
        //        new AddCountryRequest(){CountryName = "USA"} ,
        //        new AddCountryRequest(){CountryName = "Japan"} ,
        //        new AddCountryRequest(){CountryName = "UAE"} ,
        //        new AddCountryRequest(){CountryName = "Spain"} ,
        //        new AddCountryRequest(){CountryName = "UK"} ,
        //        new AddCountryRequest(){CountryName = "Egypt"}

        //    };
        //    var countriesRes = new List<CountryResponse>();

        //    foreach (var req in countriesAddReqs)
        //    {
        //        countriesRes.Add(AddConuntry(req));
        //    }
        //}

        public async Task<CountryResponse> AddConuntry(AddCountryRequest countryRequest)
        {
            if (countryRequest == null) { throw new ArgumentNullException("country can not be null"); }
            if (countryRequest.CountryName == null) { throw new ArgumentException("country name can not be null"); }
            // var CheckDuplicateCountry = await _db.Countries.FirstOrDefaultAsync(x => x.CountryName == countryRequest.CountryName);
            var CheckDuplicateCountry = await _countryRepository.GetCountryByExp(x => x.CountryName == countryRequest.CountryName);
            if (CheckDuplicateCountry != null) throw new ArgumentException("Country is already in the list we don't allow duplicate names");
            var country = countryRequest.ToCountry();
            // await _db.Countries.AddAsync(country);
            // await _db.SaveChangesAsync();
            await _countryRepository.AddCountry(country);
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            // return (await _db.Countries.ToListAsync())?.Select(x => x?.ToCountryResponse()).ToList()!;
            return (await _countryRepository.GetAllCountries())?.Select(x => x?.ToCountryResponse()).ToList()!;
        }

        public async Task<CountryResponse>? GetCountryById(Guid? id)
        {
            if (id == null) return null;
            var country = await _countryRepository.GetCountryById((Guid)id);
            return country.ToCountryResponse();
        }
    }
}
