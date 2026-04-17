using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    internal class ActuatorTypeEntity
    {
        public int ActuatorID { get; set; }
        public string? ActuatorType { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
