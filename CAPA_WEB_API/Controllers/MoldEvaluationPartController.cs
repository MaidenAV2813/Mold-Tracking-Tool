using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoldEvaluationPartController : ControllerBase
    {
        private readonly IMoldEvaluationPart_Services _services;

        public MoldEvaluationPartController(
            IMoldEvaluationPart_Services services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _services.Get());
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

        [HttpGet("byid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _services.GetById(
                    new MoldEvaluationPartEntity
                    {
                        MoldEvaPartID = id
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

        [HttpPost]
        public async Task<IActionResult> Create(
            MoldEvaluationPartEntity entity)
        {
            try
            {
                return Ok(await _services.Create(entity));
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
        public async Task<IActionResult> Update(
            MoldEvaluationPartEntity entity)
        {
            try
            {
                return Ok(await _services.Update(entity));
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

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        return Ok(await _services.Delete(
        //            new MoldEvaluationPartEntity
        //            {
        //                MoldEvaPartID = id
        //            }));
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
