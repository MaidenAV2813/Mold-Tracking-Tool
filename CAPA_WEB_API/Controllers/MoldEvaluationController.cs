using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoldEvaluationController : ControllerBase
    {
        private readonly IMoldEvaluation_Services _services;

        public MoldEvaluationController(
            IMoldEvaluation_Services services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _services.Get();

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
        public async Task<IActionResult> Create(
            MoldEvaluationEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    return BadRequest(new DBEntity
                    {
                        CodeError = -1,
                        MsgError =
                            "No se recibieron los datos de la evaluación."
                    });
                }

                var result = await _services.Create(entity);

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _services.GetById(id);

                if (result == null)
                {
                    return NotFound();
                }

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

        [HttpGet("{id}/parts")]
        public async Task<IActionResult> GetParts(int id)
        {
            try
            {
                var result =
                    await _services.GetPartsByEvaluationID(id);

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

        [HttpGet("report")]
        public async Task<IActionResult> GetReport(
        [FromQuery] int? moldID,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
        {
            try
            {
                if (
                    startDate.HasValue
                    && endDate.HasValue
                    && startDate.Value.Date >
                       endDate.Value.Date
                )
                {
                    return BadRequest(new DBEntity
                    {
                        CodeError = -1,
                        MsgError =
                            "La fecha inicial no puede ser mayor que la fecha final."
                    });
                }

                var result =
                    await _services.GetReport(
                        moldID,
                        startDate,
                        endDate
                    );

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

        [HttpGet("report/detail/{evaluationID}")]
        public async Task<IActionResult> GetReportDetail(
            int evaluationID)
        {
            try
            {
                var result =
                    await _services.GetReportDetail(
                        evaluationID
                    );

                if (result == null)
                {
                    return NotFound(new DBEntity
                    {
                        CodeError = -1,
                        MsgError =
                            "No se encontró la evaluación seleccionada."
                    });
                }

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