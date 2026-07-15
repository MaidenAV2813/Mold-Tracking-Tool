using System.Data;
using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.MoldEvaluationPart
{
    public class CreateMoldEvaluationPartModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateMoldEvaluationPartModel(
            ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? Parts { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrWhiteSpace(Parts))
            {
                ModelState.AddModelError(
                    nameof(Parts),
                    "Debe digitar el nombre de la parte.");

                return Page();
            }

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var entity = new MoldEvaluationPartEntity
            {
                Parts = Parts.Trim(),

                DateCreation = DateTime.Now,
                DateModification = DateTime.Now,

                CreatedBy = user,
                ModifiedBy = user
            };

            var response = await _apiService.PostAsync("MoldEvaluationPart",entity);

            var result = await response.Content
                .ReadFromJsonAsync<DBEntity>();

            if (result != null && result.CodeError != 0)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.MsgError ?? "No fue posible guardar la parte.");

                return Page();
            }

            return RedirectToPage(
                "/MoldEvaluationPart/MoldEvaluationPart_List");
        }
    }
}
