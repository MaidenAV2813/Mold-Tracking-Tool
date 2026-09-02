using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class DBEntity
    {
        public int? CodeError { get; set; }
        public string? MsgError { get; set; }
        public DateTime? DateCreation { get; set; }

        public DateTime? DateModification { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
