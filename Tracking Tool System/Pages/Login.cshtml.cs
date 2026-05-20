using CAPA_ENTITY;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Tracking_Tool_System.Services;

namespace Tracking_Tool_System.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ActiveDirectoryHelper _adHelper;
        private readonly ApiService _apiService;

        public LoginModel(ApiService apiService)
        {
            _apiService = apiService;
            _adHelper = new ActiveDirectoryHelper();
        }

        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError(string.Empty, "Debe ingresar usuario y contraseña.");
                return Page();
            }

            bool isValid = _adHelper.AuthenticateUser(Username, Password);

            if (!isValid)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return Page();
            }

            var users = await _apiService.GetAsync<UserEntity>("users");

            var usuarioSistema = users.FirstOrDefault(x =>
                !string.IsNullOrWhiteSpace(x.Username) &&
                x.Username.Trim().ToLower() == Username.Trim().ToLower() &&
                x.UserStatus == true);

            if (usuarioSistema == null)
            {
                ModelState.AddModelError(string.Empty, "Usted no tiene los permisos para accesar al sistema.");
                return Page();
            }

            var accessList = await _apiService.GetAsync<AccessEntity>("access");

            var accesosUsuario = accessList
                .Where(x => x.RolID == usuarioSistema.RolID)
                .Select(x => x.AccessDescription?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (!accesosUsuario.Any())
            {
                ModelState.AddModelError(string.Empty, "Usted no tiene accesos configurados para ingresar al sistema.");
                return Page();
            }

            var displayName = _adHelper.ReturnName(Username);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Username),
                new Claim("DisplayName", displayName ?? Username),
                new Claim("UserID", usuarioSistema.UserID.ToString()),
                new Claim("RolID", usuarioSistema.RolID.ToString() ?? ""),
                new Claim("RolType", usuarioSistema.RolType ?? ""),
                new Claim("EmpName", usuarioSistema.EmpName ?? "")
            };

            foreach (var acceso in accesosUsuario)
            {
                claims.Add(new Claim("Access", acceso!));
            }

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
    }
}