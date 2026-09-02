using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IItemBom_Services
    {
        Task<DBEntity> Create(ItemBomEntity entity);
        Task<DBEntity> Delete(ItemBomEntity entity);
        Task<IEnumerable<ItemBomEntity>> Get();
        Task<ItemBomEntity> GetById(ItemBomEntity entity);
        Task<DBEntity> Update(ItemBomEntity entity);
    }
}
