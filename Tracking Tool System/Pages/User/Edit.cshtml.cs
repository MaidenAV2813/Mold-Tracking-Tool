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
        public int? UserID { get; set; }

        [BindProperty]
        public int? RolID { get; set; }

        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public int? IdNumber { get; set; }

        [BindProperty]
        public string? EmpName { get; set; }

        [BindProperty]
        public bool UserStatus { get; set; }

        [BindProperty]
        public string? Rol { get; set; }

        public async Task<IActionResult> OnGet(decimal id)
        {
            var user = await _apiService.GetByIdAsync<UserEntity>($"users/{id}");

            if (user == null)
                return RedirectToPage("/User/User_List");

            RolID = user.RolID;
            IdNumber = user.IdNumber;
            EmpName = user.EmpName;
            UserStatus = user.UserStatus == true;


            return Page();
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

                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("users", entity);

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
