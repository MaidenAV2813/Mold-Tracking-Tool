using System.Data;
using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Access
{
    public class EditAccessModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditAccessModel(ApiService apiService)
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
            "Lista de moldes",
            "Crear molde",
            "Configuracion del Molde",
            "Tipos de Gates",
            "Tipos de Coladas",
            "Tipos de Criticidad",
            "Tipos de Actuadores",
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


        public async Task<IActionResult> OnGet(int rolId)
        {

            RolID = rolId;

            Roles = await _apiService.GetAsync<RolEntity>("roles");

            var accessList = await _apiService.GetAsync<AccessEntity>("access");

            SelectedModules = accessList
                .Where(x => x.RolID == RolID)
                .Select(x => x.AccessDescription ?? "")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Roles = await _apiService.GetAsync<RolEntity>("roles");

            if (RolID == null)
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar un rol.");
                return Page();
            }

            SelectedModules ??= new List<string>();

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var currentAccess = await _apiService.GetAsync<AccessEntity>("access");

            var currentRoleAccess = currentAccess
                .Where(x => x.RolID == RolID)
                .ToList();

            await _apiService.PostAsync("access/deletebyrol", new AccessEntity
            {
                RolID = RolID
            });

            foreach (var module in SelectedModules)
            {
                var moduleName = module.Trim();

                var existing = currentRoleAccess
                    .FirstOrDefault(x =>
                        x.AccessDescription != null &&
                        x.AccessDescription.Trim() == moduleName);

                var entity = new AccessEntity
                {
                    RolID = RolID,
                    AccessDescription = moduleName,

                    DateCreation = existing?.DateCreation ?? now,

                    CreatedBy = !string.IsNullOrWhiteSpace(existing?.CreatedBy)
                        ? existing.CreatedBy
                        : user,

                    ModifiedBy = user,
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
