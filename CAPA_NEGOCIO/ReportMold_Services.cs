using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;

namespace CAPA_NEGOCIO
{
    public class ReportMold_Services : IReportMold_Services
    {
        private readonly IDataAccess sql;

        public ReportMold_Services(IDataAccess _sql)
        {
            sql = _sql;
        }

        #region MetodosConsulta

        public async Task<IEnumerable<ReportMoldEntity>> Get(
            int? moldID,
            string? moldStatus
        )
        {
            try
            {
                var result = sql.QueryAsync<ReportMoldEntity>(
                    "sp_Report_Molds",
                    new
                    {
                        MoldID = moldID,
                        MoldStatus = moldStatus
                    });

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}