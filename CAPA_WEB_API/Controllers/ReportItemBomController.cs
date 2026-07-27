using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace Tracking_Tool_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportItemBomController : ControllerBase
    {
        private readonly IReportItemBom_Services service;

        public ReportItemBomController(
            IReportItemBom_Services _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<IEnumerable<ReportItemBomEntity>> Get()
        {
            return await service.Get();
        }
    }
}