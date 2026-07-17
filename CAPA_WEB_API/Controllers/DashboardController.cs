using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboard_Services _dashboardServices;

        public DashboardController(
            IDashboard_Services dashboardServices)
        {
            _dashboardServices = dashboardServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result =
                    await _dashboardServices.Get();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new DBEntity
                {
                    CodeError = ex.HResult,
                    MsgError = ex.Message
                });
            }
        }
    }
}
