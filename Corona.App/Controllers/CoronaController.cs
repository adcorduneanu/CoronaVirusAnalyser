namespace Corona.App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Corona.App.Models;
    using Corona.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public partial class CoronaController : Controller
    {
        public async Task<IActionResult> IndexAsync(string country, string locality)
        {
            var viewModel = await GetLocalityData(country, locality);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LocalityDataAsync(string country, string locality)
        {
            var viewModel = await GetLocalityData(country, locality);

            return PartialView("_Chart", viewModel);
        }

        private async Task<LocalityViewModel> GetLocalityData(string country, string locality)
        {
            using var storageContext = new StorageContext();

            var location = await storageContext.Localities
                .Include(x => x.Country)
                .Include(x => x.Sicks)
                .FirstOrDefaultAsync(x => x.Name.ToLower() == locality.ToLower()
                    && x.Country.Name.ToLower() == country.ToLower()
                );

            var dateValues = new List<DateValuePair>();
            location.Sicks
                .OrderBy(x => x.Date)
                .ToList()
                .ForEach(x =>
                {
                    var previousValue = dateValues.Any()
                        ? dateValues.Last().Value
                        : x.Records;
                    dateValues.Add(new DateValuePair(x.Date, x.Records, previousValue));
                });

            var viewModel = new LocalityViewModel
            {
                CountryName = location.Country.Name,
                LocalityName = location.Name,
                DateValues = dateValues
            };

            return viewModel;
        }
    }
}