using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY 
{
    public class MoldEvaluationEntity : DBEntity
    {
        public int? EvaluationID { get; set; }
        public int? MoldID { get; set; }
        public string? MoldNumber { get; set; }
        public DateTime? DateEvaluation { get; set; }
        public DateTime? NextEvaluationDate { get; set; }
        public decimal? GeneralScore { get; set; }
        public List<MoldPartEvaluationEntity> EvaluationParts { get; set; } = new();
        

    }
}
