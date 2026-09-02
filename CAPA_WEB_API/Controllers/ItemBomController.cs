using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemBomController : ControllerBase
    {
        private readonly IItemBom_Services _itembomServices;

        public ItemBomController(IItemBom_Services itembomServices)
        {
            _itembomServices = itembomServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _itembomServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemBomEntity entity)
        {
            try
            {
                var result = await _itembomServices.Create(entity);
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
                var result = await _itembomServices.GetById(new ItemBomEntity
                {
                    ItemNumberID = id
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
        public async Task<IActionResult> Update([FromBody] ItemBomEntity entity)
        {
            try
            {
                var result = await _itembomServices.Update(entity);
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

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] ItemBomEntity entity)
        {
            try
            {
                var result = await _itembomServices.Delete(entity);
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
