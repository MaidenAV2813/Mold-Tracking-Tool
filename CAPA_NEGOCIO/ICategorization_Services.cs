using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface ICategorization_Services
    {
        Task<DBEntity> Create(CategorizationMoldEntity entity);
        Task<DBEntity> Delete(CategorizationMoldEntity entity);
        Task<IEnumerable<CategorizationMoldEntity>> Get();

    }
}
