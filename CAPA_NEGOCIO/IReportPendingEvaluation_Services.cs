using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface
        IReportPendingEvaluation_Services
    {
        Task<IEnumerable<ReportPendingEvaluationEntity>>Get(int? year, bool detail = false);
        Task<IEnumerable<ReportPendingEvaluationEntity>>GetDetail(int? year);
    }
}