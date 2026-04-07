using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    internal class RolEntity
    {
        public int? RolID { get; set; }
        public string? RolDescription { get; set; }
        public string? RolType { get; set; }
        public string? Status { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime DateModification { get; set; }
    }
}
