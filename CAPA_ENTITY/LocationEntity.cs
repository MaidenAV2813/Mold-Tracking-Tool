using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class LocationEntity : DBEntity
    {
        public int? LocationID { get; set; }
        public string? LocationNumber { get; set; }
        public string? LocationStatus { get; set; }

    }
}
