using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;
using System.Security.Claims;

namespace Tracking_Tool_System.Pages.Transaction
{
    public class CreateTransactionModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateTransactionModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? TransactionType { get; set; }

        [BindProperty]
        public string? TransactionStatus { get; set; }

        [BindProperty]
        public DateTime DateCreation { get; set; }

        [BindProperty]
        public DateTime DateModification { get; set; } = DateTime.Now;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var user = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                var entity = new TransactionEntity
                {
                    TransactionType = TransactionType,
                    TransactionStatus = TransactionStatus,
                    CreatedBy = user,
                    ModifiedBy = user,
                    DateCreation = now,
                    DateModification = now

                };

                var response = await _apiService.PostAsync("transaction", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result != null && result.CodeError != 0)
                {
                    ModelState.AddModelError(string.Empty, result.MsgError);
                    return Page();
                }

                return RedirectToPage("/Transaction/Transaction_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}