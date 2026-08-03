using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategorizationController : ControllerBase
    {
        private readonly ICategorization_Services _categorizationServices;

        public CategorizationController(ICategorization_Services categorizationServices)
        {
            _categorizationServices = categorizationServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _categorizationServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategorizationMoldEntity entity)
        {
            try
            {
                var result = await _categorizationServices.Create(entity);
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
                var result = await _categorizationServices.GetById(new CategorizationMoldEntity
                {
                    CategorizationID = id
                    });

                if (result == null)
                {
                    return NotFound(new DBEntity
                    {
                        CodeError = -1,
                        MsgError =
                            "No se encontró el tipo de Categorizacion seleccionado."
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
        public async Task<IActionResult> Update([FromBody] CategorizationMoldEntity entity)
        {
            try
            {
                var result = await _categorizationServices.Update(entity);
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
        //public async Task<IActionResult> Delete([FromBody] CategorizationMoldEntity entity)
        //{
        //    try
        //    {
        //        var result = await _categorizationServices.Delete(entity);
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
