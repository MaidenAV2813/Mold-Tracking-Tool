using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface ICategorization_Services
    {
        Task<DBEntity> Create(CategorizationMoldEntity entity);
        Task<IEnumerable<CategorizationMoldEntity>> Get();
        Task<CategorizationMoldEntity> GetById(CategorizationMoldEntity entity);
        Task<DBEntity> Update(CategorizationMoldEntity entity);

    }
}
