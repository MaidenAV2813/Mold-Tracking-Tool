using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IMoldEvaluation_Services
    {
        Task<IEnumerable<MoldEvaluationEntity>> Get();
        Task<DBEntity> Create(MoldEvaluationEntity entity);
    }
}
