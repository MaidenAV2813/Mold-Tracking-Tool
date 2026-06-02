using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CAPA_ENTITY;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Location
{
    public class Location_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Location_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<LocationEntity> GridList { get; set; } = new List<LocationEntity>();

        public List<LocationEntity> Location { get; set; } = new();

        public List<LocationEntity> LocationFilterList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchLocation { get; set; }

        public int? SelectedLocationID { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                Location = await _apiService.GetAsync<LocationEntity>("location");

                LocationFilterList = Location
                    .GroupBy(x => x.LocationNumber)
                    .Select(g => g.First())
                    .OrderBy(x => x.LocationNumber)
                    .ToList();

                GridList = Location;

                if (!string.IsNullOrWhiteSpace(SearchLocation))
                {
                    GridList = Location
                        .Where(x => x.LocationNumber != null &&
                                    x.LocationNumber.Contains(SearchLocation, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    SelectedLocationID = GridList.FirstOrDefault()?.LocationID;
                }

                return Page();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public async Task<IActionResult> OnPostEliminar(int id)
        {
            try
            {
                var result = await _apiService.PostAsync("location/delete", new LocationEntity
                {
                    LocationID = id
                });

                var content = await result.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return new JsonResult(new DBEntity
                {
                    CodeError = ex.HResult,
                    MsgError = ex.Message
                });
            }
        }
    }
}