using Entities;

namespace ServiceContracts.DTOs.CountryDtos
{
    public class CountryResponse
    {
        public string? CountryName { get; set; }
        public Guid guid { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CountryResponse)) return false;
            return guid == ((CountryResponse)obj).guid;

        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }
    public static class CountryExtentsions
    {
        public static CountryResponse? ToCountryResponse(this Country? country)
        {
            if (country == null) return null;
            return new CountryResponse() { CountryName = country.CountryName, guid = country.guid };
        }

    }
}
