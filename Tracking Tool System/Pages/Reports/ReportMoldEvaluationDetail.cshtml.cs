using CAPA_ENTITY;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Reports
{
    public class ReportMoldEvaluationDetailModel : PageModel
    {
        private readonly ApiService _apiService;

        public ReportMoldEvaluationDetailModel(
            ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty(SupportsGet = true)]
        public int? EvaluationID { get; set; }

        public MoldEvaluationEntity? Evaluation { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (!EvaluationID.HasValue ||
                    EvaluationID.Value <= 0)
                {
                    ErrorMessage =
                        "No se recibió un identificador de evaluación válido.";

                    return Page();
                }

                Evaluation =
                    await _apiService
                        .GetReportMoldEvaluationDetail(
                            EvaluationID.Value
                        );

                if (Evaluation == null)
                {
                    ErrorMessage =
                        "No se encontró la evaluación seleccionada.";

                    return Page();
                }

                Evaluation.EvaluationParts ??=
                    new List<MoldPartEvaluationEntity>();

                return Page();
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage =
                    "No fue posible consultar el detalle de la evaluación. "
                    + ex.Message;

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    "Ocurrió un error al cargar el detalle de la evaluación. "
                    + ex.Message;

                return Page();
            }
        }

        public async Task<IActionResult> OnGetExportAsync(
    int evaluationID)
        {
            try
            {
                if (evaluationID <= 0)
                {
                    return RedirectToPage(
                        "/Reports/ReportMoldEvaluation"
                    );
                }

                var evaluation =
                    await _apiService
                        .GetReportMoldEvaluationDetail(
                            evaluationID
                        );

                if (evaluation == null)
                {
                    TempData["ErrorMessage"] =
                        "No se encontró la evaluación seleccionada.";

                    return RedirectToPage(
                        "/Reports/ReportMoldEvaluation"
                    );
                }

                evaluation.EvaluationParts ??=
                    new List<MoldPartEvaluationEntity>();

                using var workbook = new XLWorkbook();

                var worksheet =
                    workbook.Worksheets.Add(
                        "Detalle Evaluación"
                    );

                // Título
                worksheet.Cell("A1").Value =
                    "DETALLE DE EVALUACIÓN DEL MOLDE";

                worksheet.Range("A1:D1").Merge();

                worksheet.Cell("A1").Style.Font.Bold = true;
                worksheet.Cell("A1").Style.Font.FontSize = 16;
                worksheet.Cell("A1").Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                // Encabezado general
                worksheet.Cell("A3").Value =
                    "Número de molde";

                worksheet.Cell("B3").Value =
                    evaluation.MoldNumber ?? string.Empty;

                worksheet.Cell("A4").Value =
                    "Descripción";

                worksheet.Cell("B4").Value =
                    evaluation.MoldDescription ?? string.Empty;

                worksheet.Cell("A5").Value =
                    "Fecha de evaluación";

                if (evaluation.DateEvaluation.HasValue)
                {
                    worksheet.Cell("B5").Value =
                        evaluation.DateEvaluation.Value;

                    worksheet.Cell("B5").Style.DateFormat.Format =
                        "MM-dd-yyyy";
                }

                worksheet.Cell("A6").Value =
                    "Próxima evaluación";

                if (evaluation.NextEvaluationDate.HasValue)
                {
                    worksheet.Cell("B6").Value =
                        evaluation.NextEvaluationDate.Value;

                    worksheet.Cell("B6").Style.DateFormat.Format =
                        "MM-dd-yyyy";
                }

                worksheet.Cell("A7").Value =
                    "Calificación general";

                if (evaluation.GeneralScore.HasValue)
                {
                    worksheet.Cell("B7").Value =
                        evaluation.GeneralScore.Value / 100;

                    worksheet.Cell("B7").Style.NumberFormat.Format =
                        "0.00%";
                }
                else
                {
                    worksheet.Cell("B7").Value =
                        "Sin calificación";
                }

                worksheet.Range("A3:A7")
                    .Style.Font.Bold = true;

                worksheet.Range("A3:B7")
                    .Style.Border.OutsideBorder =
                        XLBorderStyleValues.Thin;

                worksheet.Range("A3:B7")
                    .Style.Border.InsideBorder =
                        XLBorderStyleValues.Thin;

                // Tabla de partes
                int headerRow = 9;

                worksheet.Cell(headerRow, 1).Value =
                    "#";

                worksheet.Cell(headerRow, 2).Value =
                    "Parte";

                worksheet.Cell(headerRow, 3).Value =
                    "Calificación";

                worksheet.Cell(headerRow, 4).Value =
                    "Observación";

                var headerRange =
                    worksheet.Range(
                        headerRow,
                        1,
                        headerRow,
                        4
                    );

                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                headerRange.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;

                headerRange.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;

                int currentRow = headerRow + 1;
                int itemNumber = 1;

                foreach (var part in evaluation.EvaluationParts)
                {
                    worksheet.Cell(currentRow, 1).Value =
                        itemNumber;

                    worksheet.Cell(currentRow, 2).Value =
                        part.Parts ?? string.Empty;

                    if (part.Score.HasValue)
                    {
                        worksheet.Cell(currentRow, 3).Value =
                            part.Score.Value / 100m;

                        worksheet.Cell(currentRow, 3)
                            .Style.NumberFormat.Format =
                                "0.00%";
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 3).Value =
                            "Sin calificación";
                    }

                    worksheet.Cell(currentRow, 4).Value =
                        string.IsNullOrWhiteSpace(
                            part.Observation
                        )
                            ? "Sin observaciones"
                            : part.Observation;

                    currentRow++;
                    itemNumber++;
                }

                if (evaluation.EvaluationParts.Count == 0)
                {
                    worksheet.Cell(currentRow, 1).Value =
                        "No existen partes registradas.";

                    worksheet.Range(
                        currentRow,
                        1,
                        currentRow,
                        4
                    ).Merge();

                    worksheet.Cell(currentRow, 1)
                        .Style.Alignment.Horizontal =
                            XLAlignmentHorizontalValues.Center;

                    currentRow++;
                }

                var detailRange =
                    worksheet.Range(
                        headerRow,
                        1,
                        currentRow - 1,
                        4
                    );

                detailRange.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;

                detailRange.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;

                worksheet.Column(1).Width = 8;
                worksheet.Column(2).Width = 35;
                worksheet.Column(3).Width = 18;
                worksheet.Column(4).Width = 60;

                worksheet.Column(4)
                    .Style.Alignment.WrapText = true;

                worksheet.SheetView.FreezeRows(headerRow);

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                string moldNumber =
                    string.IsNullOrWhiteSpace(
                        evaluation.MoldNumber
                    )
                        ? "Molde"
                        : evaluation.MoldNumber;

                foreach (char invalidCharacter
                         in Path.GetInvalidFileNameChars())
                {
                    moldNumber =
                        moldNumber.Replace(
                            invalidCharacter,
                            '_'
                        );
                }

                string fileName =
                    $"Detalle_Evaluacion_{moldNumber}_" +
                    $"{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-" +
                    "officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    "Ocurrió un error al exportar el detalle. "
                    + ex.Message;

                EvaluationID = evaluationID;

                Evaluation =
                    await _apiService
                        .GetReportMoldEvaluationDetail(
                            evaluationID
                        );

                return Page();
            }
        }
    }
}