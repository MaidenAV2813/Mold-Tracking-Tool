using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatesController : ControllerBase
    {
        private readonly IGates_Services _gatesServices;

        public GatesController(IGates_Services gatesServices)
        {
            _gatesServices = gatesServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _gatesServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GateTypeEntity entity)
        {
            try
            {
                var result = await _gatesServices.Create(entity);
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
                var result = await _gatesServices.GetById(new GateTypeEntity
                {
                    GateID = id
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
        public async Task<IActionResult> Update([FromBody] GateTypeEntity entity)
        {
            try
            {
                var result = await _gatesServices.Update(entity);
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
