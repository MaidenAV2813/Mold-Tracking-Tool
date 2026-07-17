using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public interface IDashboard_Services
    {
        Task<DashboardEntity> Get();
    }
}