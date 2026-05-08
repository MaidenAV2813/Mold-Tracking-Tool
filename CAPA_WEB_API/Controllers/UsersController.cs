using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsers_Services _UsersServices;

        public UsersController(IUsers_Services UsersServices)
        {
            _UsersServices = UsersServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _UsersServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserEntity entity)
        {
            try
            {
                var result = await _UsersServices.Create(entity);
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
                var result = await _UsersServices.GetById(new UserEntity
                {
                    IdNumber = id
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
        public async Task<IActionResult> Update([FromBody] UserEntity entity)
        {
            try
            {
                var result = await _UsersServices.Update(entity);
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
