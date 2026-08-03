using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.MoldEvaluationPart
{
    public class MoldEvaluationPart_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public MoldEvaluationPart_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public List<MoldEvaluationPartEntity> GridList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchPart { get; set; }

        [BindProperty]
        public int? MoldEvaPartID { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await LoadGrid();

            return Page();
        }

        //public async Task<IActionResult> OnPostDelete()
        //{
        //    if (MoldEvaPartID == null)
        //    {
        //        ModelState.AddModelError(
        //            string.Empty,
        //            "Debe seleccionar una parte para eliminar.");

        //        await LoadGrid();

        //        return Page();
        //    }

        //    var response = await _apiService.DeleteAsync(
        //        $"MoldEvaluationPart/{MoldEvaPartID}");

        //    var result = await response.Content
        //        .ReadFromJsonAsync<DBEntity>();

        //    if (result != null && result.CodeError != 0)
        //    {
        //        ModelState.AddModelError(
        //            string.Empty,
        //            result.MsgError ?? "No fue posible eliminar la parte.");

        //        await LoadGrid();

        //        return Page();
        //    }

        //    return RedirectToPage(
        //        "/MoldEvaluationPart/MoldEvaluationPart_List");
        //}

        private async Task LoadGrid()
        {
            GridList = await _apiService
                .GetAsync<MoldEvaluationPartEntity>(
                    "MoldEvaluationPart");

            if (!string.IsNullOrWhiteSpace(SearchPart))
            {
                GridList = GridList
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x.Parts) &&
                        x.Parts.Contains(
                            SearchPart,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
    }
}