using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.MoldEvaluationPart
{
    public class EditMoldEvaluationPartModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditMoldEvaluationPartModel(
            ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? MoldEvaPartID { get; set; }

        [BindProperty]
        public string? Parts { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var entity = await _apiService
                .GetSingleAsync<MoldEvaluationPartEntity>(
                    $"MoldEvaluationPart/byid/{id}");

            if (entity == null)
            {
                return NotFound();
            }

            MoldEvaPartID = entity.MoldEvaPartID;
            Parts = entity.Parts;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (MoldEvaPartID == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No se recibió el registro a actualizar.");

                return Page();
            }

            if (string.IsNullOrWhiteSpace(Parts))
            {
                ModelState.AddModelError(
                    nameof(Parts),
                    "Debe digitar el nombre de la parte.");

                return Page();
            }

            var user = User.Identity?.Name ?? "System";

            var entity = new MoldEvaluationPartEntity
            {
                MoldEvaPartID = MoldEvaPartID,
                Parts = Parts.Trim(),

                DateModification = DateTime.Now,
                ModifiedBy = user
            };

            var response = await _apiService.PutAsync(
                "MoldEvaluationPart",
                entity);

            var result = await response.Content
                .ReadFromJsonAsync<DBEntity>();

            if (result != null && result.CodeError != 0)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.MsgError ?? "No fue posible actualizar la parte.");

                return Page();
            }

            return RedirectToPage(
                "/MoldEvaluationPart/MoldEvaluationPart_List");
        }
    }
}