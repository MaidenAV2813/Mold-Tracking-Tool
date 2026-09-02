using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY 
{
    public class MoldEvaluationPartEntity : DBEntity
    {
        public int? MoldEvaPartID { get; set; }
        public string? Parts { get; set; }
        public Boolean PartsStatus { get; set; }
    }
}
