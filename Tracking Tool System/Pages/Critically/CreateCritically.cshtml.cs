using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Critically
{
    public class CreateCriticallyModel : PageModel
    {

        private readonly ApiService _apiService;

        public CreateCriticallyModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? CriticallyType { get; set; }

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
                var critically = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                var entity = new CriticallyMoldEntity
                {
                    CriticallyType = CriticallyType,
                    CreatedBy = critically,
                    ModifiedBy = critically,
                    DateCreation = now,
                    DateModification = now

                };

                var response = await _apiService.PostAsync("critically", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result != null && result.CodeError != 0)
                {
                    ModelState.AddModelError(string.Empty, result.MsgError);
                    return Page();
                }

                return RedirectToPage("/Critically/Critically_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}
