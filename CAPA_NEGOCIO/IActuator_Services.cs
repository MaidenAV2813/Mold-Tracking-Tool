using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IActuator_Services
    {
        Task<DBEntity> Create(ActuatorTypeEntity entity);
        Task<IEnumerable<ActuatorTypeEntity>> Get();
        Task<ActuatorTypeEntity> GetById(ActuatorTypeEntity entity);
        Task<DBEntity> Update(ActuatorTypeEntity entity);
    }
}
