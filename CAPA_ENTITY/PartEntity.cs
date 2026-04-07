using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    internal class PartEntity
    {
        public int? ItemID { get; set; }
        public int? MoldID { get; set; }
        public string? ItemNumber { get; set; }
        public string? Descriptión { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime DateModification { get; set; }
    }
}
