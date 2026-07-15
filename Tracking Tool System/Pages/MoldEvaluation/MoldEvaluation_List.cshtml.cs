using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Evaluation
{
    public class MoldEvaluation_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public MoldEvaluation_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public List<MoldEvaluationEntity> GridList { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                GridList = await _apiService
                    .GetAsync<MoldEvaluationEntity>(
                        "MoldEvaluation");

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return Page();
            }
        }
    }
}