using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CAPA_ENTITY;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Rol
{
    public class Rol_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Rol_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<RolEntity> GridList { get; set; } = new List<RolEntity>();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                GridList = await _apiService.GetAsync<RolEntity>("roles");
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
                var result = await _apiService.PostAsync("roles/delete", new RolEntity
                {
                    RolID = id
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