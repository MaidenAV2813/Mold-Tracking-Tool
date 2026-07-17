using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Categorization
{
    public class CreateCategorizationModel : PageModel
    {

        private readonly ApiService _apiService;

        public CreateCategorizationModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? CategorizationType { get; set; }

        [BindProperty]
        public DateTime DateCreation { get; set; }

        [BindProperty]
        public DateTime DateModification { get; set; } = DateTime.Now;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var categorization = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                var entity = new CategorizationMoldEntity
                {
                    CategorizationType = CategorizationType,
                    CreatedBy = categorization,
                    ModifiedBy = categorization,
                    DateCreation = DateTime.Now,
                    DateModification = DateTime.Now

                };

                var response = await _apiService.PostAsync("categorization", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result != null && result.CodeError != 0)
                {
                    ModelState.AddModelError(string.Empty, result.MsgError);
                    return Page();
                }

                return RedirectToPage("/Categorization/Categorization_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}
