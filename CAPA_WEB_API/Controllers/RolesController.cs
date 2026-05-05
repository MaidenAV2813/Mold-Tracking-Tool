using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoles_Services _rolesServices;

        public RolesController(IRoles_Services rolesServices)
        {
            _rolesServices = rolesServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _rolesServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolEntity entity)
        {
            try
            {
                var result = await _rolesServices.Create(entity);
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
