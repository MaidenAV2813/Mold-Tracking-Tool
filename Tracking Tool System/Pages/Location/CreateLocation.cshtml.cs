using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;
using System.Security.Claims;

namespace Tracking_Tool_System.Pages.Location
{
    public class CreateLocationModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateLocationModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? LocationNumber { get; set; }

        [BindProperty]
        public string? LocationStatus { get; set; }

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

                var entity = new LocationEntity
                {
                    LocationNumber = LocationNumber,
                    LocationStatus = LocationStatus,
                    CreatedBy = user,
                    ModifiedBy = user,
                    DateCreation = DateTime.Now,
                    DateModification = DateTime.Now

                };

                var response = await _apiService.PostAsync("location", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result != null && result.CodeError != 0)
                {
                    ModelState.AddModelError(string.Empty, result.MsgError);
                    return Page();
                }

                return RedirectToPage("/Location/Location_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}