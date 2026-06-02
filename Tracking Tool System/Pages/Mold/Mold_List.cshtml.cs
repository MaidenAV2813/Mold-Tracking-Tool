using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CAPA_ENTITY;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Mold
{
    public class Mold_ListModel : PageModel
    {
        private readonly ApiService _apiService;

        public Mold_ListModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public List<MoldEntity> Mold { get; set; } = new();

        public List<MoldEntity> MoldFilterList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchMold { get; set; }

        public int? SelectedMoldID { get; set; }

        public IEnumerable<MoldEntity> GridList { get; set; } = new List<MoldEntity>();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                Mold = await _apiService.GetAsync<MoldEntity>("mold");

                MoldFilterList = Mold
                    .GroupBy(x => x.MoldNumber)
                    .Select(g => g.First())
                    .OrderBy(x => x.MoldNumber)
                    .ToList();

                GridList = Mold;

                if (!string.IsNullOrWhiteSpace(SearchMold))
                {
                    GridList = Mold
                        .Where(x => x.MoldNumber != null &&
                                    x.MoldNumber.Contains(SearchMold, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    SelectedMoldID = GridList.FirstOrDefault()?.MoldID;
                }

                return Page();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public async Task<IActionResult> OnPostEliminar(int id)
        {
            try
            {
                var result = await _apiService.PostAsync("mold/delete", new MoldEntity
                {
                    MoldID = id
                });

                var content = await result.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch (Exception ex)
            {
                return new JsonResult(new DBEntity
                {
                    CodeError = ex.HResult,
                    MsgError = ex.Message
                });
            }
        }
    }
}