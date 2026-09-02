using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriticallyController : ControllerBase
    {
        private readonly ICritically_Services _criticallyServices;

        public CriticallyController(ICritically_Services criticallyServices)
        {
            _criticallyServices = criticallyServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _criticallyServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CriticallyMoldEntity entity)
        {
            try
            {
                var result = await _criticallyServices.Create(entity);
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
                var result = await _criticallyServices.GetById(new CriticallyMoldEntity
                    {
                        CriticallyID = id
                    });

                if (result == null)
                {
                    return NotFound(new DBEntity
                    {
                        CodeError = -1,
                        MsgError =
                            "No se encontró el tipo de Criticidad seleccionada."
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
        public async Task<IActionResult> Update([FromBody] CriticallyMoldEntity entity)
        {
            try
            {
                var result = await _criticallyServices.Update(entity);
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
        //public async Task<IActionResult> Delete([FromBody] CriticallyMoldEntity entity)
        //{
        //    try
        //    {
        //        var result = await _criticallyServices.Delete(entity);
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
