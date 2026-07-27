using CAPA_ENTITY;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Reports
{
    public class ReportMoldEvaluationModel : PageModel
    {
        private readonly ApiService _apiService;

        public ReportMoldEvaluationModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty(SupportsGet = true)]
        public int? MoldID { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? MoldNumber { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool RunReport { get; set; }

        public bool ReportExecuted { get; set; }

        public string? ErrorMessage { get; set; }

        public List<MoldEvaluationEntity> ReportList { get; set; }
            = new();

        public async Task OnGetAsync()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ErrorMessage = TempData["ErrorMessage"]?.ToString();
            }

            ReportList = new List<MoldEvaluationEntity>();

            if (!RunReport)
            {
                return;
            }

            /*
             * No ejecuta el reporte cuando la página
             * se abre por primera vez.
             */
            if (!RunReport)
            {
                ReportExecuted = false;
                return;
            }

            ReportExecuted = true;

            try
            {
                if (StartDate.HasValue
                    && EndDate.HasValue
                    && StartDate.Value.Date > EndDate.Value.Date)
                {
                    ErrorMessage =
                        "La fecha inicial no puede ser mayor "
                        + "que la fecha final.";

                    ReportList = new List<MoldEvaluationEntity>();
                    return;
                }

                /*
                 * Si no existe un molde seleccionado,
                 * elimina el texto escrito para evitar
                 * mostrar un filtro que no fue aplicado.
                 */
                if (!MoldID.HasValue)
                {
                    MoldNumber = null;
                }

                ReportList =
                    await _apiService.GetReportMoldEvaluations(
                        MoldID,
                        StartDate,
                        EndDate
                    );
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    "Ocurrió un error al consultar el reporte: "
                    + ex.Message;

                ReportList = new List<MoldEvaluationEntity>();
            }
        }

        public async Task<JsonResult> OnGetSearchMoldsAsync(
            string? term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return new JsonResult(
                        new List<object>()
                    );
                }

                string endpoint =
                    $"Mold/bymoldnumber/"
                    + $"{Uri.EscapeDataString(term.Trim())}";

                var molds =
                    await _apiService.GetAsync<MoldEntity>(
                        endpoint
                    );

                var result = molds
                    .Where(m =>
                        !string.IsNullOrWhiteSpace(
                            m.MoldNumber
                        )
                    )
                    .Select(m => new
                    {
                        id = m.MoldID,
                        moldNumber = m.MoldNumber,
                        description = m.MoldDescription,

                        text = string.IsNullOrWhiteSpace(
                            m.MoldDescription
                        )
                            ? m.MoldNumber
                            : $"{m.MoldNumber} - "
                              + $"{m.MoldDescription}"
                    })
                    .Take(20)
                    .ToList();

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;

                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        public async Task<IActionResult> OnGetExportAsync(
    int? moldID,
    string? moldNumber,
    DateTime? startDate,
    DateTime? endDate)
        {
            try
            {
                if (startDate.HasValue &&
                    endDate.HasValue &&
                    startDate.Value.Date > endDate.Value.Date)
                {
                    return BadRequest(
                        "La fecha inicial no puede ser mayor que la fecha final."
                    );
                }

                var reportList =
                    await _apiService.GetReportMoldEvaluations(
                        moldID,
                        startDate,
                        endDate
                    );

                if (reportList == null || reportList.Count == 0)
                {
                    TempData["ErrorMessage"] =
                        "No se encontraron registros para exportar.";

                    return RedirectToPage(
                        "/Reports/ReportMoldEvaluation",
                        new
                        {
                            MoldID = moldID,
                            MoldNumber = moldNumber,
                            StartDate = startDate?.ToString("yyyy-MM-dd"),
                            EndDate = endDate?.ToString("yyyy-MM-dd"),
                            RunReport = true
                        }
                    );
                }

                using var workbook = new XLWorkbook();

                var worksheet =
                    workbook.Worksheets.Add("Evaluaciones de Moldes");

                var currentRow = 1;

                // Título
                worksheet.Cell(currentRow, 1).Value =
                    "Reporte de Evaluaciones de Moldes";

                worksheet.Range(currentRow, 1, currentRow, 6).Merge();

                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 1).Style.Font.FontSize = 16;
                worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                currentRow += 2;

                // Información de filtros
                worksheet.Cell(currentRow, 1).Value = "Molde:";
                worksheet.Cell(currentRow, 2).Value =
                    string.IsNullOrWhiteSpace(moldNumber)
                        ? "Todos"
                        : moldNumber;

                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "Fecha inicial:";
                worksheet.Cell(currentRow, 2).Value =
                    startDate.HasValue
                        ? startDate.Value.ToString("MM-dd-yyyy")
                        : "Todas";

                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "Fecha final:";
                worksheet.Cell(currentRow, 2).Value =
                    endDate.HasValue
                        ? endDate.Value.ToString("MM-dd-yyyy")
                        : "Todas";

                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "Fecha de exportación:";
                worksheet.Cell(currentRow, 2).Value =
                    DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");

                currentRow += 2;

                // Encabezados
                var headerRow = currentRow;

                worksheet.Cell(headerRow, 1).Value = "Número de molde";
                worksheet.Cell(headerRow, 2).Value = "Fecha de evaluación";
                worksheet.Cell(headerRow, 3).Value = "Próxima evaluación";
                worksheet.Cell(headerRow, 4).Value = "Score general";
                worksheet.Cell(headerRow, 5).Value = "Creado por";
                worksheet.Cell(headerRow, 6).Value = "Fecha de creación";

                var headerRange =
                    worksheet.Range(headerRow, 1, headerRow, 6);

                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                headerRange.Style.Fill.BackgroundColor =
                    XLColor.FromHtml("#343A40");

                headerRange.Style.Font.FontColor =
                    XLColor.White;

                currentRow++;

                // Datos
                foreach (var item in reportList)
                {

                    worksheet.Cell(currentRow, 1).Value = item.MoldNumber ?? "";

                    if (item.DateEvaluation.HasValue)
                    {
                        worksheet.Cell(currentRow, 2).Value = item.DateEvaluation.Value;

                        worksheet.Cell(currentRow, 2).Style.DateFormat.Format = "MM-dd-yyyy";
                    }

                    if (item.NextEvaluationDate.HasValue)
                    {
                        worksheet.Cell(currentRow, 3).Value = item.NextEvaluationDate.Value;

                        worksheet.Cell(currentRow, 3).Style.DateFormat.Format = "MM-dd-yyyy";
                    }

                    if (item.GeneralScore.HasValue)
                    {
                        worksheet.Cell(currentRow, 4).Value = item.GeneralScore.Value;

                        worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "0.00\"%\"";

                        if (item.GeneralScore.Value >= 90)
                        {
                            worksheet.Cell(currentRow, 4)
                                .Style.Fill.BackgroundColor =
                                XLColor.LightGreen;
                        }
                        else if (item.GeneralScore.Value >= 70)
                        {
                            worksheet.Cell(currentRow, 4)
                                .Style.Fill.BackgroundColor =
                                XLColor.LightYellow;
                        }
                        else
                        {
                            worksheet.Cell(currentRow, 4)
                                .Style.Fill.BackgroundColor =
                                XLColor.LightPink;
                        }
                    }

                    worksheet.Cell(currentRow, 5).Value = item.CreatedBy ?? "";

                    if (item.DateCreation.HasValue)
                    {
                        worksheet.Cell(currentRow, 6).Value = item.DateCreation.Value;

                        worksheet.Cell(currentRow, 6).Style.DateFormat.Format = "MM-dd-yyyy HH:mm:ss";
                    }

                    currentRow++;
                }

                var lastDataRow = currentRow - 1;

                // Bordes
                var tableRange =
                    worksheet.Range(headerRow, 1, lastDataRow, 6);

                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Filtro en encabezados
                tableRange.SetAutoFilter();

                // Congelar encabezados
                worksheet.SheetView.FreezeRows(headerRow);

                // Anchos

                worksheet.Column(1).Width = 24;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 16;
                worksheet.Column(5).Width = 25;
                worksheet.Column(6).Width = 22;

                // Alineación
                worksheet.Columns(1, 6).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                worksheet.Rows().AdjustToContents();

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var fileName =
                    $"Reporte_Evaluaciones_Moldes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    $"Error al exportar el reporte: {ex.Message}";

                return RedirectToPage(
                    "/Reports/ReportMoldEvaluation",
                    new
                    {
                        MoldID = moldID,
                        MoldNumber = moldNumber,
                        StartDate = startDate?.ToString("yyyy-MM-dd"),
                        EndDate = endDate?.ToString("yyyy-MM-dd"),
                        RunReport = true
                    }
                );
            }
        }
    }
}