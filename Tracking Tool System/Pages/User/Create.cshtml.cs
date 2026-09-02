using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.User
{
    public class CreateModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? EmpName { get; set; }

        [BindProperty]
        public bool UserStatus { get; set; } = true;

        [BindProperty]
        public int? RolID { get; set; }

        public List<RolEntity> Roles { get; set; } = new();

        public async Task OnGet()
        {
            await LoadRoles();
        }

        public async Task<IActionResult> OnPost()
        {
            // Se vuelven a cargar los roles por si la página
            // debe mostrarse nuevamente después de un error.
            await LoadRoles();

            var currentUser =
                User.Identity?.Name ?? "System";

            var now = DateTime.Now;

            var entity = new UserEntity
            {
                RolID = RolID,
                Username = Username?.Trim(),
                EmpName = EmpName?.Trim(),
                UserStatus = UserStatus,

                CreatedBy = currentUser,
                ModifiedBy = currentUser,
                DateCreation = now,
                DateModification = now
            };

            try
            {
                var response = await _apiService.PostAsync(
                    "users",
                    entity
                );

                var result = await response.Content
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
                        "No fue posible crear el usuario.";

                    return Page();
                }

                TempData["SuccessMessage"] =
                    result.MsgError ??
                    "Usuario creado correctamente.";

                return RedirectToPage(
                    "/User/User_List"
                );
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    "Ocurrió un error al crear el usuario. "
                    + ex.Message;

                return Page();
            }
        }

        private async Task LoadRoles()
        {
            Roles = (await _apiService
                .GetAsync<RolEntity>("roles"))
                .Where(x => x.RolStatus == true)
                .OrderBy(x => x.RolDescription)
                .ToList();
        }
    }
}