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
        public string? ItemNumber { get; set; }

        [BindProperty]
        public int? LocationID { get; set; }

        [BindProperty]
        public int? QtyAsigned { get; set; }

        public string? ItemDescription { get; set; }

        public int? TotalQtyOnHand { get; set; }

        public int? LocationQtyOnHand { get; set; }

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
            ItemNumber = part.ItemNumber;
            LocationID = part.LocationID;
            QtyAsigned = part.QtyAsigned;

            await LoadItemBOH();

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

            if (LocationID == null)
            {
                ModelState.AddModelError("", "Debe seleccionar la localidad.");
                await LoadItemBOH();
                return Page();
            }

            if (QtyAsigned == null || QtyAsigned <= 0)
            {
                ModelState.AddModelError("", "Debe digitar una cantidad válida.");
                await LoadItemBOH();
                return Page();
            }

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var entity = new PartMaintenanceEntity
            {
                PartMaintenanceID = PartMaintenanceID,
                ItemNumberID = ItemNumberID,
                LocationID = LocationID,
                QtyAsigned = QtyAsigned,
                DateModification = now,
                ModifiedBy = user
            };

            var response = await _apiService.PutAsync("PartMaintenance", entity);

            var result = await response.Content.ReadFromJsonAsync<DBEntity>();

            if (result != null && result.CodeError != 0)
            {
                ModelState.AddModelError("", result.MsgError);
                await LoadItemBOH();
                return Page();
            }

            return RedirectToPage("/PartMaintenance/CreatePartMaintenance", new { orderNum = OrderNum });
        }

        public async Task<IActionResult> OnGetItemBOH(int itemNumberID)
        {
            var item = await _apiService.GetAsync<ItemBOHPartMaintenanceEntity>(
                $"PartMaintenance/itemboh/{itemNumberID}");

            return new JsonResult(item);
        }

        private async Task LoadItems()
        {
            ItemList = (await _apiService.GetAsync<ItemBomEntity>("ItemBom"))
                .OrderBy(x => x.ItemNumber)
                .ToList();
        }

        private async Task LoadItemBOH()
        {
            if (ItemNumberID == null)
                return;

            var bohList = await _apiService.GetAsync<ItemBOHPartMaintenanceEntity>(
                $"PartMaintenance/itemboh/{ItemNumberID}");

            var list = bohList.ToList();

            if (!list.Any())
                return;

            ItemDescription = list.FirstOrDefault()?.ItemDescription;

            TotalQtyOnHand = list.Sum(x => x.QtyOnHand ?? 0);

            LocationQtyOnHand = list
                .Where(x => x.LocationID == LocationID)
                .Select(x => x.QtyOnHand)
                .FirstOrDefault();
        }
    }
}