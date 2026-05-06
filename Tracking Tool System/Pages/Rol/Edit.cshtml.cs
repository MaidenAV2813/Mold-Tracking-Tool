using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Rol
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? RolID { get; set; }

        [BindProperty]
        public string? RolDescription { get; set; }

        [BindProperty]
        public string? RolType { get; set; }

        [BindProperty]
        public bool? Status { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var role = await _apiService.GetSingleAsync<RolEntity>($"roles/{id}");

            if (role == null)
                return NotFound();

            RolID = role.RolID;
            RolDescription = role.RolDescription;
            RolType = role.RolType;
            Status = role.Status;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var entity = new RolEntity
            {
                RolID = RolID,
                RolDescription = RolDescription,
                RolType = RolType,
                Status = Status
            };

            var response = await _apiService.PutAsync("roles", entity);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return Content(error);
            }

            return RedirectToPage("/Rol/Rol_List");
        }
    }
}