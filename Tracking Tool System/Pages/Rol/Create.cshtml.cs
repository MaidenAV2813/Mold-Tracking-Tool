using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Tracking_Tool_System.Pages.Rol
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 🔹 Campos del formulario
        [BindProperty]
        public string RolDescription { get; set; }

        [BindProperty]
        public string RolType { get; set; }

        [BindProperty]
        public bool Status { get; set; } = true;

        // 🔹 Cargar página
        public void OnGet()
        {
        }

        // 🔹 Guardar datos
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_Rol_Insert", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RolDescription", RolDescription);
                    cmd.Parameters.AddWithValue("@RolType", RolType);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@CreatedBy", User.Identity?.Name ?? "System");

                    cmd.ExecuteNonQuery();
                }

                // 🔹 Redirige al listado
                return RedirectToPage("/Rol/Index");
            }
            catch (Exception ex)
            {
                // 🔴 Muestra error en pantalla
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}