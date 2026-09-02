using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CastingController : ControllerBase
    {
        private readonly ICasting_Services _castingServices;

        public CastingController(ICasting_Services castingServices)
        {
            _castingServices = castingServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _castingServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CastingMoldEntity entity)
        {
            try
            {
                var result = await _castingServices.Create(entity);
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
                var result = await _castingServices.GetById(new CastingMoldEntity
                    {
                        CastingID = id
                    });

                if (result == null)
                {
                    return NotFound(new DBEntity
                    {
                        CodeError = -1,
                        MsgError =
                            "No se encontró el tipo de Colada seleccionada."
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
        public async Task<IActionResult> Update([FromBody] CastingMoldEntity entity)
        {
            try
            {
                var result = await _castingServices.Update(entity);
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
        //public async Task<IActionResult> Delete([FromBody] CastingMoldEntity entity)
        //{
        //    try
        //    {
        //        var result = await _castingServices.Delete(entity);
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
