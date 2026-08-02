using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Actuator
{
    public class CreateActuatorModel : PageModel
    {

        private readonly ApiService _apiService;

        public CreateActuatorModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? ActuatorType { get; set; }

        [BindProperty]
        public bool ActuatorStatus { get; set; } = true;

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
                var actuator = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                var entity = new ActuatorTypeEntity
                {
                    ActuatorType = ActuatorType,
                    CreatedBy = actuator,
                    ModifiedBy = actuator,
                    DateCreation = DateTime.Now,
                    DateModification = DateTime.Now,
                    ActuatorStatus = ActuatorStatus
                };

                var response = await _apiService.PostAsync("actuator", entity);

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

                TempData["SuccessMessage"] = result.MsgError ?? "Actuador creado correctamente.";

                return RedirectToPage("/Actuator/Actuator_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}
