using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Tracking_Tool_System.Services; // 👈 tu helper

namespace Tracking_Tool_System.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ActiveDirectoryHelper _adHelper;

        public LoginModel()
        {
            _adHelper = new ActiveDirectoryHelper();
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            // 🔥 Validación contra Active Directory
            bool isValid = _adHelper.AuthenticateUser(Username, Password);

            if (isValid)
            {
                var displayName = _adHelper.ReturnName(Username);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Username),
                    new Claim("DisplayName", displayName ?? Username)
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal
                );

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError("", "Credenciales inválidas");
            return Page();
        }
    }
}
