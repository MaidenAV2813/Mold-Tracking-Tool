using System.Collections.Generic;
using System.Threading.Tasks;
using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IReportMold_Services
    {
        Task<IEnumerable<ReportMoldEntity>> Get(int? moldID,string? moldStatus
        );
    }
}