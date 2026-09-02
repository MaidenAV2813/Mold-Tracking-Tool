using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IMold_Services
    {
        Task<DBEntity> Create(MoldEntity entity);
        Task<DBEntity> Delete(MoldEntity entity);
        Task<IEnumerable<MoldEntity>> Get();
        Task<MoldEntity> GetById(MoldEntity entity);
        Task<DBEntity> Update(MoldEntity entity);
    }
}
