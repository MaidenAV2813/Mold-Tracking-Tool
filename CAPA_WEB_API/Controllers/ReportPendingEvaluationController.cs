using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace Tracking_Tool_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportPendingEvaluationController
        : ControllerBase
    {
        private readonly
            IReportPendingEvaluation_Services
            _reportPendingEvaluationServices;

        public ReportPendingEvaluationController(
            IReportPendingEvaluation_Services
                reportPendingEvaluationServices)
        {
            _reportPendingEvaluationServices =
                reportPendingEvaluationServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportPendingEvaluationEntity>>> Get(
        [FromQuery] int? year,
        [FromQuery] bool detail = false)
        {
            var result = await _reportPendingEvaluationServices.Get(year, detail);

            return Ok(result);
        }
    }
}