using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class CastingMoldEntity : DBEntity
    {
        public int? CastingID { get; set; }
        public string? CastingType { get; set; }
        public Boolean CastingStatus { get; set; }
    }
}
