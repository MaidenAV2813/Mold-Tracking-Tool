using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartMaintenanceController : ControllerBase
    {
        private readonly IPartMaintenance_Services _partMaintenanceServices;

        public PartMaintenanceController(IPartMaintenance_Services partMaintenanceServices)
        {
            _partMaintenanceServices = partMaintenanceServices;
        }

        [HttpGet("{orderNum}")]
        public async Task<IActionResult> Get(string orderNum)
        {
            try
            {
                var result = await _partMaintenanceServices.Get(orderNum);
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

        [HttpGet("byid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _partMaintenanceServices.GetById(new PartMaintenanceEntity
                {
                    PartMaintenanceID = id
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
        public async Task<IActionResult> Create([FromBody] PartMaintenanceEntity entity)
        {
            try
            {
                var result = await _partMaintenanceServices.Create(entity);
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
        public async Task<IActionResult> Update([FromBody] PartMaintenanceEntity entity)
        {
            try
            {
                var result = await _partMaintenanceServices.Update(entity);
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
                var result = await _partMaintenanceServices.Delete(
                    new PartMaintenanceEntity
                    {
                        PartMaintenanceID = id
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
        
    }
}
