using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Critically
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? CriticallyID { get; set; }

        [BindProperty]
        public string? CriticallyType { get; set; }

        [BindProperty]
        public bool CriticallyStatus { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var critically = await _apiService.GetSingleAsync<CriticallyMoldEntity>($"critically/{id}");

            if (critically == null)
                return NotFound();

            CriticallyID = critically.CriticallyID;
            CriticallyType = critically.CriticallyType;
            CriticallyStatus = critically.CriticallyStatus;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;
            var entity = new CriticallyMoldEntity
            {
                CriticallyID = CriticallyID,
                CriticallyType = CriticallyType,
                CriticallyStatus = CriticallyStatus,
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("critically/Update", entity);

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

            TempData["SuccessMessage"] = result.MsgError ?? "Estado actualizado correctamente.";

            return RedirectToPage("/Critically/Critically_List");
        }
    }
}