using Microsoft.AspNetCore.Mvc;

namespace CAPA_PRESENTACION.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string usuario, string password)
        {
            // Aquí validas usuario y contraseña

            if (usuario == "admin" && password == "123")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View("Index");
        }
    }
}
