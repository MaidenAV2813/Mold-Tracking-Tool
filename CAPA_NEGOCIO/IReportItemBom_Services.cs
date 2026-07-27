using System.Collections.Generic;
using System.Threading.Tasks;
using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IReportItemBom_Services
    {
        Task<IEnumerable<ReportItemBomEntity>> Get();
    }
}