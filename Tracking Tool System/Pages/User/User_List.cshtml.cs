using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.User
{
    public class User_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public User_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public List<UserEntity> Users { get; set; } = new();

        public async Task OnGet()
        {
            Users = await _apiService.GetAsync<UserEntity>("users");
        }
    }
}

