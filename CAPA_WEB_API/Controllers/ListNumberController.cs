using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListNumberController : ControllerBase
    {
        private readonly IListNumber_Services _ListNumberServices;

        public ListNumberController(IListNumber_Services ListNumberServices)
        {
            _ListNumberServices = ListNumberServices;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _ListNumberServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ListNumberEntity entity)
        {
            try
            {
                var result = await _ListNumberServices.Create(entity);
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
                var result = await _ListNumberServices.GetById(
                    new ListNumberEntity
                    {
                        ListNumberID = id
                    });

                if (result == null)
                {
                    return NotFound(
                        "No se encontró el ListNumber seleccionado."
                    );
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

        [HttpPut]
        public async Task<IActionResult> Update(
    [FromBody] ListNumberEntity entity)
        {
            try
            {
                var result = await _ListNumberServices.Update(entity);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = User.Identity?.Name ?? "System";

                var result = await _ListNumberServices.Delete(
                    new ListNumberEntity
                    {
                        ListNumberID = id,
                        ModifiedBy = user
                    });

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

        [HttpGet("bymold/{moldId}")]
        public async Task<IActionResult> GetByMoldID(int moldId)
        {
            try
            {
                var result =
                    await _ListNumberServices.GetByMoldID(
                        new ListNumberEntity
                        {
                            MoldID = moldId
                        }
                    );

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
