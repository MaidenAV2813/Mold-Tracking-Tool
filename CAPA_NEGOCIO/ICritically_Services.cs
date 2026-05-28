using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface ICritically_Services
    {
        Task<DBEntity> Create(CriticallyMoldEntity entity);
        Task<DBEntity> Delete(CriticallyMoldEntity entity);
        Task<IEnumerable<CriticallyMoldEntity>> Get();

    }
}
