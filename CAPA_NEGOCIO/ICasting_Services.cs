using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface ICasting_Services
    {
        Task<DBEntity> Create(CastingMoldEntity entity);
        Task<IEnumerable<CastingMoldEntity>> Get();
        Task<CastingMoldEntity> GetById(CastingMoldEntity entity);
        Task<DBEntity> Update(CastingMoldEntity entity);
    }
}
