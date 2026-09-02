using System.Data;
using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Location
{
    public class EditLocationModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditLocationModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int LocationID { get; set; }

        [BindProperty]
        public string? LocationNumber { get; set; }

        [BindProperty]
        public string? LocationStatus { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {

            LocationID = id;

            //Obtener la localidad a editar
            var location = (await _apiService.GetAsync<LocationEntity>("location"))
                .FirstOrDefault(x => x.LocationID == id);

            if (location == null)
                return NotFound();

            // Asignar valores a los controles
            LocationNumber = location.LocationNumber;
            LocationStatus = location.LocationStatus;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            
            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var locationList = await _apiService.GetAsync<LocationEntity>("location");

            var entity = new LocationEntity
            {
                LocationID = LocationID,
                LocationNumber = LocationNumber,
                LocationStatus = LocationStatus,
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("location", entity);

            if (!response.IsSuccessStatusCode)
            {

                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            return RedirectToPage("/Location/Location_List");
        }
    }
}