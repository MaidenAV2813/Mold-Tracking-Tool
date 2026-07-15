using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;
using System.Security.Claims;

namespace Tracking_Tool_System.Pages.ItemBom
{
    public class CreateItemBomModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateItemBomModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? MoldID { get; set; }

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

        [BindProperty]
        public DateTime DateCreation { get; set; }

        [BindProperty]
        public DateTime DateModification { get; set; } = DateTime.Now;

        public List<MoldEntity> MoldList { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            MoldList = (await _apiService
                .GetAsync<MoldEntity>("Mold"))
                .OrderBy(x => x.MoldNumber)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var user = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                var entity = new ItemBomEntity
                {
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
                    CreatedBy = user,
                    ModifiedBy = user,
                    DateCreation = DateTime.Now,
                    DateModification = DateTime.Now

                };

                
                var response = await _apiService.PostAsync("itembom", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result != null && result.CodeError != 0)
                {
                    ModelState.AddModelError(string.Empty, result.MsgError);
                    return Page();
                }

                return RedirectToPage("/ItemBom/ItemBom_List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}