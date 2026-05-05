using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IRoles_Services
    {
        Task<DBEntity> Create(RolEntity entity);
        Task<DBEntity> Delete(RolEntity entity);
        Task<IEnumerable<RolEntity>> Get();
        Task<RolEntity> GetById(RolEntity entity);
        Task<DBEntity> Update(RolEntity entity);
    }
}
