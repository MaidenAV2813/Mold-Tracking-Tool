using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Categorization
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? CategorizationID { get; set; }

        [BindProperty]
        public string? CategorizationType { get; set; }

        [BindProperty]
        public bool CategorizationStatus { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var categorization = await _apiService.GetSingleAsync<CategorizationMoldEntity>($"categorization/{id}");

            if (categorization == null)
                return NotFound();

            CategorizationID = categorization.CategorizationID;
            CategorizationType = categorization.CategorizationType;
            CategorizationStatus = categorization.CategorizationStatus;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;
            var entity = new CategorizationMoldEntity
            {
                CategorizationID = CategorizationID,
                CategorizationType = CategorizationType,
                CategorizationStatus = CategorizationStatus,
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("categorization/Update", entity);

            var result = await response.Content.ReadFromJsonAsync<DBEntity>();

            if (result == null)
            {
                TempData["ErrorMessage"] = "La API no devolvió una respuesta válida.";

                return Page();
            }

            if (result.CodeError != 0)
            {
                TempData["ErrorMessage"] = result.MsgError ?? "No fue posible crear la categorización.";

                return Page();
            }

            TempData["SuccessMessage"] = result.MsgError ?? "Estado actualizado correctamente.";

            return RedirectToPage("/Categorization/Categorization_List");
        }
    }
}