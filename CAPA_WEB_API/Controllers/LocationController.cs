using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocation_Services _locationServices;

        public LocationController(ILocation_Services locationServices)
        {
            _locationServices = locationServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _locationServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationEntity entity)
        {
            try
            {
                var result = await _locationServices.Create(entity);
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
                var result = await _locationServices.GetById(new LocationEntity
                {
                    LocationID = id
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
        public async Task<IActionResult> Update([FromBody] LocationEntity entity)
        {
            try
            {
                var result = await _locationServices.Update(entity);
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
