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
        public bool GateStatus { get; set; } = true;

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
                    DateModification = DateTime.Now,
                    GateStatus = GateStatus

                };

                var response = await _apiService.PostAsync("gates", entity);

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

                TempData["SuccessMessage"] = result.MsgError ?? "Gate creado correctamente.";

                return RedirectToPage("/Gates/Gate_List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al crear el gate." + ex.Message;

                return Page();
            }
        }

    }
}
