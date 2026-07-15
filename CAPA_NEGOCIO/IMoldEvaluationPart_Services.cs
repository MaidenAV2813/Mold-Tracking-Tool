using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IMoldEvaluationPart_Services
    {
        Task<IEnumerable<MoldEvaluationPartEntity>> Get();
        Task<MoldEvaluationPartEntity> GetById(MoldEvaluationPartEntity entity);
        Task<DBEntity> Create(MoldEvaluationPartEntity entity);
        Task<DBEntity> Update(MoldEvaluationPartEntity entity);
        Task<DBEntity> Delete(MoldEvaluationPartEntity entity);
    }
}
