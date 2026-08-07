using CAPA_ENTITY;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.InventoryBOH
{
    public class InventoryBOH_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public InventoryBOH_ListModel(
            ApiService apiService)
        {
            _apiService = apiService;
        }

        public IEnumerable<InventoryBOHEntity> GridList { get; set; }
            = new List<InventoryBOHEntity>();

        public List<InventoryBOHEntity> Part { get; set; }
            = new();

        public List<InventoryBOHEntity> PartFilterList { get; set; }
            = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchPart { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchLocation { get; set; }

        public int? SelectedItemNumberID { get; set; }

        public List<LocationEntity> LocationList { get; set; }
            = new();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                await LoadDataAsync();

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message
                );

                return Page();
            }
        }

        public async Task<IActionResult> OnGetExportAsync(
            string? searchPart,
            string? searchLocation)
        {
            try
            {
                List<InventoryBOHEntity> inventory =
                    await _apiService
                        .GetAsync<InventoryBOHEntity>(
                            "inventoryboh"
                        );

                IEnumerable<InventoryBOHEntity> query =
                    inventory;

                if (!string.IsNullOrWhiteSpace(searchPart))
                {
                    string value = searchPart.Trim();

                    query = query.Where(x =>
                        !string.IsNullOrWhiteSpace(
                            x.ItemNumber
                        )
                        &&
                        x.ItemNumber.Contains(
                            value,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );
                }

                if (!string.IsNullOrWhiteSpace(searchLocation))
                {
                    string value = searchLocation.Trim();

                    query = query.Where(x =>
                        !string.IsNullOrWhiteSpace(
                            x.LocationNumber
                        )
                        &&
                        string.Equals(
                            x.LocationNumber.Trim(),
                            value,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );
                }

                List<InventoryBOHEntity> exportList =
                    query
                        .OrderBy(x => x.ItemNumber)
                        .ThenBy(x => x.LocationNumber)
                        .ToList();

                using var workbook =
                    new XLWorkbook();

                var worksheet =
                    workbook.Worksheets.Add(
                        "Balance de Inventario"
                    );

                // Título
                worksheet.Cell("A1").Value =
                    "BALANCE DE INVENTARIO";

                worksheet.Range("A1:G1").Merge();

                worksheet.Cell("A1")
                    .Style.Font.Bold = true;

                worksheet.Cell("A1")
                    .Style.Font.FontSize = 16;

                worksheet.Cell("A1")
                    .Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                worksheet.Cell("A2").Value =
                    $"Fecha de generación: " +
                    $"{DateTime.Now:MM-dd-yyyy HH:mm:ss}";

                worksheet.Range("A2:G2").Merge();

                worksheet.Cell("A2")
                    .Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                int currentRow = 4;

                // Mostrar filtros utilizados
                if (!string.IsNullOrWhiteSpace(searchPart))
                {
                    worksheet.Cell(currentRow, 1).Value =
                        "Filtro número de parte:";

                    worksheet.Cell(currentRow, 2).Value =
                        searchPart.Trim();

                    worksheet.Cell(currentRow, 1)
                        .Style.Font.Bold = true;

                    currentRow++;
                }

                if (!string.IsNullOrWhiteSpace(searchLocation))
                {
                    worksheet.Cell(currentRow, 1).Value =
                        "Filtro localidad:";

                    worksheet.Cell(currentRow, 2).Value =
                        searchLocation.Trim();

                    worksheet.Cell(currentRow, 1)
                        .Style.Font.Bold = true;

                    currentRow++;
                }

                if (currentRow > 4)
                {
                    currentRow++;
                }

                int headerRow = currentRow;

                // Encabezados
                worksheet.Cell(headerRow, 1).Value =
                    "Número de parte";

                worksheet.Cell(headerRow, 2).Value =
                    "Descripción";

                worksheet.Cell(headerRow, 3).Value =
                    "Localidad";

                worksheet.Cell(headerRow, 4).Value =
                    "Cantidad";

                worksheet.Cell(headerRow, 5).Value =
                    "Unidad de medida";

                worksheet.Cell(headerRow, 6).Value =
                    "Estado";

                worksheet.Cell(headerRow, 7).Value =
                    "Suplidor";

                var headerRange =
                    worksheet.Range(
                        headerRow,
                        1,
                        headerRow,
                        7
                    );

                headerRange.Style.Font.Bold = true;

                headerRange.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                headerRange.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;

                headerRange.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;

                currentRow = headerRow + 1;

                foreach (var item in exportList)
                {
                    worksheet.Cell(currentRow, 1).Value =
                        item.ItemNumber ?? string.Empty;

                    worksheet.Cell(currentRow, 2).Value =
                        item.ItemDescription ?? string.Empty;

                    worksheet.Cell(currentRow, 3).Value =
                        item.LocationNumber ?? string.Empty;

                    if (item.QtyOnHand.HasValue)
                    {
                        worksheet.Cell(currentRow, 4).Value =
                            Convert.ToDouble(
                                item.QtyOnHand.Value
                            );
                    }

                    worksheet.Cell(currentRow, 5).Value =
                        item.UOM ?? string.Empty;

                    worksheet.Cell(currentRow, 6).Value =
                        item.ItemStatus ?? string.Empty;

                    worksheet.Cell(currentRow, 7).Value =
                        item.ActualSupplier ?? string.Empty;

                    currentRow++;
                }

                if (exportList.Count == 0)
                {
                    worksheet.Cell(currentRow, 1).Value =
                        "No se encontraron registros.";

                    worksheet.Range(
                        currentRow,
                        1,
                        currentRow,
                        6
                    ).Merge();

                    worksheet.Cell(currentRow, 1)
                        .Style.Alignment.Horizontal =
                            XLAlignmentHorizontalValues.Center;

                    currentRow++;
                }

                var dataRange =
                    worksheet.Range(
                        headerRow,
                        1,
                        currentRow - 1,
                        7
                    );

                dataRange.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;

                dataRange.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;

                // Formato de cantidad
                if (currentRow > headerRow + 1)
                {
                    worksheet.Range(
                        headerRow + 1,
                        3,
                        currentRow - 1,
                        3
                    ).Style.NumberFormat.Format =
                        "#,##0.00";
                }

                // Filtro de Excel
                if (exportList.Count > 0)
                {
                    worksheet.Range(
                        headerRow,
                        1,
                        currentRow - 1,
                        7
                    ).SetAutoFilter();
                }

                worksheet.SheetView
                    .FreezeRows(headerRow);

                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 25;
                worksheet.Column(3).Width = 15;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 10;
                worksheet.Column(7).Width = 20;

                worksheet.Columns(1, 7)
                    .Style.Alignment.Vertical =
                        XLAlignmentVerticalValues.Center;

                worksheet.Column(7)
                    .Style.Alignment.WrapText = true;

                using var stream =
                    new MemoryStream();

                workbook.SaveAs(stream);

                string fileName =
                    $"Balance_Inventario_" +
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
                    "No fue posible exportar el balance de inventario. "
                    + ex.Message;

                return RedirectToPage(
                    "./InventoryBOH_List"
                );
            }
        }

        private async Task LoadDataAsync()
        {
            Part = await _apiService
                .GetAsync<InventoryBOHEntity>(
                    "inventoryboh"
                );

            PartFilterList = Part
                .GroupBy(x => x.ItemNumber)
                .Select(g => g.First())
                .OrderBy(x => x.ItemNumber)
                .ToList();

            LocationList =
                (await _apiService
                    .GetAsync<LocationEntity>(
                        "location"
                    ))
                .OrderBy(x => x.LocationNumber)
                .ToList();

            IEnumerable<InventoryBOHEntity> query =
                Part;

            if (!string.IsNullOrWhiteSpace(SearchPart))
            {
                string value =
                    SearchPart.Trim();

                query = query.Where(x =>
                    !string.IsNullOrWhiteSpace(
                        x.ItemNumber
                    )
                    &&
                    x.ItemNumber.Contains(
                        value,
                        StringComparison.OrdinalIgnoreCase
                    )
                );
            }

            if (!string.IsNullOrWhiteSpace(SearchLocation))
            {
                string value =
                    SearchLocation.Trim();

                query = query.Where(x =>
                    !string.IsNullOrWhiteSpace(
                        x.LocationNumber
                    )
                    &&
                    string.Equals(
                        x.LocationNumber.Trim(),
                        value,
                        StringComparison.OrdinalIgnoreCase
                    )
                );
            }

            GridList = query
                .OrderBy(x => x.ItemNumber)
                .ThenBy(x => x.LocationNumber)
                .ToList();

            SelectedItemNumberID =
                GridList
                    .FirstOrDefault()?
                    .ItemNumberID;
        }
    }
}