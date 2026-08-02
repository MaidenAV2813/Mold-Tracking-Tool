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
            "Lista de moldes",
            "Crear molde",
            "Configuración del Molde",
            "Tipos de Gates",
            "Tipos de Coladas",
            "Tipos de Criticidad",
            "Tipos de Actuadores",
            "Tipos de Categorización",
            "Evaluación del Molde",
            "Partes del molde",
            "Evaluación",
            "Mantenimiento del Molde",
            "Consultar ordenes",
            "Inventario de Repuestos",
            "Localidades",
            "Tipos de Transacciones",
            "Inventario",
            "Transacciones de Inventario",
            "BOM de Repuestos/Moldes",
            "ListNumbers",
            "Reportes e Indicadores",
            "Reporte de Moldes",
            "Reporte de Evaluaciones de Moldes",
            "Reporte de Partes/BOM",
            "Reporte Evaluaciones Pendientes",
            "Mantenimientos del sistema",
            "Roles",
            "Usuarios",
            "Accesos",
            "Reportes"
        };

        public async Task OnGet()
        {
            await LoadRoles();
        }

        public async Task<IActionResult> OnPost()
        {
            // Se cargan nuevamente los roles por si hay que retornar la página.
            await LoadRoles();

            if (RolID == null)
            {
                TempData["ErrorMessage"] =
                    "Debe seleccionar un rol.";

                return Page();
            }

            if (SelectedModules == null ||
                !SelectedModules.Any())
            {
                TempData["ErrorMessage"] =
                    "Debe seleccionar al menos un módulo.";

                return Page();
            }

            var currentUser =
                User.Identity?.Name ?? "System";

            var now = DateTime.Now;

            try
            {
                foreach (var module in SelectedModules)
                {
                    var entity = new AccessEntity
                    {
                        RolID = RolID,
                        AccessDescription =
                            module.Trim(),

                        CreatedBy =
                            currentUser,

                        ModifiedBy =
                            currentUser,

                        DateCreation =
                            now,

                        DateModification =
                            now
                    };

                    var response =
                        await _apiService.PostAsync(
                            "access",
                            entity
                        );

                    var result =
                        await response.Content
                            .ReadFromJsonAsync<DBEntity>();

                    if (result == null)
                    {
                        TempData["ErrorMessage"] =
                            "La API no devolvió una respuesta válida.";

                        return Page();
                    }

                    if (result.CodeError != 0)
                    {
                        TempData["ErrorMessage"] =
                            result.MsgError ??
                            "No fue posible asignar los accesos.";

                        return Page();
                    }
                }

                TempData["SuccessMessage"] =
                    "Los accesos fueron asignados correctamente.";

                return RedirectToPage(
                    "/Access/Access_List"
                );
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    "Ocurrió un error al asignar los accesos. "
                    + ex.Message;

                return Page();
            }
        }

        private async Task LoadRoles()
        {
            Roles = (await _apiService
                .GetAsync<RolEntity>("roles"))
                .Where(x => x.RolStatus == true)
                .ToList();
        }
    }
}
