using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
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

        public List<UserEntity> UserFilterList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SelectedUsername { get; set; }

        public int? SelectedUserID { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                Users = await _apiService.GetAsync<UserEntity>("users");

                UserFilterList = Users
                .GroupBy(x => x.Username)
                .Select(g => g.First())
                .ToList();

                if (!string.IsNullOrWhiteSpace(SelectedUsername))
                {
                    Users = Users
                        .Where(x => x.Username == SelectedUsername)
                        .ToList();

                    SelectedUserID = Users.FirstOrDefault()?.UserID;
                }

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
                var result = await _apiService.PostAsync("users/delete", new UserEntity
                {
                    UserID = id
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

