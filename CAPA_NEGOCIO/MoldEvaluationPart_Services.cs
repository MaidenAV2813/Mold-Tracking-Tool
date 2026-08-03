using CAPA_DATOS;
using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public class MoldEvaluationPart_Services
        : IMoldEvaluationPart_Services
    {
        private readonly IDataAccess sql;

        public MoldEvaluationPart_Services(IDataAccess _sql)
        {
            sql = _sql;
        }

        public async Task<IEnumerable<MoldEvaluationPartEntity>> Get()
        {
            return await sql.QueryAsync<MoldEvaluationPartEntity>("sp_MoldEvaluationPart_List",new { });
        }

        public async Task<MoldEvaluationPartEntity> GetById(MoldEvaluationPartEntity entity)
        {
            return await sql.QueryFirstAsync<MoldEvaluationPartEntity>(
                "sp_MoldEvaluationPart_GetById",
                new
                {
                    entity.MoldEvaPartID
                });
        }

        public async Task<DBEntity> Create(MoldEvaluationPartEntity entity)
        {
            return await sql.ExecuteAsync("sp_MoldEvaluationPart_Insert",new
                {
                    entity.Parts,
                    entity.DateCreation,
                    entity.DateModification,
                    entity.CreatedBy,
                    entity.ModifiedBy,
                    entity.PartsStatus
                });
        }

        public async Task<DBEntity> Update(MoldEvaluationPartEntity entity)
        {
            return await sql.ExecuteAsync("sp_MoldEvaluationPart_Update",new
                {
                    entity.MoldEvaPartID,
                    entity.Parts,
                    entity.DateModification,
                    entity.ModifiedBy,
                    entity.PartsStatus
                });
        }

        //public async Task<DBEntity> Delete(MoldEvaluationPartEntity entity)
        //{
        //    return await sql.ExecuteAsync("sp_MoldEvaluationPart_Delete",new
        //        {
        //            entity.MoldEvaPartID
        //        });
        //}
    }
}