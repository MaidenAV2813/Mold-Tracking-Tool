using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.PartMaintenance
{
    public class EditPartMaintenanceModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditPartMaintenanceModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? PartMaintenanceID { get; set; }

        [BindProperty]
        public string? OrderNum { get; set; }

        [BindProperty]
        public int? ItemNumberID { get; set; }

        [BindProperty]
        public int? QtyAsigned { get; set; }

        public List<ItemBomEntity> ItemList { get; set; } = new();

        public async Task<IActionResult> OnGet(int id, string orderNum)
        {
            await LoadItems();

            OrderNum = orderNum;

            var part = await _apiService.GetSingleAsync<PartMaintenanceEntity>(
                $"PartMaintenance/byid/{id}");

            if (part == null)
                return NotFound();

            PartMaintenanceID = part.PartMaintenanceID;
            ItemNumberID = part.ItemNumberID;
            QtyAsigned = part.QtyAsigned;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await LoadItems();

            if (PartMaintenanceID == null)
            {
                ModelState.AddModelError("", "No se recibió el registro a editar.");
                return Page();
            }

            if (ItemNumberID == null)
            {
                ModelState.AddModelError("", "Debe seleccionar un número de parte.");
                return Page();
            }

            if (QtyAsigned == null || QtyAsigned <= 0)
            {
                ModelState.AddModelError("", "Debe digitar una cantidad válida.");
                return Page();
            }

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var entity = new PartMaintenanceEntity
            {
                PartMaintenanceID = PartMaintenanceID,
                ItemNumberID = ItemNumberID,
                QtyAsigned = QtyAsigned,
                DateModification = now,
                ModifiedBy = user
            };

            var response = await _apiService.PutAsync("PartMaintenance", entity);

            var result = await response.Content.ReadFromJsonAsync<DBEntity>();

            if (result != null && result.CodeError != 0)
            {
                ModelState.AddModelError("", result.MsgError);
                return Page();
            }

            return RedirectToPage("/PartMaintenance/CreatePartMaintenance", new { orderNum = OrderNum });
        }

        private async Task LoadItems()
        {
            ItemList = (await _apiService.GetAsync<ItemBomEntity>("ItemBom"))
                .OrderBy(x => x.ItemNumber)
                .ToList();
        }
    }
}