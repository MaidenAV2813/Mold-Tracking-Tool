using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY 
{
    public class MoldPartEvaluationEntity : DBEntity
    {
        public int? MoldPartEvaID { get; set; }
        public int? EvaluationID { get; set; }
        public int? MoldEvaPartID { get; set; }
        public string? Parts { get; set; }
        public int? Score { get; set; }
        public string? Observation { get; set; }

    }
}
