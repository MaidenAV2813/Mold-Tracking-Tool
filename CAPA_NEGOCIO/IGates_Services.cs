using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IGates_Services
    {
        Task<DBEntity> Create(GateTypeEntity entity);
        Task<DBEntity> Delete(GateTypeEntity entity);
        Task<IEnumerable<GateTypeEntity>> Get();
        Task<GateTypeEntity> GetById(GateTypeEntity entity);
        Task<DBEntity> Update(GateTypeEntity entity);
    }
}
