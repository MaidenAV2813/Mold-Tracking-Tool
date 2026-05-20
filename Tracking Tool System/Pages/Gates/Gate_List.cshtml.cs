using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Gates
{
    public class Gates_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Gates_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<GateTypeEntity> GridList { get; set; } = new List<GateTypeEntity>();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                GridList = await _apiService.GetAsync<GateTypeEntity>("Gates");
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
                var result = await _apiService.PostAsync("Gates/delete", new GateTypeEntity
                {
                    GateID = id
                });

                var content = await result.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return new JsonResult(new DBEntity
                {
                    CodeError = ex.HResult,
                    MsgError = ex.Message
                });
            }
        }
    }
}
