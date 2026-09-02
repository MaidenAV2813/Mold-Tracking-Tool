using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface ILocation_Services
    {
        Task<DBEntity> Create(LocationEntity entity);
        Task<DBEntity> Delete(LocationEntity entity);
        Task<IEnumerable<LocationEntity>> Get();
        Task<LocationEntity> GetById(LocationEntity entity);
        Task<DBEntity> Update(LocationEntity entity);
    }
}
