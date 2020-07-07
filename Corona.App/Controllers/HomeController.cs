namespace Corona.App.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Corona.App.Models;
    using Corona.Domain.Providers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICoronaDataProvider coronaDataProvider;

        public HomeController(ILogger<HomeController> logger, ICoronaDataProvider coronaDataProvider)
        {
            this.logger = logger;
            this.coronaDataProvider = coronaDataProvider;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var countries = await this.coronaDataProvider.GetCountries();
            
            return View(CountryViewModel.ProjectFromDomain(countries));
        }

        public async Task<IActionResult> AllAsync()
        {
            var localities = await this.coronaDataProvider.GetLocalitiesData();

            return View(localities.Select(LocalityViewModel.ProjectFromDomain));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
