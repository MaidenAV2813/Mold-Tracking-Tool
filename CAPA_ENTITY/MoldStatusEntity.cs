using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    internal class MoldStatusEntity
    {
        public int? StatusID { get; set; }
        public string? StatusType { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime DateModification { get; set; }

    }
}
