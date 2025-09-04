using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDEx.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDtos;
using Services;

namespace CRUDEx.Filters.ActionFilters
{
    public class CreateAndEditPersonActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countryService;
        private readonly ILogger<CreateAndEditPersonActionFilter> _logger;
        private readonly string _actionType;

        public CreateAndEditPersonActionFilter(ICountriesService countryService, string actionType, ILogger<CreateAndEditPersonActionFilter> logger)
        {
            _countryService = countryService;
            _actionType = actionType;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var personController = (PersonController)context.Controller;
            if (!personController.ModelState.IsValid)
            {

                var countries = await _countryService.GetAllCountries();
                personController.ViewBag.Countries = new SelectList(countries, "guid", "CountryName");
                var personReq = (AddPersonRequest)context.ActionArguments["personReq"];
                _logger.LogInformation("code black : wer are in the filter of create&update");
                if (_actionType == "Create")
                    context.Result = personController.View("CreatePersonView", personReq);
                else
                    context.Result = personController.View("UpdatePerson", personReq);
            }
        }
    }
}