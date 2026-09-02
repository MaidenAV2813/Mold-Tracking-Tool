using System.Text.Json;
using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;


namespace Tracking_Tool_System.Pages.ListNumber
{
    public class CreateListNumberModel : PageModel
    {
        private readonly ApiService _apiService;

        public CreateListNumberModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public List<string> SelectedListNumbers { get; set; } = new();

        /*
         * Contiene en formato JSON los números de parte seleccionados
         * junto con sus descripciones.
         */
        [BindProperty]
        public string? SelectedListNumbersData { get; set; }

        [BindProperty]
        public int? MoldID { get; set; }

        public List<MoldEntity> Mold { get; set; } = new();

        public List<vw_EBS_List_NumbersEntity> ListNumbers { get; set; } = new();

        private async Task LoadMolds()
        {
            var result = await _apiService.GetAsync<MoldEntity>("mold");

            Mold = result?.ToList() ?? new List<MoldEntity>();
        }

        /*
         * Consulta Oracle EBS por medio del nuevo procedimiento almacenado.
         * El término enviado corresponde al SEGMENT1 o número de parte.
         */
        public async Task<JsonResult> OnGetSearchListNumbers(string? term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return new JsonResult(new
                    {
                        results = new List<object>()
                    });
                }

                string partNumber = term.Trim();

                var result =
                    await _apiService.GetAsync<vw_EBS_List_NumbersEntity>(
                        $"vw_EBS_List_Numbers/bylistnumber/" +
                        $"{Uri.EscapeDataString(partNumber)}"
                    );

                var data = result
                    .Where(x => !string.IsNullOrWhiteSpace(x.SEGMENT1))
                    .Select(x => new
                    {
                        id = x.SEGMENT1,
                        text = x.SEGMENT1,
                        description = x.DESCRIPTION ?? string.Empty,
                        itemType = x.ITEM_TYPE ?? string.Empty
                    })
                    .ToList();

                return new JsonResult(new
                {
                    results = data
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    results = new List<object>(),
                    error = ex.Message
                });
            }
        }

        public async Task<JsonResult> OnGetSearchMolds(string? term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return new JsonResult(new
                    {
                        results = new List<object>()
                    });
                }

                var result =
                    await _apiService.GetAsync<MoldEntity>(
                        $"Mold/bymoldnumber/" +
                        $"{Uri.EscapeDataString(term.Trim())}"
                    );

                var data = result.Select(x => new
                {
                    id = x.MoldID,
                    text = x.MoldNumber
                }).ToList();

                return new JsonResult(new
                {
                    results = data
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    results = new List<object>(),
                    error = ex.Message
                });
            }
        }

        public async Task<IActionResult> OnGet()
        {
            await LoadMolds();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (MoldID == null)
            {
                ModelState.AddModelError(
                    nameof(MoldID),
                    "Debe seleccionar un número de molde."
                );
            }

            List<SelectedPartModel> selectedParts = new();

            if (!string.IsNullOrWhiteSpace(SelectedListNumbersData))
            {
                try
                {
                    selectedParts =
                        JsonSerializer.Deserialize<List<SelectedPartModel>>(
                            SelectedListNumbersData,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }
                        ) ?? new List<SelectedPartModel>();
                }
                catch (JsonException)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "No fue posible procesar la información de los números de parte seleccionados."
                    );
                }
            }

            if (!selectedParts.Any())
            {
                ModelState.AddModelError(
                    nameof(SelectedListNumbersData),
                    "Debe seleccionar al menos un número de parte."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadMolds();
                return Page();
            }

            string user = User.Identity?.Name ?? "System";
            DateTime now = DateTime.Now;

            var partsToSave = selectedParts
                .Where(x => !string.IsNullOrWhiteSpace(x.ListNumber))
                .GroupBy(
                    x => x.ListNumber!.Trim(),
                    StringComparer.OrdinalIgnoreCase
                )
                .Select(x => x.First())
                .ToList();

            foreach (var part in partsToSave)
            {
                var entity = new ListNumberEntity
                {
                    MoldID = MoldID,
                    ListNumber = part.ListNumber!.Trim(),
                    Description = part.Description?.Trim() ?? string.Empty,
                    CreatedBy = user,
                    ModifiedBy = user,
                    DateCreation = DateTime.Now,
                    DateModification = DateTime.Now
                };

                var response = await _apiService.PostAsync(
                    "listnumber",
                    entity
                );

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent =
                        await response.Content.ReadAsStringAsync();

                    string mensaje =
                        $"No fue posible guardar el número de parte {entity.ListNumber}.";

                    try
                    {
                        var dbError = JsonSerializer.Deserialize<DBEntity>(
                            errorContent,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        if (!string.IsNullOrWhiteSpace(dbError?.MsgError))
                        {
                            mensaje = dbError.MsgError;
                        }
                    }
                    catch (JsonException)
                    {
                        // Si no es un JSON válido, se deja el mensaje general.
                    }

                    ModelState.AddModelError(
                        string.Empty,
                        mensaje
                    );

                    // En Create se vuelven a cargar los moldes
                    await LoadMolds();

                    return Page();
                }
            }

            TempData["SuccessMessage"] =
                $"{partsToSave.Count} números de parte fueron agregados correctamente.";

            return RedirectToPage(
                "/ListNumber/ListNumber_List"
            );
        }

        /*
         * Modelo utilizado únicamente para recibir el JSON de la página.
         */
        public class SelectedPartModel
        {
            public string? ListNumber { get; set; }

            public string? Description { get; set; }

            public string? ItemType { get; set; }
        }
    }
}
