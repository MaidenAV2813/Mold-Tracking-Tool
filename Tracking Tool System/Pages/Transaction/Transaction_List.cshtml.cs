using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CAPA_ENTITY;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Transaction
{
    public class Transaction_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Transaction_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<TransactionEntity> GridList { get; set; } = new List<TransactionEntity>();

        public List<TransactionEntity> Transaction { get; set; } = new();

        public List<TransactionEntity> TransactionFilterList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTransaction { get; set; }

        public int? SelectedTransactionTypeID { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                Transaction = await _apiService.GetAsync<TransactionEntity>("transaction");

                TransactionFilterList = Transaction
                    .GroupBy(x => x.TransactionType)
                    .Select(g => g.First())
                    .OrderBy(x => x.TransactionType)
                    .ToList();

                GridList = Transaction;

                if (!string.IsNullOrWhiteSpace(SearchTransaction))
                {
                    GridList = Transaction
                        .Where(x => x.TransactionType != null &&
                                    x.TransactionType.Contains(SearchTransaction, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    SelectedTransactionTypeID = GridList.FirstOrDefault()?.TransactionTypeID;
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
                var result = await _apiService.PostAsync("transaction/delete", new TransactionEntity
                {
                    TransactionTypeID = id
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