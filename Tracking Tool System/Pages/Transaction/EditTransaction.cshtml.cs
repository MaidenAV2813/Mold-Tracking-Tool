using System.Data;
using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Transaction
{
    public class EditTransactionModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditTransactionModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int TransactionTypeID { get; set; }

        [BindProperty]
        public string? TransactionType { get; set; }

        [BindProperty]
        public string? TransactionStatus { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {

            TransactionTypeID = id;

            //Obtener la transaccion a editar
            var transaction = (await _apiService.GetAsync<TransactionEntity>("transaction"))
                .FirstOrDefault(x => x.TransactionTypeID == id);

            if (transaction == null)
                return NotFound();

            // Asignar valores a los controles
            TransactionType = transaction.TransactionType;
            TransactionStatus = transaction.TransactionStatus;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            
            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var transactionList = await _apiService.GetAsync<TransactionEntity>("transaction");

            var entity = new TransactionEntity
            {
                TransactionTypeID = TransactionTypeID,
                TransactionType = TransactionType,
                TransactionStatus = TransactionStatus,
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("transaction", entity);

            if (!response.IsSuccessStatusCode)
            {

                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            return RedirectToPage("/Transaction/Transaction_List");
        }
    }
}