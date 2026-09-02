using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IInventoryTransactions_Services
    {
        Task<DBEntity> Create(InventoryTransactionsEntity entity);
        Task<IEnumerable<InventoryTransactionsEntity>> Get();
        Task<InventoryTransactionsEntity> GetById(InventoryTransactionsEntity entity);
    }
}
