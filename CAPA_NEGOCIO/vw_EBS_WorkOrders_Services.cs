using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using CAPA_ENTITY;
using CAPA_DATOS;




namespace CAPA_NEGOCIO
{
    public class vw_EBS_WorkOrders_Services : Ivw_EBS_WorkOrders_Services
    {
        private readonly IDataAccess sql;

        public vw_EBS_WorkOrders_Services(IDataAccess _sql)
        {
            sql = _sql;
        }

        public async Task<IEnumerable<vw_EBS_WorkOrdersEntity>> GetByOrder(string orderNum)
        {
            var result = sql.QueryAsync<vw_EBS_WorkOrdersEntity>(
                "sp_EBS_WorkOrders_Get",
                new
                {
                    WIP_ENTITY_NAME = orderNum
                });

            return await result;
        }
    }
}

