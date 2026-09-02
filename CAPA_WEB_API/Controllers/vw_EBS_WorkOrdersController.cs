using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class vw_EBS_WorkOrdersController : ControllerBase
    {
        private readonly Ivw_EBS_WorkOrders_Services _vw_ebs_workordersServices;

        public vw_EBS_WorkOrdersController(Ivw_EBS_WorkOrders_Services vw_ebs_workordersServices)
        {
            _vw_ebs_workordersServices = vw_ebs_workordersServices;
        }


        [HttpGet("byorder/{orderNum}")]
        public async Task<IActionResult> GetByOrder(string orderNum)
        {
            try
            {
                var result = await _vw_ebs_workordersServices.GetByOrder(orderNum);

                if (result == null || !result.Any())
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
