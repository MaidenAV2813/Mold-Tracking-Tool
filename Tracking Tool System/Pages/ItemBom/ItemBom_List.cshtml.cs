using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CAPA_ENTITY;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.ItemBom
{
    public class ItemBom_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public ItemBom_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<ItemBomEntity> GridList { get; set; } = new List<ItemBomEntity>();

        public List<ItemBomEntity> Part { get; set; } = new();

        public List<ItemBomEntity> PartFilterList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchPart { get; set; }

        public int? SelectedItemNumberID { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                Part = await _apiService.GetAsync<ItemBomEntity>("itembom");

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

        public async Task<IActionResult> OnPostEliminar(int id)
        {
            try
            {
                var result = await _apiService.PostAsync("part/delete", new ItemBomEntity
                {
                    ItemNumberID = id
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