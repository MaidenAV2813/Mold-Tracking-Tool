using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Gates
{
    public class CreateGateModel : PageModel
    {

        private readonly ApiService _apiService;

        public CreateGateModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? GateType { get; set; }

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
                var gate = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                var entity = new GateTypeEntity
                {
                    GateType = GateType,
                    CreatedBy = gate,
                    ModifiedBy = gate,
                    DateCreation = DateTime.Now,
                    DateModification = DateTime.Now

                };

                var response = await _apiService.PostAsync("gates", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result != null && result.CodeError != 0)
                {
                    ModelState.AddModelError(string.Empty, result.MsgError);
                    return Page();
                }

                return RedirectToPage("/Gates/Gate_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}
