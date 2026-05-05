using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Tracking_Tool_System.Pages.Rol
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public int RolID { get; set; }

        [BindProperty]
        public string RolDescription { get; set; }

        [BindProperty]
        public string RolType { get; set; }

        [BindProperty]
        public bool Status { get; set; }

        public void OnGet(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_Rol_SelectById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RolID", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    RolID = Convert.ToInt32(reader["RolID"]);
                    RolDescription = reader["RolDescription"].ToString();
                    RolType = reader["RolType"].ToString();
                    Status = Convert.ToBoolean(reader["Status"]);
                }
            }
        }

        public IActionResult OnPost()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("sp_Rol_Update", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RolID", RolID);
                cmd.Parameters.AddWithValue("@RolDescription", RolDescription);
                cmd.Parameters.AddWithValue("@RolType", RolType);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@ModifiedBy", User.Identity?.Name ?? "System");

                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("/Rol/Index");
        }
    }
}
