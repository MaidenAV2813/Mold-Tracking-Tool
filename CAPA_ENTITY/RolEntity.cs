using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class RolEntity : DBEntity
    {
        public int? RolID { get; set; }
        public string? RolDescription { get; set; }
        public string? RolType { get; set; }
        public Boolean RolStatus { get; set; }

    }
}
