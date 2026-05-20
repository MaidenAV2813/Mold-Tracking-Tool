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



            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var entity = new UserEntity
            {
                RolID = RolID,
                Username = Username,
                EmpName = EmpName?.Trim(),
                UserStatus = UserStatus,
                CreatedBy = user,
                ModifiedBy = user,
                DateCreation = now,
                DateModification = now
            };

            var response = await _apiService.PostAsync("users", entity);


            var result = await response.Content.ReadFromJsonAsync<DBEntity>();

            if (result != null && result.CodeError != 0)
            {
                Roles = (await _apiService.GetAsync<RolEntity>("roles"))
                    .Where(x => x.RolStatus == true)
                    .ToList();

                ModelState.AddModelError(string.Empty, result.MsgError);
                return Page();
            }

            return RedirectToPage("/User/User_List");
        }
    }
}
