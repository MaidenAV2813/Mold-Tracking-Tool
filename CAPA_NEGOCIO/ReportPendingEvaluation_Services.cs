using CAPA_DATOS;
using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public class ReportPendingEvaluation_Services
        : IReportPendingEvaluation_Services
    {
        private readonly IDataAccess sql;

        public ReportPendingEvaluation_Services(
            IDataAccess sql)
        {
            this.sql = sql;
        }

        public async Task<IEnumerable<ReportPendingEvaluationEntity>>
        Get(int? year, bool detail = false)
        {
            return await sql.QueryAsync<ReportPendingEvaluationEntity>(
                "sp_Report_PendingEvaluationsByMonth",
                new
                {
                    Year = year,
                    Detail = detail
                });
        }

        public async Task<
        IEnumerable<ReportPendingEvaluationEntity>>
        GetDetail(int? year)
        {
            return await sql.QueryAsync<
                ReportPendingEvaluationEntity
            >(
                "sp_Report_PendingEvaluationsByMonth",
                new
                {
                    Year = year,
                    Detail = true
                }
            );
        }
    }
}