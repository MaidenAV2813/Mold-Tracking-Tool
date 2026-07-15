using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class ListNumberEntity : DBEntity
    {

        public int? ListNumberID { get; set; }

        public int? MoldID { get; set; }
        public string? MoldNumber { get; set; }

        public string? ListNumber { get; set; }

        public string? Description { get; set; }
    }
}
