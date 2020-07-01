namespace Corona.App.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Corona.App.Models;
    using Corona.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Country = Models.Country;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            using var storageContext = new StorageContext();

            var countries = await storageContext.Countries
                .Include(x => x.Localities)
                .Select(x => new Country
                {
                    Name = x.Name,
                    Localities = x.Localities
                        .OrderBy(x => x.Name)
                        .Select(y => y.Name)
                        .ToList()
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            var viewModel = new CountryViewModel
            {
                Countries = countries,
                Country = countries.FirstOrDefault()?.Name,
                Locality = countries.FirstOrDefault()?.Localities?.FirstOrDefault()
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
