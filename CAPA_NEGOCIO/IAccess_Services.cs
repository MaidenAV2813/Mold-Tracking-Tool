using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IAccess_Services
    {
        Task<IEnumerable<AccessEntity>> Get();
        Task<AccessEntity> GetById(AccessEntity entity);
        Task<DBEntity> Create(AccessEntity entity);
        Task<DBEntity> Update(AccessEntity entity);
        Task<DBEntity> Delete(AccessEntity entity);
        Task<DBEntity> DeleteByRol(AccessEntity entity);

    }
}
