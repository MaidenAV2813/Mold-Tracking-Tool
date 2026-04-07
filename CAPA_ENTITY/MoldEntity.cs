using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    internal class MoldEntity
    {
        public int? MoldID { get; set; }
        public int? StatusID { get; set; }
        public int? CriticallyID { get; set; }
        public int? GateID { get; set; }
        public int? CastingID { get; set; }
        public int? ActuatorID { get; set; }
        public string ActiveNumber { get; set; }
        public string? MoldNumber { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime DateModification { get; set; }
        public DateTime DateEvaluation { get; set; }
        public DateTime DateNextEvaluation { get; set; }
        public string? DigitalPlane { get; set; }
        public int? CavityQty { get; set; }
        public string? HaveCounter { get; set; }
        public string? Countertype { get; set; }
        public string? ThreeLayer { get; set; }
    }
}
