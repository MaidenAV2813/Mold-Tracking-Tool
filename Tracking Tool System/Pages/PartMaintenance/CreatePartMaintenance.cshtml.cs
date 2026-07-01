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
        public int? QtyAsigned { get; set; }

        [BindProperty]
        public int? DeletePartMaintenanceID { get; set; }

        public vw_EBS_WorkOrdersEntity? WorkOrderInfo { get; set; }

        public List<ItemBomEntity> ItemList { get; set; } = new();

        public List<PartMaintenanceEntity> PartList { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            await LoadItems();
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

            if (QtyAsigned == null || QtyAsigned <= 0)
            {
                ModelState.AddModelError("", "Debe digitar una cantidad válida.");
                return Page();
            }

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

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

        public async Task<IActionResult> OnPostEliminar()
        {
            await LoadItems();

            if (DeletePartMaintenanceID == null)
            {
                ModelState.AddModelError("", "No se recibió el registro a eliminar.");
                return Page();
            }

            var response = await _apiService.DeleteAsync($"PartMaintenance/{DeletePartMaintenanceID}");

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