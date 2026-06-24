using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IInventoryBOH_Services
    {
        Task<DBEntity> Create(InventoryBOHEntity entity);
        Task<IEnumerable<InventoryBOHEntity>> Get();
        Task<InventoryBOHEntity> GetById(InventoryBOHEntity entity);

    }
}
