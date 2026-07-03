using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.PartMaintenance
{
    public class DeletePartMaintenanceModel : PageModel
    {
        private readonly ApiService _apiService;

        public DeletePartMaintenanceModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? PartMaintenanceID { get; set; }

        [BindProperty]
        public string? OrderNum { get; set; }

        public string? ItemNumber { get; set; }
        public string? ItemDescription { get; set; }
        public int? QtyAsigned { get; set; }

        public async Task<IActionResult> OnGet(int id, string orderNum)
        {
            OrderNum = orderNum;

            var part = await _apiService.GetSingleAsync<PartMaintenanceEntity>(
                $"PartMaintenance/byid/{id}");

            if (part == null)
                return NotFound();

            PartMaintenanceID = part.PartMaintenanceID;
            ItemNumber = part.ItemNumber;
            ItemDescription = part.ItemDescription;
            QtyAsigned = part.QtyAsigned;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (PartMaintenanceID == null)
            {
                ModelState.AddModelError("", "No se recibió el registro a eliminar.");
                return Page();
            }

            var response = await _apiService.DeleteAsync($"PartMaintenance/{PartMaintenanceID}");

            var result = await response.Content.ReadFromJsonAsync<DBEntity>();

            if (result != null && result.CodeError != 0)
            {
                ModelState.AddModelError("", result.MsgError);
                return Page();
            }

            return RedirectToPage("/PartMaintenance/CreatePartMaintenance", new { orderNum = OrderNum });
        }
    }
}