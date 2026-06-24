using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CAPA_ENTITY;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.InventoryBOH
{
    public class InventoryBOH_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public InventoryBOH_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<InventoryBOHEntity> GridList { get; set; } = new List<InventoryBOHEntity>();

        public List<InventoryBOHEntity> Part { get; set; } = new();

        public List<InventoryBOHEntity> PartFilterList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchPart { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchLocation { get; set; }
        public int? SelectedItemNumberID { get; set; }

        public List<LocationEntity> LocationList { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                Part = await _apiService.GetAsync<InventoryBOHEntity>("inventoryboh");

                PartFilterList = Part
                    .GroupBy(x => x.ItemNumber)
                    .Select(g => g.First())
                    .OrderBy(x => x.ItemNumber)
                    .ToList();

                LocationList = (await _apiService
                    .GetAsync<LocationEntity>("location"))
                    .OrderBy(x => x.LocationNumber)
                    .ToList();

                IEnumerable<InventoryBOHEntity> query = Part;

                if (!string.IsNullOrWhiteSpace(SearchPart))
                {
                    query = query.Where(x =>
                        x.ItemNumber != null &&
                        x.ItemNumber.Contains(SearchPart, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(SearchLocation))
                {
                    query = query.Where(x =>
                        x.LocationNumber != null &&
                        x.LocationNumber.Contains(SearchLocation, StringComparison.OrdinalIgnoreCase));
                }

                GridList = query
                    .OrderBy(x => x.ItemNumber)
                    .ThenBy(x => x.LocationNumber)
                    .ToList();

                SelectedItemNumberID = GridList.FirstOrDefault()?.ItemNumberID;

                return Page();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        
    }
}