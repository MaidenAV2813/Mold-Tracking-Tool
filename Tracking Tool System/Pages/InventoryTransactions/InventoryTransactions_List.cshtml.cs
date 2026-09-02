using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CAPA_ENTITY;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.InventoryTransactions
{
    public class InventoryTransactions_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public InventoryTransactions_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<InventoryTransactionsEntity> GridList { get; set; } = new List<InventoryTransactionsEntity>();

        public List<InventoryTransactionsEntity> Part { get; set; } = new();

        public List<InventoryTransactionsEntity> PartFilterList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchPart { get; set; }

        public int? SelectedItemNumberID { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                Part = await _apiService.GetAsync<InventoryTransactionsEntity>("inventorytransactions");

                PartFilterList = Part
                    .GroupBy(x => x.ItemNumber)
                    .Select(g => g.First())
                    .OrderBy(x => x.ItemNumber)
                    .ToList();

                GridList = Part;

                if (!string.IsNullOrWhiteSpace(SearchPart))
                {
                    GridList = Part
                        .Where(x => x.ItemNumber != null &&
                                    x.ItemNumber.Contains(SearchPart, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    SelectedItemNumberID = GridList.FirstOrDefault()?.ItemNumberID;
                }


                return Page();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        
    }
}