using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;
using System.Security.Claims;

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

        [BindProperty]
        public DateTime DateCreation { get; set; }

        [BindProperty]
        public DateTime DateModification { get; set; } = DateTime.Now;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var user = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                var entity = new RolEntity
                {
                    RolDescription = RolDescription,
                    RolType = RolType,
                    RolStatus = RolStatus,
                    CreatedBy = user,
                    ModifiedBy = user,
                    DateCreation = now,
                    DateModification = now

                };

                var response = await _apiService.PostAsync("roles", entity);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, error);
                    return Page();
                }

                return RedirectToPage("/Rol/Rol_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}