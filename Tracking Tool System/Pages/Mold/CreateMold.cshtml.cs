using System.Data;
using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Mold
{
    public class CreateMoldModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateMoldModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? CriticallyID { get; set; }

        [BindProperty]
        public int? GateID { get; set; }

        [BindProperty]
        public int? CastingID { get; set; }

        [BindProperty]
        public int? ActuatorID { get; set; }

        [BindProperty]
        public string? MoldNumber { get; set; }

        [BindProperty]
        public string? MoldAssetNumber { get; set; }

        [BindProperty]
        public string? MoldStatus { get; set; }

        [BindProperty]
        public string? MoldOrigin { get; set; }

        [BindProperty]
        public string? DigitalPlane { get; set; }

        [BindProperty]
        public int? CavityQty { get; set; }

        [BindProperty]
        public int? BlockCavityQty { get; set; }

        [BindProperty]
        public int? InitialCount { get; set; }

        [BindProperty]
        public string? HaveCounter { get; set; }

        [BindProperty]
        public string? CounterType { get; set; }

        [BindProperty]
        public string? ThreeLayer { get; set; }
        [BindProperty]
        public DateTime DateCreation { get; set; }

        [BindProperty]
        public DateTime DateModification { get; set; } = DateTime.Now;


        public List<CriticallyMoldEntity> CriticallyList { get; set; } = new();

        public List<GateTypeEntity> GateList { get; set; } = new();

        public List<CastingMoldEntity> CastingList { get; set; } = new();

        public List<ActuatorTypeEntity> ActuatorList { get; set; } = new();

        public async Task OnGet()
        {
            CriticallyList = (await _apiService
                .GetAsync<CriticallyMoldEntity>("Critically"))
                .OrderBy(x => x.CriticallyType)
                .ToList();

            GateList = (await _apiService
                .GetAsync<GateTypeEntity>("Gates"))
                .OrderBy(x => x.GateType)
                .ToList();

            CastingList = (await _apiService
                .GetAsync<CastingMoldEntity>("Casting"))
                .OrderBy(x => x.CastingType)
                .ToList();

            ActuatorList = (await _apiService
                .GetAsync<ActuatorTypeEntity>("Actuator"))
                .OrderBy(x => x.ActuatorType)
                .ToList();
        }


        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                if (HaveCounter == "No")
                {
                    CounterType = "N/A";
                }
                var user = User.Identity?.Name ?? "System";
                var now = DateTime.Now;

                var entity = new MoldEntity
                {

                    CriticallyID = CriticallyID,
                    GateID = GateID,
                    CastingID = CastingID,
                    ActuatorID = ActuatorID,
                    MoldAssetNumber = MoldAssetNumber,
                    MoldNumber = MoldNumber,
                    MoldStatus = MoldStatus,
                    MoldOrigin = MoldOrigin,
                    DigitalPlane = DigitalPlane,
                    CavityQty = CavityQty,
                    BlockCavityQty = BlockCavityQty,
                    HaveCounter = HaveCounter,
                    CounterType = CounterType,
                    ThreeLayer = ThreeLayer,
                    InitialCount = InitialCount,
                    DateCreation = now,
                    DateModification = now,
                    CreatedBy = user,
                    ModifiedBy = user

                };

                var response = await _apiService.PostAsync("mold", entity);

                var result = await response.Content.ReadFromJsonAsync<DBEntity>();

                if (result != null && result.CodeError != 0)
                {
                    ModelState.AddModelError(string.Empty, result.MsgError);
                    return Page();
                }

                return RedirectToPage("/Mold/CreateMold");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}
