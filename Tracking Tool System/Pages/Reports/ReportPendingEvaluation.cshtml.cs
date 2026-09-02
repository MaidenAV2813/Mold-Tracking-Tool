using CAPA_ENTITY;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Reports
{
    public class ReportPendingEvaluationModel : PageModel
    {
        private readonly ApiService _apiService;

        public ReportPendingEvaluationModel(
            ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        public List<ReportPendingEvaluationEntity>
            ReportList
        { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public int SelectedYear =>
            Year ?? DateTime.Now.Year;

        public int TotalPending =>
            ReportList.Sum(
                item => item.PendingQuantity
            );

        public async Task OnGetAsync()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ErrorMessage =
                    TempData["ErrorMessage"]?.ToString();
            }

            try
            {
                Year ??= DateTime.Now.Year;

                ReportList =
                    await _apiService.GetPendingEvaluationsReport(Year);
            }
            catch (Exception ex)
            {
                ReportList =
                    new List<ReportPendingEvaluationEntity>();

                ErrorMessage =
                    "Ocurrió un error al cargar el reporte: "
                    + ex.Message;
            }
        }

        public async Task<IActionResult>
    OnGetExportAsync(int? year)
        {
            try
            {
                year ??= DateTime.Now.Year;

                var report =
        await _apiService
        .GetPendingEvaluationsReport(year, true);

                if (report == null || report.Count == 0)
                {
                    TempData["ErrorMessage"] =
                        "No existen moldes para exportar.";

                    return RedirectToPage(
                        "/Reports/ReportPendingEvaluation",
                        new
                        {
                            Year = year
                        }
                    );
                }

                using var workbook = new XLWorkbook();

                var worksheet =
                    workbook.Worksheets.Add(
                        "Evaluaciones pendientes"
                    );

                worksheet.Cell(1, 1).Value =
                    "Detalle de evaluaciones pendientes";

                worksheet.Range(1, 1, 1, 6).Merge();

                worksheet.Cell(1, 1)
                    .Style.Font.Bold = true;

                worksheet.Cell(1, 1)
                    .Style.Font.FontSize = 16;

                worksheet.Cell(2, 1).Value =
                    "Año del reporte:";

                worksheet.Cell(2, 2).Value =
                    year.Value;

                worksheet.Cell(3, 1).Value =
                    "Fecha de exportación:";

                worksheet.Cell(3, 2).Value =
                    DateTime.Now;

                worksheet.Cell(3, 2)
                    .Style.DateFormat.Format =
                    "MM-dd-yyyy HH:mm:ss";

                const int headerRow = 5;

                worksheet.Cell(headerRow, 1).Value =
                    "Mes";

                worksheet.Cell(headerRow, 2).Value =
                    "Número de molde";

                worksheet.Cell(headerRow, 3).Value =
                    "Descripción";

                worksheet.Cell(headerRow, 4).Value =
                    "Fecha de última evaluación";

                worksheet.Cell(headerRow, 5).Value =
                    "Fecha de próxima evaluación";

                worksheet.Cell(headerRow, 6).Value =
                    "Año";

                var headerRange =
                    worksheet.Range(
                        headerRow,
                        1,
                        headerRow,
                        6
                    );

                headerRange.Style.Font.Bold = true;

                headerRange.Style
                    .Fill.BackgroundColor =
                    XLColor.FromHtml("#343A40");

                headerRange.Style
                    .Font.FontColor =
                    XLColor.White;

                int currentRow = headerRow + 1;

                foreach (var item in report
                    .OrderBy(x => x.MonthNumber)
                    .ThenBy(x => x.NextEvaluationDate)
                    .ThenBy(x => x.MoldNumber))
                {
                    worksheet.Cell(currentRow, 1).Value =
                        item.MonthName ?? string.Empty;

                    worksheet.Cell(currentRow, 2).Value =
                        item.MoldNumber ?? string.Empty;

                    worksheet.Cell(currentRow, 3).Value =
                        item.MoldDescription ?? string.Empty;

                    if (item.DateEvaluation.HasValue)
                    {
                        worksheet.Cell(currentRow, 4).Value =
                            item.DateEvaluation.Value;

                        worksheet.Cell(currentRow, 4)
                            .Style.DateFormat.Format =
                            "MM-dd-yyyy";
                    }

                    if (item.NextEvaluationDate.HasValue)
                    {
                        worksheet.Cell(currentRow, 5).Value =
                            item.NextEvaluationDate.Value;

                        worksheet.Cell(currentRow, 5)
                            .Style.DateFormat.Format =
                            "MM-dd-yyyy";
                    }

                    worksheet.Cell(currentRow, 6).Value =
                        item.ReportYear;

                    currentRow++;
                }

                var tableRange =
                    worksheet.Range(
                        headerRow,
                        1,
                        currentRow - 1,
                        6
                    );

                tableRange.Style.Border
                    .OutsideBorder =
                    XLBorderStyleValues.Thin;

                tableRange.Style.Border
                    .InsideBorder =
                    XLBorderStyleValues.Thin;

                tableRange.SetAutoFilter();

                worksheet.SheetView
                    .FreezeRows(headerRow);

                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 40;
                worksheet.Column(4).Width = 24;
                worksheet.Column(5).Width = 25;
                worksheet.Column(6).Width = 12;

                using var stream =
                    new MemoryStream();

                workbook.SaveAs(stream);

                string fileName =
                    $"Evaluaciones_Pendientes_{year.Value}_"
                    + $"{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-"
                    + "officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    "Error al exportar: "
                    + ex.Message;

                return RedirectToPage(
                    "/Reports/ReportPendingEvaluation",
                    new
                    {
                        Year =
                            year
                            ?? DateTime.Now.Year
                    }
                );
            }
        }
    }
}