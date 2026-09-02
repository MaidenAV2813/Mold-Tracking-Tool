using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryBOHController : ControllerBase
    {
        private readonly IInventoryBOH_Services _inventorybohServices;

        public InventoryBOHController(IInventoryBOH_Services inventorybohServices)
        {
            _inventorybohServices = inventorybohServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _inventorybohServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryBOHEntity entity)
        {
            try
            {
                var result = await _inventorybohServices.Create(entity);
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
                var result = await _inventorybohServices.GetById(new InventoryBOHEntity
                {
                    BOHID = id
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

    }
}
