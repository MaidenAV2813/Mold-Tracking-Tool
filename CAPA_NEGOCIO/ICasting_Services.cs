using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface ICasting_Services
    {
        Task<DBEntity> Create(CastingMoldEntity entity);
        Task<DBEntity> Delete(CastingMoldEntity entity);
        Task<IEnumerable<CastingMoldEntity>> Get();

    }
}
