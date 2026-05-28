using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IActuator_Services
    {
        Task<DBEntity> Create(ActuatorTypeEntity entity);
        Task<DBEntity> Delete(ActuatorTypeEntity entity);
        Task<IEnumerable<ActuatorTypeEntity>> Get();

    }
}
