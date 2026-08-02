using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages.Casting
{
    public class EditModel : PageModel
    {
        private readonly ApiService _apiService;

        public EditModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public int? CastingID { get; set; }

        [BindProperty]
        public string? CastingType { get; set; }

        [BindProperty]
        public bool CastingStatus { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var casting = await _apiService.GetSingleAsync<CastingMoldEntity>($"casting/{id}");

            if (casting == null)
                return NotFound();

            CastingID = casting.CastingID;
            CastingType = casting.CastingType;
            CastingStatus = casting.CastingStatus;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = User.Identity?.Name ?? "System";
            var now = DateTime.Now;
            var entity = new CastingMoldEntity
            {
                CastingID = CastingID,
                CastingType = CastingType,
                CastingStatus = CastingStatus,
                ModifiedBy = user,
                DateModification = now
            };

            var response = await _apiService.PutAsync("casting/Update", entity);

            var result = await response.Content.ReadFromJsonAsync<DBEntity>();

            if (result == null)
            {
                TempData["ErrorMessage"] = "La API no devolvió una respuesta válida.";

                return Page();
            }

            if (result.CodeError != 0)
            {
                TempData["ErrorMessage"] = result.MsgError ?? "No fue posible crear la colada.";

                return Page();
            }

            TempData["SuccessMessage"] = result.MsgError ?? "Estado actualizado correctamente.";

            return RedirectToPage("/Casting/Casting_List");
        }
    }
}