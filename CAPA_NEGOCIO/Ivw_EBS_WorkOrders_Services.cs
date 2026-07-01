using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface Ivw_EBS_WorkOrders_Services
    {
        Task<IEnumerable<vw_EBS_WorkOrdersEntity>> GetByOrder(string orderNum);
    }
}
