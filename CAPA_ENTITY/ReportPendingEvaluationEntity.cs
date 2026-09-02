namespace CAPA_ENTITY
{
    public class ReportPendingEvaluationEntity
    {
        public int MonthNumber { get; set; }

        public string? MonthName { get; set; }

        public int PendingQuantity { get; set; }

        public int ReportYear { get; set; }

        public int? MoldID { get; set; }

        public string? MoldNumber { get; set; }

        public string? MoldDescription { get; set; }

        public DateTime? DateEvaluation { get; set; }

        public DateTime? NextEvaluationDate { get; set; }
    }
}