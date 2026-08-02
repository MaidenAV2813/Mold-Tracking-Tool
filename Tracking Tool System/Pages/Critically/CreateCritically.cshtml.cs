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
        public bool CriticallyStatus { get; set; } = true;

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
                    DateCreation = DateTime.Now,
                    DateModification = DateTime.Now,
                    CriticallyStatus = CriticallyStatus

                };

                var response = await _apiService.PostAsync("critically", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result == null)
                {
                    TempData["ErrorMessage"] = "La API no devolvió una respuesta válida.";

                    return Page();
                }

                if (result.CodeError != 0)
                {
                    TempData["ErrorMessage"] = result.MsgError ?? "No fue posible crear la criticidad.";

                    return Page();
                }

                TempData["SuccessMessage"] = result.MsgError ?? "Criticidad creada correctamente.";

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
