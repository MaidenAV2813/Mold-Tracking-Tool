using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : ControllerBase
    {
        private readonly IAccess_Services _AccessServices;

        public AccessController(IAccess_Services AccessServices)
        {
            _AccessServices = AccessServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _AccessServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccessEntity entity)
        {
            try
            {
                var result = await _AccessServices.Create(entity);
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

        //Edit Metodos

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _AccessServices.GetById(new AccessEntity
                {
                    AccessID = id
                });

                if (result == null)
                    return NotFound();

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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AccessEntity entity)
        {
            try
            {
                var result = await _AccessServices.Update(entity);
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
        [HttpPost("deletebyrol")]
        public async Task<IActionResult> DeleteByRol([FromBody] AccessEntity entity)
        {
            var result = await _AccessServices.DeleteByRol(entity);
            return Ok(result);
        }
    }
}
