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
                    DateCreation = now,
                    DateModification = now

                };

                var response = await _apiService.PostAsync("actuator", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result != null && result.CodeError != 0)
                {
                    ModelState.AddModelError(string.Empty, result.MsgError);
                    return Page();
                }

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
