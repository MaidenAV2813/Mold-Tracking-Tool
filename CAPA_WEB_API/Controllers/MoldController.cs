using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoldController : ControllerBase
    {
        private readonly IMold_Services _moldServices;

        public MoldController(IMold_Services moldServices)
        {
            _moldServices = moldServices;
        }

        // GET: api/Mold
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _moldServices.Get();

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

        // GET: api/Mold/bymoldnumber/123
        // Busca únicamente por número de molde.
        [HttpGet("bymoldnumber/{term}")]
        public async Task<IActionResult> GetByMoldNumber(string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return Ok(new List<MoldEntity>());
                }

                string searchTerm = term.Trim();

                var molds = await _moldServices.Get();

                if (molds == null)
                {
                    return Ok(new List<MoldEntity>());
                }

                var result = molds
                    .Where(m =>
                        !string.IsNullOrWhiteSpace(m.MoldNumber)
                        &&
                        m.MoldNumber
                            .Trim()
                            .Contains(
                                searchTerm,
                                StringComparison.OrdinalIgnoreCase
                            )
                    )
                    .Take(20)
                    .ToList();

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

        // GET: api/Mold/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _moldServices.GetById(
                    new MoldEntity
                    {
                        MoldID = id
                    }
                );

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

        // POST: api/Mold
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] MoldEntity entity)
        {
            try
            {
                var result = await _moldServices.Create(entity);

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

        // PUT: api/Mold
        [HttpPut]
        public async Task<IActionResult> Update(
            [FromBody] MoldEntity entity)
        {
            try
            {
                var result = await _moldServices.Update(entity);

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