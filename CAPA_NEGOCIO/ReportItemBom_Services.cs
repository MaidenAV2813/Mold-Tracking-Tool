using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;

namespace CAPA_NEGOCIO
{
    public class ReportItemBom_Services : IReportItemBom_Services
    {
        private readonly IDataAccess sql;

        public ReportItemBom_Services(IDataAccess _sql)
        {
            sql = _sql;
        }

        public async Task<IEnumerable<ReportItemBomEntity>> Get()
        {
            try
            {
                var result = sql.QueryAsync<ReportItemBomEntity>(
                    "sp_Report_ItemBom"
                );

                return await result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}