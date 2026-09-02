using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAPA_DATOS;
using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public class vw_EBS_List_Numbers_Services
        : Ivw_EBS_List_Numbers_Services
    {
        private readonly IDataAccess sql;

        public vw_EBS_List_Numbers_Services(IDataAccess _sql)
        {
            sql = _sql ?? throw new ArgumentNullException(nameof(_sql));
        }

        public async Task<IEnumerable<vw_EBS_List_NumbersEntity>>
            GetByListnumber(string listnumber)
        {
            if (string.IsNullOrWhiteSpace(listnumber))
            {
                return new List<vw_EBS_List_NumbersEntity>();
            }

            var result =
                await sql.QueryAsync<vw_EBS_List_NumbersEntity>(
                    "sp_vw_EBS_List_Numbers_List",
                    new
                    {
                        SEGMENT1 = listnumber.Trim()
                    }
                );

            return result;
        }
    }
}