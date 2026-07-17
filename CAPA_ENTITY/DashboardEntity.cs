
namespace CAPA_ENTITY
{
    public class DashboardEntity
    {
        public int ActiveMolds { get; set; }

        public int PendingEvaluationsCurrentMonth { get; set; }

        public int TotalSparesInventory { get; set; }

        public string MaintenanceByMonthJson { get; set; } = "[]";

        public string TopAssignedSparesJson { get; set; } = "[]";

        public List<DashboardMaintenanceMonthEntity>
            MaintenanceByMonth
        { get; set; } = new();

        public List<DashboardTopSpareEntity>
            TopAssignedSpares
        { get; set; } = new();
    }

    public class DashboardMaintenanceMonthEntity
    {
        public int MonthNumber { get; set; }

        public string MonthName { get; set; } = string.Empty;

        public int MaintenanceCount { get; set; }
    }

    public class DashboardTopSpareEntity
    {
        public int? ItemNumberID { get; set; }

        public string ItemNumber { get; set; } = string.Empty;

        public string ItemDescription { get; set; } = string.Empty;

        public int TotalAssigned { get; set; }
    }
}
