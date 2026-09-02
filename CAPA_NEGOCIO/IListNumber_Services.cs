using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IListNumber_Services
    {
        Task<IEnumerable<ListNumberEntity>> Get();
        Task<IEnumerable<ListNumberEntity>> GetByMoldID(ListNumberEntity entity);
        Task<ListNumberEntity> GetById(ListNumberEntity entity);
        Task<DBEntity> Create(ListNumberEntity entity);
        Task<DBEntity> Update(ListNumberEntity entity);
        Task<DBEntity> Delete(ListNumberEntity entity);

    }
}
