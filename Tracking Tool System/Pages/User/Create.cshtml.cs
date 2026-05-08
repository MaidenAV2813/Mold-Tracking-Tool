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
        public int? IdNumber { get; set; }

        [BindProperty]
        public string? EmpName { get; set; }

        [BindProperty]
        public bool UserStatus { get; set; } = true;

        [BindProperty]
        public int? RolID { get; set; }

        public List<RolEntity> Roles { get; set; } = new();

        public async Task OnGet()
        {
            Roles = await _apiService.GetAsync<RolEntity>("roles");
        }
        
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var entity = new UserEntity
            {
                RolID = RolID,
                IdNumber = IdNumber,
                EmpName = EmpName?.Trim(),
                UserStatus = UserStatus,

                CreatedBy = user,
                ModifiedBy = user,
                DateCreation = now,
                DateModification = now
            };

            var response = await _apiService.PostAsync("users", entity);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            return RedirectToPage("/User/User_List");
        }
    }
}
