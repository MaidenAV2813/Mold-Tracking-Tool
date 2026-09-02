using System.Text.Json;
using CAPA_DATOS;
using CAPA_ENTITY;

namespace CAPA_NEGOCIO
{
    public class Dashboard_Services : IDashboard_Services
    {
        private readonly IDataAccess sql;

        public Dashboard_Services(IDataAccess _sql)
        {
            sql = _sql;
        }

        public async Task<DashboardEntity> Get()
        {
            var result =
                await sql.QueryFirstAsync<DashboardEntity>(
                    "sp_Dashboard_Get",
                    new { });

            if (result == null)
            {
                return new DashboardEntity();
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            result.MaintenanceByMonth =
                JsonSerializer.Deserialize<
                    List<DashboardMaintenanceMonthEntity>>(
                        result.MaintenanceByMonthJson ?? "[]",
                        jsonOptions)
                ?? new List<DashboardMaintenanceMonthEntity>();

            result.TopAssignedSpares =
                JsonSerializer.Deserialize<
                    List<DashboardTopSpareEntity>>(
                        result.TopAssignedSparesJson ?? "[]",
                        jsonOptions)
                ?? new List<DashboardTopSpareEntity>();

            return result;
        }
    }
}