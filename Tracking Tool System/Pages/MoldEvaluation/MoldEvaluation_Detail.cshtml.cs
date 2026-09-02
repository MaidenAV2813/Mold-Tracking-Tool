using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.MoldEvaluation
{
    public class MoldEvaluation_DetailModel : PageModel
    {
        private readonly ApiService _apiService;

        public MoldEvaluation_DetailModel(
            ApiService apiService)
        {
            _apiService = apiService;
        }

        public MoldEvaluationEntity Evaluation { get; set; }
            = new();

        public List<MoldPartEvaluationEntity> EvaluationParts
        { get; set; } = new();

        public async Task<IActionResult> OnGet(int id)
        {
            if (id <= 0)
            {
                return RedirectToPage(
                    "/MoldEvaluation/MoldEvaluation_List");
            }

            var evaluation =
                await _apiService
                    .GetSingleAsync<MoldEvaluationEntity>(
                        $"MoldEvaluation/{id}");

            if (evaluation == null)
            {
                TempData["ErrorMessage"] =
                    "No se encontró la evaluación seleccionada.";

                return RedirectToPage(
                    "/MoldEvaluation/MoldEvaluation_List");
            }

            Evaluation = evaluation;

            EvaluationParts =
                await _apiService
                    .GetAsync<MoldPartEvaluationEntity>(
                        $"MoldEvaluation/{id}/parts");

            return Page();
        }
    }
}
