using System.Text.Json;
using CAPA_DATOS;
using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public class MoldEvaluation_Services : IMoldEvaluation_Services
    {
        private readonly IDataAccess sql;

        public MoldEvaluation_Services(IDataAccess _sql)
        {
            sql = _sql;
        }

        public async Task<IEnumerable<MoldEvaluationEntity>> Get()
        {
            var result = sql.QueryAsync<MoldEvaluationEntity>(
                "sp_MoldEvaluation_List",
                new { });

            return await result;
        }

        public async Task<DBEntity> Create(MoldEvaluationEntity entity)
        {
            string evaluationPartsJson = JsonSerializer.Serialize(
                entity.EvaluationParts.Select(x => new
                {
                    x.MoldEvaPartID,
                    x.Score,
                    x.Observation
                }));

            var result = sql.ExecuteAsync(
                "sp_MoldEvaluation_Insert",
                new
                {
                    entity.MoldID,
                    entity.DateEvaluation,
                    EvaluationPartsJson = evaluationPartsJson,
                    entity.DateCreation,
                    entity.DateModification,
                    entity.CreatedBy,
                    entity.ModifiedBy
                });

            return await result;
        }

        public async Task<MoldEvaluationEntity> GetById(int evaluationID)
        {
            return await sql.QueryFirstAsync<MoldEvaluationEntity>(
                "sp_MoldEvaluation_GetById",
                new
                {
                    EvaluationID = evaluationID
                });
        }

        public async Task<IEnumerable<MoldPartEvaluationEntity>>
        GetPartsByEvaluationID(int evaluationID)
        {
            return await sql.QueryAsync<MoldPartEvaluationEntity>(
                "sp_MoldPartEvaluation_GetByEvaluationID",
                new
                {
                    EvaluationID = evaluationID
                });
        }
    }
}