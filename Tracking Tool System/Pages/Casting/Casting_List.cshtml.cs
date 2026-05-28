using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Casting
{
    public class Casting_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Casting_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<CastingMoldEntity> GridList { get; set; } = new List<CastingMoldEntity>();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                GridList = (await _apiService.GetAsync<CastingMoldEntity>("Casting"))
                    .OrderBy(x => x.CastingType);
                return Page();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public async Task<IActionResult> OnPostEliminar(int id)
        {
            try
            {
                var response = await _apiService.PostAsync("Casting/delete", new CastingMoldEntity
                {
                    CastingID = id
                });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, error);
                    GridList = await _apiService.GetAsync<CastingMoldEntity>("Casting");
                    return Page();
                }

                return RedirectToPage("/Casting/Casting_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                GridList = await _apiService.GetAsync<CastingMoldEntity>("Casting");
                return Page();
            }
        }
    }
}
