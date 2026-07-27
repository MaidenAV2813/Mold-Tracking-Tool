using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IMoldEvaluation_Services
    {
        Task<IEnumerable<MoldEvaluationEntity>> Get();
        Task<MoldEvaluationEntity> GetById(int evaluationID);
        Task<IEnumerable<MoldPartEvaluationEntity>> GetPartsByEvaluationID(int evaluationID);
        Task<DBEntity> Create(MoldEvaluationEntity entity);
        Task<IEnumerable<MoldEvaluationEntity>> GetReport(
            int? moldID,
            DateTime? startDate,
            DateTime? endDate
        );

        Task<MoldEvaluationEntity> GetReportDetail(
            int evaluationID
        );
    }
}
