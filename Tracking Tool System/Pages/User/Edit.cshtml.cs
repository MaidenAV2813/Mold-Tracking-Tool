using System.Data;
using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.User
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int UserID { get; set; }

        [BindProperty]
        public int? RolID { get; set; }

        public string? RolDescription { get; set; }

        public List<RolEntity> Roles { get; set; } = new();

        [BindProperty]
        public String? Username { get; set; }

        [BindProperty]
        public string? EmpName { get; set; }

        [BindProperty]
        public bool UserStatus { get; set; }

        [BindProperty]
        public string? Rol { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {

            Roles = (await _apiService.GetAsync<RolEntity>("roles"))
                .Where(x => x.RolStatus == true)
                .ToList();

            var user = await _apiService.GetByIdAsync<UserEntity>($"users/{id}");

            if (user == null)
                return RedirectToPage("/User/User_List");

            UserID = user.UserID;
            RolID = user.RolID;
            RolDescription = user.RolDescription;
            Username = user.Username;
            EmpName = user.EmpName;
            UserStatus = user.UserStatus == true;


            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Roles = (await _apiService.GetAsync<RolEntity>("roles"))
                    .Where(x => x.RolStatus == true)
                    .ToList();
                return Page();
            }

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var accessList = await _apiService.GetAsync<AccessEntity>("access");

            bool rolTieneAccesos = accessList.Any(x => x.RolID == RolID);

            if (!rolTieneAccesos)
            {
                Roles = await _apiService.GetAsync<RolEntity>("roles");

                ModelState.AddModelError(string.Empty, "No se puede asignar este rol porque no tiene accesos configurados.");
                return Page();
            }



            var entity = new UserEntity
            {
                UserID = UserID,
                RolID = RolID,
                RolDescription = RolDescription,
                Username = Username,
                EmpName = EmpName?.Trim(),
                UserStatus = UserStatus,
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("users", entity);

            if (!response.IsSuccessStatusCode)
            {

                Roles = await _apiService.GetAsync<RolEntity>("roles");
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            return RedirectToPage("/User/User_List");
        }
    }
}
