using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class vw_EBS_List_NumbersController : ControllerBase
    {
        private readonly Ivw_EBS_List_Numbers_Services _services;

        public vw_EBS_List_NumbersController(
            Ivw_EBS_List_Numbers_Services services)
        {
            _services = services;
        }

        [HttpGet("bylistnumber/{listnumber}")]
        public async Task<IActionResult> GetByListnumber(
            string listnumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(listnumber))
                {
                    return BadRequest("Debe indicar el número de parte.");
                }

                var result =
                    await _services.GetByListnumber(listnumber.Trim());

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