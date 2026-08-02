using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Gates
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? GateID { get; set; }

        [BindProperty]
        public string? GateType { get; set; }

        [BindProperty]
        public bool GateStatus { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var gate = await _apiService.GetSingleAsync<GateTypeEntity>($"gates/{id}");

            if (gate == null)
                return NotFound();

            GateID = gate.GateID;
            GateType = gate.GateType;
            GateStatus = gate.GateStatus;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;
            var entity = new GateTypeEntity
            {
                GateID = GateID,
                GateType = GateType,
                GateStatus = GateStatus,
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("gates/Update", entity);

            var result = await response.Content.ReadFromJsonAsync<DBEntity>();

            if (result == null)
            {
                TempData["ErrorMessage"] = "La API no devolvió una respuesta válida.";

                return Page();
            }

            if (result.CodeError != 0)
            {
                TempData["ErrorMessage"] = result.MsgError ?? "No fue posible crear el gate.";

                return Page();
            }

            TempData["SuccessMessage"] = result.MsgError ?? "Estado actualizado correctamente.";

            return RedirectToPage("/Gates/Gate_List");
        }
    }
}