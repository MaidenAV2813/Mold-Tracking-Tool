using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY 
{
    public class MoldEntity : DBEntity
    {
        public int? MoldID { get; set; }
        public int? CriticallyID { get; set; }
        public int? GateID { get; set; }
        public int? CastingID { get; set; }
        public int? ActuatorID { get; set; }
        public int? CategorizationID { get; set; }
        public string? MoldAssetNumber { get; set; }
        public string? MoldNumber { get; set; }
        public string? MoldDescription { get; set; }
        public string? MoldStatus { get; set; }
        public string? MoldOrigin { get; set; }   
        public string? DigitalPlane { get; set; }
        public int? CavityQty { get; set; }
        public int? BlockCavityQty { get; set; }
        public string? HaveCounter { get; set; }
        public string? CounterType { get; set; }
        public string? ThreeLayer { get; set; }
        public int? InitialCount { get; set; }
        public string? Percentage_Spares_available { get; set; }
        public string? Last_Reparir_12_Months { get; set; }
        public string? Comment_Last_Reparir_12_Months { get; set; }
        public string? Quality_Issue { get; set; }
        public string? Comment_Quality_Issue { get; set; }

        public string? CastingType { get; set; }

        public string? ActuatorType { get; set; }

        public string? CriticallyType { get; set; }

        public string? GateType { get; set; }

        public string? CategorizationType { get; set; }


    }
}
