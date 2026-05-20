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

            {
                Roles = await _apiService.GetAsync<RolEntity>("roles");

                if (RolID == null)
                {
                    ModelState.AddModelError(string.Empty, "Debe seleccionar un rol.");
                    return Page();
                }

                var user = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                await _apiService.PostAsync("access/deletebyrol", new AccessEntity
                {
                    RolID = RolID
                });

                foreach (var module in SelectedModules)
                {
                    var entity = new AccessEntity
                    {
                        RolID = RolID,
                        AccessDescription = module.Trim(),
                        ModifiedBy = user,
                        CreatedBy = user,
                        DateCreation = now,
                        DateModification = now
                    };

                    await _apiService.PostAsync("access", entity);
                }

                return RedirectToPage("/Access/Access_List");
            }
        }
    }
}
