using System.Data;
using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.ItemBom
{
    public class EditItemBomModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditItemBomModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int ItemNumberID { get; set; }

        [BindProperty]
        public int? MoldID { get; set; }

        [BindProperty]
        public string? MoldNumber { get; set; }

        [BindProperty]
        public string? ItemNumber { get; set; }

        [BindProperty]
        public string? ItemDescription { get; set; }

        [BindProperty]
        public int? ItemCost { get; set; }

        [BindProperty]
        public int? ItemInvMin { get; set; }

        [BindProperty]
        public int? ItemInvMax { get; set; }

        [BindProperty]
        public string? ItemSupplierNumber { get; set; }

        [BindProperty]
        public string? ActualSupplier { get; set; }

        [BindProperty]
        public string? UOM { get; set; }

        [BindProperty]
        public string? ItemStatus { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {

            ItemNumberID = id;

            //Obtener el item a editar
            var part = (await _apiService.GetAsync<ItemBomEntity>("itembom"))
                .FirstOrDefault(x => x.ItemNumberID == id);

            if (part == null)
                return NotFound();

            // Asignar valores a los controles

            MoldID = part.MoldID;
            MoldNumber = part.MoldNumber;
            ItemNumber = part.ItemNumber;
            ItemDescription = part.ItemDescription;
            ItemCost = part.ItemCost;
            ItemInvMin = part.ItemInvMin;
            ItemInvMax = part.ItemInvMax;
            ItemSupplierNumber = part.ItemSupplierNumber;
            ActualSupplier = part.ActualSupplier;
            UOM = part.UOM;
            ItemStatus = part.ItemStatus;



            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            
            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var partList = await _apiService.GetAsync<ItemBomEntity>("itembom");

            var entity = new ItemBomEntity
            {
                ItemNumberID = ItemNumberID,
                MoldID = MoldID,
                ItemNumber = ItemNumber,
                ItemDescription = ItemDescription,
                ItemCost = ItemCost,
                ItemInvMin = ItemInvMin,
                ItemInvMax = ItemInvMax,
                ItemSupplierNumber = ItemSupplierNumber,
                ActualSupplier = ActualSupplier,
                UOM = UOM,
                ItemStatus = ItemStatus,
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("itembom", entity);

            if (!response.IsSuccessStatusCode)
            {

                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            return RedirectToPage("/ItemBom/ItemBom_List");
        }
    }
}