using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface ICritically_Services
    {
        Task<DBEntity> Create(CriticallyMoldEntity entity);
        Task<IEnumerable<CriticallyMoldEntity>> Get();
        Task<CriticallyMoldEntity> GetById(CriticallyMoldEntity entity);
        Task<DBEntity> Update(CriticallyMoldEntity entity);

    }
}
