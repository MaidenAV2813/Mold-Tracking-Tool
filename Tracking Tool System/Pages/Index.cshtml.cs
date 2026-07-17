using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;

        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public DashboardEntity Dashboard { get; set; }
            = new();

        public async Task OnGet()
        {
            Dashboard =
                await _apiService
                    .GetSingleAsync<DashboardEntity>(
                        "Dashboard")
                ?? new DashboardEntity();
        }
    }
}
