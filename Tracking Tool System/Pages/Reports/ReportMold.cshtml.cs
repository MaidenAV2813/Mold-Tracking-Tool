using CAPA_ENTITY;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tracking_Tool_System.Pages.Reports
{
    public class ReportMoldModel : PageModel
    {
        private readonly ApiService _apiService;

        public ReportMoldModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        // ID del molde seleccionado en el buscador.
        [BindProperty(SupportsGet = true)]
        public int? MoldID { get; set; }

        // Número visible en el campo de búsqueda.
        [BindProperty(SupportsGet = true)]
        public string? MoldNumber { get; set; }

        // Estado seleccionado.
        [BindProperty(SupportsGet = true)]
        public string? MoldStatus { get; set; }

        // Resultados del reporte.
        public List<ReportMoldEntity> ReportList { get; set; } = new();

        public bool ReportExecuted { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            ReportExecuted = false;

            try
            {
                // La página abre sin ejecutar el reporte.
                if (!Request.Query.ContainsKey("runReport"))
                {
                    return;
                }

                ReportExecuted = true;

                ReportList = await _apiService.GetReportMolds(
                    MoldID,
                    MoldStatus
                );
            }
            catch (Exception ex)
            {
                ReportExecuted = true;
                ErrorMessage = ex.Message;
                ReportList = new List<ReportMoldEntity>();
            }
        }

        // GET:
        // /Reports/ReportMold?handler=SearchMolds&term=123
        public async Task<JsonResult> OnGetSearchMoldsAsync(string? term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return new JsonResult(new List<object>());
                }

                string endpoint =
                    $"Mold/bymoldnumber/{Uri.EscapeDataString(term.Trim())}";

                var molds =
                    await _apiService.GetAsync<MoldEntity>(endpoint);

                var result = molds
                    .Where(m =>
                        !string.IsNullOrWhiteSpace(m.MoldNumber)
                    )
                    .Select(m => new
                    {
                        id = m.MoldID,
                        moldNumber = m.MoldNumber,
                        description = m.MoldDescription,
                        text = string.IsNullOrWhiteSpace(m.MoldDescription)
                            ? m.MoldNumber
                            : $"{m.MoldNumber} - {m.MoldDescription}"
                    })
                    .Take(20)
                    .ToList();

                // Devuelve directamente una lista.
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

        public async Task<IActionResult> OnGetExportAsync()
        {
            try
            {
                var reportList = await _apiService.GetReportMolds(
                    MoldID,
                    MoldStatus
                );

                if (reportList == null || reportList.Count == 0)
                {
                    ReportExecuted = true;
                    ErrorMessage =
                        "No existen datos para exportar con los filtros seleccionados.";

                    ReportList = new List<ReportMoldEntity>();

                    return Page();
                }

                using var workbook = new XLWorkbook();

                var worksheet =
                    workbook.Worksheets.Add("Reporte de Moldes");

                // Encabezados
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Número de molde";
                worksheet.Cell(1, 3).Value = "Descripción";
                worksheet.Cell(1, 4).Value = "Número de activo";
                worksheet.Cell(1, 5).Value = "Estado";
                worksheet.Cell(1, 6).Value = "Origen";
                worksheet.Cell(1, 7).Value = "Plano digital";
                worksheet.Cell(1, 8).Value = "Criticidad";
                worksheet.Cell(1, 9).Value = "Gate";
                worksheet.Cell(1, 10).Value = "Colada";
                worksheet.Cell(1, 11).Value = "Actuador";
                worksheet.Cell(1, 12).Value = "Categorización";
                worksheet.Cell(1, 13).Value = "Cavidades";
                worksheet.Cell(1, 14).Value = "Cavidades bloqueadas";
                worksheet.Cell(1, 15).Value = "Tiene contador";
                worksheet.Cell(1, 16).Value = "Tipo de contador";
                worksheet.Cell(1, 17).Value = "Tres placas";
                worksheet.Cell(1, 18).Value = "Conteo inicial";
                worksheet.Cell(1, 19).Value = "Repuestos disponibles";
                worksheet.Cell(1, 20).Value =
                    "Reparación últimos 12 meses";
                worksheet.Cell(1, 21).Value =
                    "Comentario reparación";
                worksheet.Cell(1, 22).Value =
                    "Rechazo de calidad";
                worksheet.Cell(1, 23).Value =
                    "Comentario calidad";
                worksheet.Cell(1, 24).Value = "Creado por";
                worksheet.Cell(1, 25).Value = "Fecha creación";
                worksheet.Cell(1, 26).Value = "Modificado por";
                worksheet.Cell(1, 27).Value = "Fecha modificación";

                int row = 2;

                foreach (var item in reportList)
                {
                    worksheet.Cell(row, 1).Value =
                        item.MoldID;

                    worksheet.Cell(row, 2).Value =
                        item.MoldNumber ?? "";

                    worksheet.Cell(row, 3).Value =
                        item.MoldDescription ?? "";

                    worksheet.Cell(row, 4).Value =
                        item.MoldAssetNumber ?? "";

                    worksheet.Cell(row, 5).Value =
                        item.MoldStatus ?? "";

                    worksheet.Cell(row, 6).Value =
                        item.MoldOrigin ?? "";

                    worksheet.Cell(row, 7).Value =
                        Convert.ToString(item.DigitalPlane);

                    worksheet.Cell(row, 8).Value =
                        item.CriticallyType ?? "";

                    worksheet.Cell(row, 9).Value =
                        item.GateType ?? "";

                    worksheet.Cell(row, 10).Value =
                        item.CastingType ?? "";

                    worksheet.Cell(row, 11).Value =
                        item.ActuatorType ?? "";

                    worksheet.Cell(row, 12).Value =
                        item.CategorizationType ?? "";

                    worksheet.Cell(row, 13).Value =
                        Convert.ToString(item.CavityQty);

                    worksheet.Cell(row, 14).Value =
                        Convert.ToString(item.BlockCavityQty);

                    worksheet.Cell(row, 15).Value =
                        Convert.ToString(item.HaveCounter);

                    worksheet.Cell(row, 16).Value =
                        item.CounterType ?? "";

                    worksheet.Cell(row, 17).Value =
                        Convert.ToString(item.ThreeLayer);

                    worksheet.Cell(row, 18).Value =
                        Convert.ToString(item.InitialCount);

                    worksheet.Cell(row, 19).Value =
                        Convert.ToString(
                            item.Percentage_Spares_available
                        );

                    worksheet.Cell(row, 20).Value =
                        Convert.ToString(
                            item.Last_Reparir_12_Months
                        );

                    worksheet.Cell(row, 21).Value =
                        item.Comment_Last_Reparir_12_Months ?? "";

                    worksheet.Cell(row, 22).Value =
                        Convert.ToString(item.Quality_Issue);

                    worksheet.Cell(row, 23).Value =
                        item.Comment_Quality_Issue ?? "";

                    worksheet.Cell(row, 24).Value =
                        item.CreatedBy ?? "";

                    if (item.DateCreation.HasValue)
                    {
                        worksheet.Cell(row, 25).Value =
                            item.DateCreation.Value;

                        worksheet.Cell(row, 25)
                            .Style.DateFormat.Format =
                            "MM-dd-yyyy HH:mm:ss";
                    }

                    worksheet.Cell(row, 26).Value =
                        item.ModifiedBy ?? "";

                    if (item.DateModification.HasValue)
                    {
                        worksheet.Cell(row, 27).Value =
                            item.DateModification.Value;

                        worksheet.Cell(row, 27)
                            .Style.DateFormat.Format =
                            "MM-dd-yyyy HH:mm:ss";
                    }

                    row++;
                }

                var dataRange = worksheet.Range(
                    1,
                    1,
                    reportList.Count + 1,
                    27
                );

                dataRange.CreateTable();

                worksheet.SheetView.FreezeRows(1);

                worksheet.Columns().AdjustToContents();

                foreach (var column in worksheet.ColumnsUsed())
                {
                    if (column.Width > 50)
                    {
                        column.Width = 50;
                    }
                }

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                string fileName =
                    $"Reporte_Moldes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                ReportExecuted = true;

                ErrorMessage =
                    $"No fue posible exportar el reporte: {ex.Message}";

                ReportList = new List<ReportMoldEntity>();

                return Page();
            }
        }
    }
}

