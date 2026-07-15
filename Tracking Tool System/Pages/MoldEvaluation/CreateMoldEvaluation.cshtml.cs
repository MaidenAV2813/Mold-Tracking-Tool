using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.MoldEvaluation
{
    public class CreateMoldEvaluationModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateMoldEvaluationModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? MoldID { get; set; }

        [BindProperty]
        public DateTime? DateEvaluation { get; set; }

        [BindProperty]
        public DateTime? NextEvaluationDate { get; set; }

        [BindProperty]
        public decimal? GeneralScore { get; set; }

        [BindProperty]
        public List<MoldPartEvaluationEntity> EvaluationParts { get; set; }
            = new();

        public List<MoldEntity> MoldList { get; set; } = new();

        public List<int> ScoreOptions { get; set; } = new()
        {
            0,
            25,
            50,
            75,
            80,
            90,
            100
        };

        public async Task<IActionResult> OnGet()
        {
            DateEvaluation = DateTime.Today;
            NextEvaluationDate = DateEvaluation.Value.AddYears(1);

            await LoadData();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            // Recalcula la próxima evaluación en el servidor.
            NextEvaluationDate = DateEvaluation?.AddYears(1);

            // Calcula el promedio de las partes evaluadas.
            CalculateGeneralScore();

            ValidateEvaluation();

            if (!ModelState.IsValid)
            {
                await LoadMolds();
                return Page();
            }

            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;

            var entity = new MoldEvaluationEntity
            {
                MoldID = MoldID,
                DateEvaluation = DateEvaluation,

                // Se envía para mantener la entidad completa,
                // aunque SQL vuelve a calcularla.
                NextEvaluationDate = NextEvaluationDate,

                GeneralScore = GeneralScore,

                EvaluationParts = EvaluationParts,

                DateCreation = now,
                DateModification = now,
                CreatedBy = user,
                ModifiedBy = user
            };

            var response = await _apiService.PostAsync(
                "MoldEvaluation",
                entity);

            var result = await response.Content
                .ReadFromJsonAsync<DBEntity>();

            if (result == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "La API no devolvió una respuesta válida.");

                await LoadMolds();
                return Page();
            }

            if (result.CodeError != 0)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.MsgError ??
                    "No fue posible guardar la evaluación.");

                await LoadMolds();
                return Page();
            }

            TempData["SuccessMessage"] =
                result.MsgError ??
                "Evaluación registrada correctamente.";

            return RedirectToPage(
                "/MoldEvaluation/MoldEvaluation_List");
        }

        private void ValidateEvaluation()
        {
            if (MoldID == null)
            {
                ModelState.AddModelError(
                    nameof(MoldID),
                    "Debe seleccionar un molde.");
            }

            if (DateEvaluation == null)
            {
                ModelState.AddModelError(
                    nameof(DateEvaluation),
                    "Debe seleccionar la fecha de evaluación.");
            }

            if (EvaluationParts == null ||
                EvaluationParts.Count == 0)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No existen partes configuradas para la evaluación.");
            }

            if (EvaluationParts != null &&
                EvaluationParts.Any(x => x.MoldEvaPartID == null))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Una o más partes de la evaluación no son válidas.");
            }

            if (EvaluationParts != null &&
                EvaluationParts.Any(x =>
                    x.Score != null &&
                    x.Score != 0 &&
                    x.Score != 25 &&
                    x.Score != 50 &&
                    x.Score != 75 &&
                    x.Score != 80 &&
                    x.Score != 90 &&
                    x.Score != 100))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Una o más calificaciones no son válidas.");
            }

            if (EvaluationParts != null &&
                !EvaluationParts.Any(x => x.Score.HasValue))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Debe evaluar al menos una parte del molde.");
            }
        }

        private void CalculateGeneralScore()
        {
            if (EvaluationParts == null)
            {
                GeneralScore = null;
                return;
            }

            var validScores = EvaluationParts
                .Where(x => x.Score.HasValue)
                .Select(x => Convert.ToDecimal(x.Score!.Value))
                .ToList();

            if (!validScores.Any())
            {
                GeneralScore = null;
                return;
            }

            GeneralScore = Math.Round(
                validScores.Average(),
                2,
                MidpointRounding.AwayFromZero);
        }

        private async Task LoadData()
        {
            await LoadMolds();
            await LoadEvaluationParts();
        }

        private async Task LoadMolds()
        {
            MoldList = await _apiService
                .GetAsync<MoldEntity>("Mold");
        }

        private async Task LoadEvaluationParts()
        {
            var parts = await _apiService
                .GetAsync<MoldEvaluationPartEntity>(
                    "MoldEvaluationPart");

            EvaluationParts = parts
                .OrderBy(x => x.Parts)
                .Select(x => new MoldPartEvaluationEntity
                {
                    MoldEvaPartID = x.MoldEvaPartID,
                    Parts = x.Parts,
                    Score = null,
                    Observation = null
                })
                .ToList();
        }
    }
}
