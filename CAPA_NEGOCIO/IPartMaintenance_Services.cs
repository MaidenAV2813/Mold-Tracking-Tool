using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IPartMaintenance_Services
    {
        Task<DBEntity> Create(PartMaintenanceEntity entity);
        Task<IEnumerable<PartMaintenanceEntity>> Get(string orderNum);
        Task<PartMaintenanceEntity> GetById(PartMaintenanceEntity entity);
        Task<DBEntity> Update(PartMaintenanceEntity entity);
        Task<DBEntity> Delete(PartMaintenanceEntity entity);
    }
}
