using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class AccessEntity : DBEntity
    {

        public int? AccessID { get; set; }

        public int? RolID { get; set; }

        public string? RolDescription { get; set; }

        public string? AccessDescription { get; set; }

        public string? RolType { get; set; }

        public bool? RolStatus { get; set; }
    }
}
