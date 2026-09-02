using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CAPA_ENTITY;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Reports
{
    public class ReportItemBomModel : PageModel
    {
        private readonly ApiService _apiService;

        public ReportItemBomModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public List<ReportItemBomEntity> ReportList { get; set; }
            = new List<ReportItemBomEntity>();

        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ErrorMessage =
                    TempData["ErrorMessage"]?.ToString();
            }

            try
            {
                var result =
                    await _apiService.GetReportItemBom();

                ReportList =
                    result?.ToList()
                    ?? new List<ReportItemBomEntity>();
            }
            catch (Exception ex)
            {
                ReportList =
                    new List<ReportItemBomEntity>();

                ErrorMessage =
                    "Ocurrió un error al cargar el reporte: "
                    + ex.Message;
            }
        }

        public async Task<IActionResult> OnGetExportAsync()
        {
            try
            {
                var result =
                    await _apiService.GetReportItemBom();

                var report =
                    result?.ToList()
                    ?? new List<ReportItemBomEntity>();

                if (report.Count == 0)
                {
                    TempData["ErrorMessage"] =
                        "No existen registros para exportar.";

                    return RedirectToPage(
                        "/Reports/ReportItemBom"
                    );
                }

                using var workbook =
                    new XLWorkbook();

                var worksheet =
                    workbook.Worksheets.Add(
                        "Reporte Repuestos"
                    );

                worksheet.Cell(1, 1).Value =
                    "Número de molde";

                worksheet.Cell(1, 2).Value =
                    "Descripción del molde";

                worksheet.Cell(1, 3).Value =
                    "Criticidad del molde";

                worksheet.Cell(1, 4).Value =
                    "Número de parte";

                worksheet.Cell(1, 5).Value =
                    "Descripción del repuesto";

                worksheet.Cell(1, 6).Value =
                    "Categorización";

                worksheet.Cell(1, 7).Value =
                    "Criticidad del repuesto";

                worksheet.Cell(1, 8).Value =
                    "Índice de criticidad";

                worksheet.Cell(1, 9).Value =
                    "Número de proveedor";

                worksheet.Cell(1, 10).Value =
                    "Proveedor actual";

                worksheet.Cell(1, 11).Value =
                    "Inventario total";

                worksheet.Cell(1, 12).Value =
                    "Índice de rotación";

                worksheet.Cell(1, 13).Value =
                    "Nivel de compra";

                worksheet.Cell(1, 14).Value =
                    "Costo del repuesto";

                worksheet.Cell(1, 15).Value =
                    "Inventario mínimo";

                worksheet.Cell(1, 16).Value =
                    "Inventario máximo";

                worksheet.Cell(1, 17).Value =
                    "Porcentaje";

                worksheet.Cell(1, 18).Value =
                    "Plano";

                var headerRange =
                    worksheet.Range(1, 1, 1, 18);

                headerRange.Style.Font.Bold = true;

                headerRange.Style.Font.FontColor =
                    XLColor.White;

                headerRange.Style.Fill.BackgroundColor =
                    XLColor.DarkBlue;

                headerRange.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                headerRange.Style.Alignment.Vertical =
                    XLAlignmentVerticalValues.Center;

                headerRange.Style.Border.BottomBorder =
                    XLBorderStyleValues.Thin;

                headerRange.Style.Border.TopBorder =
                    XLBorderStyleValues.Thin;

                headerRange.Style.Border.LeftBorder =
                    XLBorderStyleValues.Thin;

                headerRange.Style.Border.RightBorder =
                    XLBorderStyleValues.Thin;

                int row = 2;

                foreach (var item in report)
                {
                    worksheet.Cell(row, 1).Value =
                        item.MoldNumber ?? string.Empty;

                    worksheet.Cell(row, 2).Value =
                        item.MoldDescription ?? string.Empty;

                    worksheet.Cell(row, 3).Value =
                        item.CriticallyType ?? string.Empty;

                    worksheet.Cell(row, 4).Value =
                        item.ItemNumber ?? string.Empty;

                    worksheet.Cell(row, 5).Value =
                        item.ItemDescription ?? string.Empty;

                    worksheet.Cell(row, 6).Value =
                        item.CategorizationType ;

                    worksheet.Cell(row, 7).Value =
                        item.ItemCritically ;

                    worksheet.Cell(row, 8).Value =
                        item.CriticalityIndex ?? 0;

                    worksheet.Cell(row, 9).Value =
                        item.ItemSupplierNumber ?? string.Empty;

                    worksheet.Cell(row, 10).Value =
                        item.ActualSupplier ?? string.Empty;

                    worksheet.Cell(row, 11).Value =
                        item.TotalQtyOnHand ?? 0;

                    worksheet.Cell(row, 12).Value =
                        item.IndexRotation ?? 0;

                    worksheet.Cell(row, 13).Value =
                        item.PurchaseLevel ?? string.Empty;

                    worksheet.Cell(row, 14).Value =
                        item.ItemCost ?? 0;

                    worksheet.Cell(row, 15).Value =
                        item.ItemInvMin ?? 0;

                    worksheet.Cell(row, 16).Value =
                        item.ItemInvMax ?? 0;

                    worksheet.Cell(row, 17).Value =
                        item.Porcentage ?? 0;

                    worksheet.Cell(row, 18).Value =
                        item.Plano ?? 0;

                    row++;
                }

                var dataRange =
                    worksheet.Range(
                        2,
                        1,
                        report.Count + 1,
                        18
                    );

                dataRange.Style.Border.BottomBorder =
                    XLBorderStyleValues.Thin;

                dataRange.Style.Border.TopBorder =
                    XLBorderStyleValues.Thin;

                dataRange.Style.Border.LeftBorder =
                    XLBorderStyleValues.Thin;

                dataRange.Style.Border.RightBorder =
                    XLBorderStyleValues.Thin;

                worksheet.Column(11)
                    .Style.NumberFormat.Format = "0.00";

                worksheet.Column(12)
                    .Style.NumberFormat.Format = "0.00";

                worksheet.Column(14)
                    .Style.NumberFormat.Format = "#,##0.00";

                worksheet.Column(15)
                    .Style.NumberFormat.Format = "0.00";

                worksheet.Column(16)
                    .Style.NumberFormat.Format = "0.00";

                worksheet.Column(17)
                    .Style.NumberFormat.Format = "0.00";

                worksheet.Column(18)
                    .Style.NumberFormat.Format = "0.00";

                var completeRange =
                    worksheet.Range(
                        1,
                        1,
                        report.Count + 1,
                        18
                    );

                completeRange.SetAutoFilter();

                worksheet.SheetView.FreezeRows(1);

                worksheet.Columns().AdjustToContents();

                worksheet.Column(2).Width = 35;
                worksheet.Column(5).Width = 45;
                worksheet.Column(10).Width = 30;

                worksheet.Column(2)
                    .Style.Alignment.WrapText = true;

                worksheet.Column(5)
                    .Style.Alignment.WrapText = true;

                worksheet.Column(10)
                    .Style.Alignment.WrapText = true;

                for (
                    int currentRow = 2;
                    currentRow <= report.Count + 1;
                    currentRow++
                )
                {
                    var purchaseCell =
                        worksheet.Cell(
                            currentRow,
                            13
                        );

                    string purchaseLevel =
                        purchaseCell.GetString();

                    if (
                        purchaseLevel.Equals(
                            "COMPRAR",
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    {
                        purchaseCell.Style
                            .Fill.BackgroundColor =
                            XLColor.LightPink;

                        purchaseCell.Style
                            .Font.FontColor =
                            XLColor.DarkRed;

                        purchaseCell.Style
                            .Font.Bold = true;
                    }
                    else if (
                        purchaseLevel.Equals(
                            "NO COMPRAR",
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    {
                        purchaseCell.Style
                            .Fill.BackgroundColor =
                            XLColor.LightGreen;

                        purchaseCell.Style
                            .Font.FontColor =
                            XLColor.DarkGreen;

                        purchaseCell.Style
                            .Font.Bold = true;
                    }
                }

                using var stream =
                    new MemoryStream();

                workbook.SaveAs(stream);

                string fileName =
                    $"Reporte_Repuestos_Molde_" +
                    $"{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-" +
                    "officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    "Error al exportar el reporte: "
                    + ex.Message;

                return RedirectToPage(
                    "/Reports/ReportItemBom"
                );
            }
        }
    }
}

