using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.ListNumber
{
    public class ListNumber_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public ListNumber_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public List<ListNumberEntity> ListNumber { get; set; } = new();

        public List<MoldEntity> Mold { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? SelectedMoldID { get; set; }

        [BindProperty]
        public int? SelectedListNumberID { get; set; }

        public async Task OnGet()
        {
            await LoadData();
        }

        public async Task<IActionResult> OnPostDelete()
        {
            if (!SelectedListNumberID.HasValue ||
                SelectedListNumberID.Value <= 0)
            {
                TempData["ErrorMessage"] =
                    "Debe seleccionar un número de parte.";

                return RedirectToPage(new
                {
                    SelectedMoldID
                });
            }

            try
            {
                var response = await _apiService.DeleteAsync(
                    $"ListNumber/{SelectedListNumberID.Value}");

                var result =
                    await response.Content.ReadFromJsonAsync<DBEntity>();

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] =
                        result?.MsgError ??
                        "No fue posible eliminar el número de parte.";

                    return RedirectToPage(new
                    {
                        SelectedMoldID
                    });
                }

                if (result != null && result.CodeError != 0)
                {
                    TempData["ErrorMessage"] = result.MsgError;

                    return RedirectToPage(new
                    {
                        SelectedMoldID
                    });
                }

                TempData["SuccessMessage"] =
                    result?.MsgError ??
                    "El número de parte fue eliminado correctamente.";

                return RedirectToPage(new
                {
                    SelectedMoldID
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    $"Error al eliminar: {ex.Message}";

                return RedirectToPage(new
                {
                    SelectedMoldID
                });
            }
        }

        private async Task LoadData()
        {
            Mold = await _apiService.GetAsync<MoldEntity>("mold");

            ListNumber =
                await _apiService.GetAsync<ListNumberEntity>(
                    "listnumber");

            if (SelectedMoldID.HasValue)
            {
                ListNumber = ListNumber
                    .Where(x =>
                        x.MoldID == SelectedMoldID.Value)
                    .ToList();
            }
        }
    }
}

