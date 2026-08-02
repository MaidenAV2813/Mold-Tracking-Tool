using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Actuator
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? ActuatorID { get; set; }

        [BindProperty]
        public string? ActuatorType { get; set; }

        [BindProperty]
        public bool ActuatorStatus { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var actuator = await _apiService.GetSingleAsync<ActuatorTypeEntity>($"actuator/{id}");

            if (actuator == null)
                return NotFound();

            ActuatorID = actuator.ActuatorID;
            ActuatorType = actuator.ActuatorType;
            ActuatorStatus = actuator.ActuatorStatus;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;
            var entity = new ActuatorTypeEntity
            {
                ActuatorID = ActuatorID,
                ActuatorType = ActuatorType,
                ActuatorStatus = ActuatorStatus,
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("actuator/Update", entity);

            var result = await response.Content.ReadFromJsonAsync<DBEntity>();

            if (result == null)
            {
                TempData["ErrorMessage"] = "La API no devolvió una respuesta válida.";

                return Page();
            }

            if (result.CodeError != 0)
            {
                TempData["ErrorMessage"] = result.MsgError ?? "No fue posible crear el actuador.";

                return Page();
            }

            TempData["SuccessMessage"] = result.MsgError ?? "Estado actualizado correctamente.";

            return RedirectToPage("/Actuator/Actuator_List");
        }
    }
}