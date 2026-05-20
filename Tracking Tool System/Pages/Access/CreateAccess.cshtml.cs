using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Access
{
    public class CreateAccessModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateAccessModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? RolID { get; set; }

        [BindProperty]
        public List<string> SelectedModules { get; set; } = new();

        public List<RolEntity> Roles { get; set; } = new();

        public List<string> Modules { get; set; } = new()
        {
            "Registro del Molde",
            "Configuracion del Molde",
            "Tipos de Gates",
            "Evaluacion del Molde",
            "Mantenimiento del Molde",
            "Inventario de Repuestos",
            "Reportes e Indicadores",
            "Mantenimientos del sistema",
            "Roles",
            "Usuarios",
            "Accesos",
            "Reportes"
        };

        public async Task OnGet()
        {

            Roles = (await _apiService.GetAsync<RolEntity>("roles"))
                .Where(x => x.RolStatus == true)
                .ToList();
        }
        
        public async Task<IActionResult> OnPost()
        {
            Roles = (await _apiService.GetAsync<RolEntity>("roles"))
                .Where(x => x.RolStatus == true)
                .ToList();

            if (!ModelState.IsValid)
                return Page();

            if (RolID == null)
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar un rol.");
                return Page();
            }

            if (SelectedModules == null || !SelectedModules.Any())
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar al menos un módulo.");
                return Page();
            }

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            foreach (var module in SelectedModules)
            {
                var entity = new AccessEntity
                {
                    RolID = RolID,
                    AccessDescription = module.Trim(),
                    CreatedBy = user,
                    ModifiedBy = user,
                    DateCreation = now,
                    DateModification = now
                };

                var response = await _apiService.PostAsync("access", entity);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, error);
                    return Page();
                }
            }

            return RedirectToPage("/Access/Access_List");
        }
    }
}
