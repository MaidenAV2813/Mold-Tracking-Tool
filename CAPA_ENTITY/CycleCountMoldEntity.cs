using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    internal class CycleCountMoldEntity
    {
        public int? CycleCountID { get; set; }
        public int? MoldID { get; set; }
        public string? Origin { get; set; }
        public int OracleInitialCount { get; set; }
        public int SupplierInitialCount { get; set; }
        public int ManualInitialCount { get; set; }
    }
}
