using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Critically
{
    public class Critically_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Critically_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<CriticallyMoldEntity> GridList { get; set; } = new List<CriticallyMoldEntity>();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                GridList = (await _apiService.GetAsync<CriticallyMoldEntity>("Critically"))
                    .OrderBy(x => x.CriticallyType);
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
                var response = await _apiService.PostAsync("Critically/delete", new CriticallyMoldEntity
                {
                    CriticallyID = id
                });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, error);
                    GridList = await _apiService.GetAsync<CriticallyMoldEntity>("Critically");
                    return Page();
                }

                return RedirectToPage("/Critically/Critically_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                GridList = await _apiService.GetAsync<CriticallyMoldEntity>("Critically");
                return Page();
            }
        }
    }
}
