using System;
using System.Collections.Generic;
using System.Linq;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportMoldController : ControllerBase
    {
        private readonly IReportMold_Services _reportMoldServices;
        private readonly IMold_Services _moldServices;

        public ReportMoldController(
            IReportMold_Services reportMoldServices,
            IMold_Services moldServices)
        {
            _reportMoldServices = reportMoldServices;
            _moldServices = moldServices;
        }

        // GET: api/ReportMold
        // GET: api/ReportMold?moldID=1
        // GET: api/ReportMold?moldStatus=Activo
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int? moldID = null,
            [FromQuery] string? moldStatus = null)
        {
            try
            {
                var result = await _reportMoldServices.Get(
                    moldID,
                    moldStatus
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "Ocurrió un error al consultar el reporte de moldes.",
                        error = ex.Message
                    }
                );
            }
        }

        // GET: api/ReportMold/search?term=123
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return Ok(new List<object>());
                }

                var molds = await _moldServices.Get();

                var result = molds
                    .Where(m =>
                        !string.IsNullOrWhiteSpace(m.MoldNumber)
                        && m.MoldNumber.Contains(
                            term.Trim(),
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    .Select(m => new
                    {
                        id = m.MoldID,
                        moldNumber = m.MoldNumber,
                        description = m.MoldDescription
                    })
                    .Take(20)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "Ocurrió un error al buscar los moldes.",
                        error = ex.Message
                    }
                );
            }
        }
    }
}