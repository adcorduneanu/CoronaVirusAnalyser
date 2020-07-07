namespace Corona.App.Controllers
{
    using System.Threading.Tasks;
    using Corona.App.Models;
    using Corona.Domain.Providers;
    using Microsoft.AspNetCore.Mvc;

    public partial class CoronaController : Controller
    {
        private readonly ICoronaDataProvider coronaDataProvider;

        public CoronaController(ICoronaDataProvider coronaDataProvider)
        {
            this.coronaDataProvider = coronaDataProvider;
        }        

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
            var location = await this.coronaDataProvider.GetLocalityData(country, locality);

            return LocalityViewModel.ProjectFromDomain(location);
        }
    }
}