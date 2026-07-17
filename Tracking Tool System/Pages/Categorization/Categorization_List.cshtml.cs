using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Categorization
{
    public class Categorization_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Categorization_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<CategorizationMoldEntity> GridList { get; set; } = new List<CategorizationMoldEntity>();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                GridList = (await _apiService.GetAsync<CategorizationMoldEntity>("Categorization"))
                    .OrderBy(x => x.CategorizationType);
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
                var response = await _apiService.PostAsync("Categorization/delete", new CategorizationMoldEntity
                {
                    CategorizationID = id
                });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, error);
                    GridList = await _apiService.GetAsync<CategorizationMoldEntity>("Categorization");
                    return Page();
                }

                return RedirectToPage("/Categorization/Categorization_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                GridList = await _apiService.GetAsync<CategorizationMoldEntity>("Categorization");
                return Page();
            }
        }
    }
}
