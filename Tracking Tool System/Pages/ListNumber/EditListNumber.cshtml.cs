using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;
using System.Text.Json;

namespace Tracking_Tool_System.Pages.ListNumber
{
    public class EditListNumberModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditListNumberModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        /*
         * MoldID recibido desde la página ListNumber_List.
         */
        [BindProperty(SupportsGet = true)]
        public int? MoldID { get; set; }

        /*
         * Número de molde que se muestra en la pantalla.
         */
        public string? MoldNumber { get; set; }

        /*
         * Registros asociados al molde.
         * Esta lista también recibe los cambios del formulario.
         */
        [BindProperty]
        public List<ListNumberEntity> ListNumbers { get; set; } = new();

        /*
         * Consulta todos los ListNumbers asociados al molde.
         */
        private async Task LoadListNumbers(int moldId)
        {
            var result =
                await _apiService.GetAsync<ListNumberEntity>(
                    $"listnumber/bymold/{moldId}"
                );

            ListNumbers = result?.ToList()
                ?? new List<ListNumberEntity>();

            MoldNumber = ListNumbers
                .FirstOrDefault()?
                .MoldNumber;
        }

        /*
         * Carga únicamente el número de molde.
         * Se utiliza cuando hay un error en el POST y se debe
         * regresar a la página sin perder los cambios del usuario.
         */
        private async Task LoadMoldNumber(int moldId)
        {
            var molds =
                await _apiService.GetAsync<MoldEntity>("mold");

            MoldNumber = molds?
                .FirstOrDefault(x => x.MoldID == moldId)?
                .MoldNumber;
        }

        /*
         * Búsqueda de números de parte en Oracle EBS.
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
                    await _apiService
                        .GetAsync<vw_EBS_List_NumbersEntity>(
                            "vw_EBS_List_Numbers/bylistnumber/" +
                            Uri.EscapeDataString(partNumber)
                        );

                var data = result
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x.SEGMENT1))
                    .Select(x => new
                    {
                        id = x.SEGMENT1,
                        text = x.SEGMENT1,
                        description =
                            x.DESCRIPTION ?? string.Empty,
                        itemType =
                            x.ITEM_TYPE ?? string.Empty
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

        public async Task<IActionResult> OnGetAsync(int? moldId)
        {
            if (!moldId.HasValue)
            {
                return RedirectToPage(
                    "/ListNumber/ListNumber_List"
                );
            }

            MoldID = moldId.Value;

            try
            {
                await LoadListNumbers(moldId.Value);

                if (!ListNumbers.Any())
                {
                    TempData["ErrorMessage"] =
                        "El molde seleccionado no tiene números de parte asociados.";

                    return RedirectToPage(
                        "/ListNumber/ListNumber_List",
                        new
                        {
                            SelectedMoldID = moldId.Value
                        }
                    );
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    $"No fue posible cargar los ListNumbers. {ex.Message}";

                return RedirectToPage(
                    "/ListNumber/ListNumber_List"
                );
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!MoldID.HasValue)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No se recibió el molde que se desea modificar."
                );
            }

            if (ListNumbers == null || !ListNumbers.Any())
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No existen números de parte para actualizar."
                );
            }

            /*
             * Validación individual de los registros.
             */
            if (ListNumbers != null)
            {
                for (int i = 0; i < ListNumbers.Count; i++)
                {
                    var item = ListNumbers[i];

                    if (!item.ListNumberID.HasValue)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            $"El registro de la fila {i + 1} no tiene identificador."
                        );
                    }

                    if (string.IsNullOrWhiteSpace(item.ListNumber))
                    {
                        ModelState.AddModelError(
                            $"ListNumbers[{i}].ListNumber",
                            "Debe seleccionar un número de parte."
                        );
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                if (MoldID.HasValue)
                {
                    await LoadMoldNumber(MoldID.Value);
                }

                return Page();
            }

            string user =
                User.Identity?.Name ?? "System";

            DateTime now = DateTime.Now;

            foreach (var item in ListNumbers)
            {
                item.MoldID = MoldID;
                item.ListNumber = item.ListNumber?.Trim();
                item.Description = item.Description?.Trim();
                item.ModifiedBy = user;
                item.DateModification = now;

                var response =
                    await _apiService.PutAsync(
                        "listnumber",
                        item
                    );

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent =
                        await response.Content.ReadAsStringAsync();

                    string mensaje =
                        $"No fue posible actualizar el número de parte {item.ListNumber}.";

                    try
                    {
                        var dbError = JsonSerializer.Deserialize<DBEntity>(
                            errorContent,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }
                        );

                        if (!string.IsNullOrWhiteSpace(dbError?.MsgError))
                        {
                            mensaje = dbError.MsgError;
                        }
                    }
                    catch (JsonException)
                    {
                        // Si la respuesta no es un JSON válido,
                        // se conserva el mensaje general.
                    }

                    ModelState.AddModelError(
                        string.Empty,
                        mensaje
                    );

                    await LoadMoldNumber(MoldID!.Value);

                    return Page();
                }
            }

            TempData["SuccessMessage"] =
                $"{ListNumbers.Count} números de parte fueron actualizados correctamente.";

            return RedirectToPage(
                "/ListNumber/ListNumber_List",
                new
                {
                    SelectedMoldID = MoldID
                }
            );
        }
    }
}