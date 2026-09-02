using CAPA_ENTITY;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace CAPA_WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryTransactions : ControllerBase
    {
        private readonly IInventoryTransactions_Services _inventorytransactionsServices;

        public InventoryTransactions(IInventoryTransactions_Services inventorytransactionsServices)
        {
            _inventorytransactionsServices = inventorytransactionsServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _inventorytransactionsServices.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryTransactionsEntity entity)
        {
            try
            {
                var result = await _inventorytransactionsServices.Create(entity);
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
                var result = await _inventorytransactionsServices.GetById(new InventoryTransactionsEntity
                {
                    TransactionID = id
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

    }
}
