using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.PartMaintenance
{
    public class CreatePartMaintenanceModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreatePartMaintenanceModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public string? OrderNum { get; set; }

        [BindProperty]
        public int? ItemNumberID { get; set; }

        [BindProperty]
        public string? ItemDescription { get; set; }

        [BindProperty]
        public int? QtyAsigned { get; set; }

        public vw_EBS_WorkOrdersEntity? WorkOrderInfo { get; set; }

        public List<ItemBomEntity> ItemList { get; set; } = new();

        public List<PartMaintenanceEntity> PartList { get; set; } = new();

        [BindProperty]
        public int? EditPartMaintenanceID { get; set; }

        [BindProperty]
        public int? EditItemNumberID { get; set; }

        [BindProperty]
        public int? EditQtyAsigned { get; set; }

        [BindProperty]
        public int? TotalQtyOnHand { get; set; }

        [BindProperty]
        public int? LocationQtyOnHand { get; set; }

        [BindProperty]
        public int? LocationID { get; set; }

        //[BindProperty]
        //public string? LocationNumber { get; set; }

        public async Task<IActionResult> OnGet(string? orderNum)
        {
            await LoadItems();

            if (!string.IsNullOrWhiteSpace(orderNum))
            {
                OrderNum = orderNum;
                await LoadWorkOrder();
                await LoadPartMaintenance();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostBuscar()
        {
            await LoadItems();

            if (string.IsNullOrWhiteSpace(OrderNum))
            {
                ModelState.AddModelError("", "Debe digitar el número de PM.");
                return Page();
            }

            var result = await _apiService.GetAsync<vw_EBS_WorkOrdersEntity>(
                $"vw_EBS_WorkOrders/byorder/{OrderNum}");

            WorkOrderInfo = result.FirstOrDefault();

            if (WorkOrderInfo == null)
            {
                ModelState.AddModelError("", "La orden PM no existe en Oracle.");
                return Page();
            }

            await LoadPartMaintenance();

            return Page();
        }

        public async Task<IActionResult> OnPostAgregar()
        {
            await LoadItems();

            if (string.IsNullOrWhiteSpace(OrderNum))
            {
                ModelState.AddModelError("", "Debe digitar el número de PM.");
                return Page();
            }

            if (ItemNumberID == null)
            {
                ModelState.AddModelError("", "Debe seleccionar un número de parte.");
                return Page();
            }

            if (LocationID == null)
            {
                ModelState.AddModelError("", "Debe seleccionar la localidad desde donde se rebajará el inventario.");

                await LoadWorkOrder();
                await LoadPartMaintenance();
                await LoadItems();

                return Page();
            }

            if (QtyAsigned == null || QtyAsigned <= 0)
            {
                ModelState.AddModelError("", "Debe digitar una cantidad válida.");
                return Page();
            }

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;


            if (QtyAsigned > LocationQtyOnHand)
            {
                ModelState.AddModelError("", "La cantidad a rebajar es mayor que el inventario disponible.");

                await LoadWorkOrder();
                await LoadPartMaintenance();
                await LoadItems();

                return Page();
            }


            var entity = new PartMaintenanceEntity
            {
                OrderNum = OrderNum,
                ItemNumberID = ItemNumberID,
                QtyAsigned = QtyAsigned,
                CreatedBy = user,
                ModifiedBy = user,
                DateCreation = now,
                DateModification = now
            };

            var response = await _apiService.PostAsync("PartMaintenance", entity);

            var result = await response.Content.ReadFromJsonAsync<DBEntity>();

            if (result != null && result.CodeError != 0)
            {
                ModelState.AddModelError("", result.MsgError);
                return Page();
            }

            await LoadWorkOrder();
            await LoadPartMaintenance();
            return Page();

        }

        public async Task<IActionResult> OnGetItemBOH(int itemNumberID)
        {
            var item = await _apiService.GetAsync<ItemBOHPartMaintenanceEntity>(
                $"PartMaintenance/itemboh/{itemNumberID}");

            return new JsonResult(item);
        }

        private async Task LoadItems()
        {
            ItemList = (await _apiService
                .GetAsync<ItemBomEntity>("ItemBom"))
                .OrderBy(x => x.ItemNumber)
                .ToList();
        }

        private async Task LoadPartMaintenance()
        {
            if (!string.IsNullOrWhiteSpace(OrderNum))
            {
                PartList = (await _apiService
                    .GetAsync<PartMaintenanceEntity>($"PartMaintenance/{OrderNum}"))
                    .ToList();
            }
        }

        private async Task LoadWorkOrder()
        {
            if (!string.IsNullOrWhiteSpace(OrderNum))
            {
                var result = await _apiService.GetAsync<vw_EBS_WorkOrdersEntity>(
                    $"vw_EBS_WorkOrders/byorder/{OrderNum}");

                WorkOrderInfo = result.FirstOrDefault();
            }
        }
    }
}