using System.Data;
using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Mold
{
    public class EditMoldModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditMoldModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int MoldID { get; set; }

        [BindProperty]
        public int? CriticallyID { get; set; }

        [BindProperty]
        public string? CriticallyType { get; set; }

        [BindProperty]
        public int? GateID { get; set; }

        [BindProperty]
        public string? GateType { get; set; }

        [BindProperty]
        public int? CastingID { get; set; }

        [BindProperty]
        public string? CastingType { get; set; }

        [BindProperty]
        public int? ActuatorID { get; set; }

        [BindProperty]
        public string? ActuatorType { get; set; }

        public List<MoldEntity> Molds { get; set; } = new();

        public List<CriticallyMoldEntity> Critically { get; set; } = new();

        public List<GateTypeEntity> Gate { get; set; } = new();

        public List<CastingMoldEntity> Casting { get; set; } = new();

        public List<ActuatorTypeEntity> Actuator { get; set; } = new();

        [BindProperty]
        public String? MoldAssetNumber { get; set; }

        [BindProperty]
        public string? MoldNumber { get; set; }

        [BindProperty]
        public string? MoldDescription { get; set; }

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
        public string? HaveCounter { get; set; }

        [BindProperty]
        public string? CounterType { get; set; }

        [BindProperty]
        public string? ThreeLayer { get; set; }

        [BindProperty]
        public int? InitialCount { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {

            MoldID = id;

            // Cargar listas para los combos
            Critically = await _apiService.GetAsync<CriticallyMoldEntity>("critically");
            Gate = await _apiService.GetAsync<GateTypeEntity>("gates");
            Casting = await _apiService.GetAsync<CastingMoldEntity>("casting");
            Actuator = await _apiService.GetAsync<ActuatorTypeEntity>("actuator");

            //Obtener el molde a editar
            var mold = (await _apiService.GetAsync<MoldEntity>("mold"))
                .FirstOrDefault(x => x.MoldID == id);

            if (mold == null)
                return NotFound();

            // Asignar valores a los controles
            MoldNumber = mold.MoldNumber;
            MoldDescription = mold.MoldDescription;
            MoldAssetNumber = mold.MoldAssetNumber;
            MoldStatus = mold.MoldStatus;
            MoldOrigin = mold.MoldOrigin;
            DigitalPlane = mold.DigitalPlane;

            CriticallyID = mold.CriticallyID;
            GateID = mold.GateID;
            CastingID = mold.CastingID;
            ActuatorID = mold.ActuatorID;

            CavityQty = mold.CavityQty;
            BlockCavityQty = mold.BlockCavityQty;
            HaveCounter = mold.HaveCounter;
            CounterType = mold.CounterType;
            ThreeLayer = mold.ThreeLayer;
            InitialCount = mold.InitialCount;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            
            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var moldList = await _apiService.GetAsync<MoldEntity>("mold");

            if (HaveCounter == "No")
            {
                CounterType = "N/A";
            }

            var entity = new MoldEntity
            {
                MoldID = MoldID,
                CriticallyID = CriticallyID,
                GateID = GateID,
                CastingID = CastingID,
                ActuatorID  = ActuatorID,
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
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("mold", entity);

            if (!response.IsSuccessStatusCode)
            {

                Molds = await _apiService.GetAsync<MoldEntity>("mold");
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
                return Page();
            }

            return RedirectToPage("/Mold/Mold_List");
        }
    }
}