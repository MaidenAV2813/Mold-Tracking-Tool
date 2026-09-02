using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Rol
{
    public class CreateModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? RolDescription { get; set; }

        [BindProperty]
        public string? RolType { get; set; }

        [BindProperty]
        public bool RolStatus { get; set; } = true;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var currentUser =
                    User.Identity?.Name ?? "System";

                var now = DateTime.Now;

                var entity = new RolEntity
                {
                    RolDescription = RolDescription?.Trim(),

                    RolType = RolType?.Trim(),

                    RolStatus = RolStatus,

                    CreatedBy = currentUser,

                    ModifiedBy = currentUser,

                    DateCreation = now,

                    DateModification = now
                };

                var response = await _apiService.PostAsync("roles",entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result == null)
                {
                    TempData["ErrorMessage"] = "La API no devolvió una respuesta válida.";

                    return Page();
                }

                if (result.CodeError != 0)
                {
                    TempData["ErrorMessage"] = result.MsgError ?? "No fue posible crear el rol.";

                    return Page();
                }

                TempData["SuccessMessage"] = result.MsgError ?? "Rol creado correctamente.";

                return RedirectToPage("/Rol/Rol_List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al crear el rol." + ex.Message;

                return Page();
            }
        }
    }
}