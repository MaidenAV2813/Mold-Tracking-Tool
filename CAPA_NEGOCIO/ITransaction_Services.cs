using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface ITransaction_Services
    {
        Task<DBEntity> Create(TransactionEntity entity);
        Task<DBEntity> Delete(TransactionEntity entity);
        Task<IEnumerable<TransactionEntity>> Get();
        Task<TransactionEntity> GetById(TransactionEntity entity);
        Task<DBEntity> Update(TransactionEntity entity);
    }
}
