using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Access
{
    public class Access_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Access_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public List<AccessEntity> Access { get; set; } = new();

        public List<RolEntity> Roles { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? SelectedRolID { get; set; }


        public async Task OnGet()
        {
            Roles = await _apiService.GetAsync<RolEntity>("roles");
            Access = await _apiService.GetAsync<AccessEntity>("access");

            if (SelectedRolID.HasValue)
            {
                Access = Access
                    .Where(x => x.RolID == SelectedRolID.Value)
                    .ToList();
            }
        }
    }
}

