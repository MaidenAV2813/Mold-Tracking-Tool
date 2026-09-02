using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActuatorController : ControllerBase
    {
        private readonly IActuator_Services _actuatorServices;

        public ActuatorController(IActuator_Services actuatorServices)
        {
            _actuatorServices = actuatorServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _actuatorServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActuatorTypeEntity entity)
        {
            try
            {
                var result = await _actuatorServices.Create(entity);
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _actuatorServices.GetById(new ActuatorTypeEntity
                    {
                        ActuatorID = id
                    });

                if (result == null)
                {
                    return NotFound(new DBEntity
                    {
                        CodeError = -1,
                        MsgError =
                            "No se encontró el tipo de Actuador seleccionado."
                    });
                }

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


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ActuatorTypeEntity entity)
        {
            try
            {
                var result = await _actuatorServices.Update(entity);
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

        //[HttpPost("delete")]
        //public async Task<IActionResult> Delete([FromBody] ActuatorTypeEntity entity)
        //{
        //    try
        //    {
        //        var result = await _actuatorServices.Delete(entity);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new DBEntity
        //        {
        //            CodeError = ex.HResult,
        //            MsgError = ex.Message
        //        });
        //    }
        //}

    }
}
