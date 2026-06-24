using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;
using System.Security.Claims;

namespace Tracking_Tool_System.Pages.InventoryTransactions
{
    public class CreateInventoryTransactionsModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateInventoryTransactionsModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? ItemNumberID { get; set; }

        [BindProperty]
        public string? ItemNumber { get; set; }

        [BindProperty]
        public int? TransactionTypeID { get; set; }

        [BindProperty]
        public int? TransactionType { get; set; }

        [BindProperty]
        public int LocationID { get; set; }

        [BindProperty]
        public string? LocationNumber { get; set; }

        [BindProperty]
        public int? Qty { get; set; }

        [BindProperty]
        public string? Comments { get; set; }

        [BindProperty]
        public string? ActualSupplier { get; set; }

        [BindProperty]
        public DateTime DateCreation { get; set; }

        public List<TransactionEntity> TransactionList { get; set; } = new();
        public List<LocationEntity> LocationList { get; set; } = new();
        public List<ItemBomEntity> ItemBomList { get; set; } = new();


        public async Task<IActionResult> OnGet()
        {
            TransactionList = (await _apiService
                .GetAsync<TransactionEntity>("Transaction"))
                .OrderBy(x => x.TransactionType)
                .ToList();

            LocationList = (await _apiService
                .GetAsync<LocationEntity>("Location"))
                .OrderBy(x => x.LocationNumber)
                .ToList();

            ItemBomList = (await _apiService
                .GetAsync<ItemBomEntity>("ItemBom"))
                .OrderBy(x => x.ItemNumber)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            if (LocationID == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar una localidad.");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            try
            {
                var user = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                var entity = new InventoryTransactionsEntity

                {
                    ItemNumberID = ItemNumberID,
                    TransactionTypeID = TransactionTypeID,
                    LocationID = LocationID,
                    Qty = Qty,
                    Comments = Comments,
                    CreatedBy = user,
                    DateCreation = now,

                };

                
                var response = await _apiService.PostAsync("inventorytransactions", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result != null && result.CodeError != 0)
                {
                    ModelState.AddModelError(string.Empty, result.MsgError);
                    return Page();
                }

                return RedirectToPage("/InventoryTransactions/InventoryTransactions_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}