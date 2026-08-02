using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Actuator
{
    public class Actuator_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Actuator_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<ActuatorTypeEntity> GridList { get; set; } = new List<ActuatorTypeEntity>();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                GridList = (await _apiService.GetAsync<ActuatorTypeEntity>("Actuator"))
                    .OrderBy(x => x.ActuatorType);
                return Page();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        //public async Task<IActionResult> OnPostEliminar(int id)
        //{
        //    try
        //    {
        //        var response = await _apiService.PostAsync("Actuator/delete", new ActuatorTypeEntity
        //        {
        //            ActuatorID = id
        //        });

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            var error = await response.Content.ReadAsStringAsync();
        //            ModelState.AddModelError(string.Empty, error);
        //            GridList = await _apiService.GetAsync<ActuatorTypeEntity>("Actuator");
        //            return Page();
        //        }

        //        return RedirectToPage("/Actuator/Actuator_List");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);
        //        GridList = await _apiService.GetAsync<ActuatorTypeEntity>("Actuator");
        //        return Page();
        //    }
        //}
    }
}
