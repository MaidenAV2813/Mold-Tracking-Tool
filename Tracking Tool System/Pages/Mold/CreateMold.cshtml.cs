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
        public int? CategorizationID { get; set; }

        [BindProperty]
        public string? MoldNumber { get; set; }

        [BindProperty]
        public string? MoldDescription { get; set; }

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
        public string? Percentage_Spares_available { get; set; }

        [BindProperty]
        public string? Last_Reparir_12_Months { get; set; }

        [BindProperty]
        public string? Comment_Last_Reparir_12_Months { get; set; }

        [BindProperty]
        public string? Quality_Issue { get; set; }

        [BindProperty]
        public string? Comment_Quality_Issue { get; set; }

        [BindProperty]
        public DateTime DateCreation { get; set; }

        [BindProperty]
        public DateTime DateModification { get; set; } = DateTime.Now;


        public List<CriticallyMoldEntity> CriticallyList { get; set; } = new();

        public List<GateTypeEntity> GateList { get; set; } = new();

        public List<CastingMoldEntity> CastingList { get; set; } = new();

        public List<ActuatorTypeEntity> ActuatorList { get; set; } = new();

        public List<CategorizationMoldEntity> CategorizationList { get; set; } = new();

        public async Task OnGet()
        {
            CriticallyList = (await _apiService
                .GetAsync<CriticallyMoldEntity>("Critically"))
                .Where(x => x.CriticallyStatus == true)
                .OrderBy(x => x.CriticallyType)
                .ToList();

            GateList = (await _apiService
                .GetAsync<GateTypeEntity>("gates"))
                .Where(x => x.GateStatus == true)
                .OrderBy(x => x.GateType)
                .ToList();

            CastingList = (await _apiService
                .GetAsync<CastingMoldEntity>("Casting"))
                .Where(x => x.CastingStatus == true)
                .OrderBy(x => x.CastingType)
                .ToList();

            ActuatorList = (await _apiService
                .GetAsync<ActuatorTypeEntity>("Actuator"))
                .Where(x => x.ActuatorStatus == true)
                .OrderBy(x => x.ActuatorType)
                .ToList();

            CategorizationList = (await _apiService
                .GetAsync<CategorizationMoldEntity>("Categorization"))
                //.Where(x => x.CategorizationStatus == true)
                .OrderBy(x => x.CategorizationType)
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
                    CategorizationID = CategorizationID,
                    MoldAssetNumber = MoldAssetNumber,
                    MoldNumber = MoldNumber,
                    MoldDescription = MoldDescription,
                    MoldStatus = MoldStatus,
                    MoldOrigin = MoldOrigin,
                    DigitalPlane = DigitalPlane,
                    CavityQty = CavityQty,
                    BlockCavityQty = BlockCavityQty,
                    HaveCounter = HaveCounter,
                    CounterType = CounterType,
                    ThreeLayer = ThreeLayer,
                    InitialCount = InitialCount,
                    Percentage_Spares_available = Percentage_Spares_available,
                    Last_Reparir_12_Months = Last_Reparir_12_Months,
                    Comment_Last_Reparir_12_Months = Comment_Last_Reparir_12_Months,
                    Quality_Issue = Quality_Issue,
                    Comment_Quality_Issue = Comment_Quality_Issue,
                    DateCreation = DateTime.Now,
                    DateModification = DateTime.Now,
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
