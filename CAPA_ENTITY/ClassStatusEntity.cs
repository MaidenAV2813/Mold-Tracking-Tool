using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    internal class ClassStatusEntity
    {
        public int? ClassID { get; set; }
        public int? StatusID { get; set; }
        public int? Id_Estatus_id_Clase { get; set; }
        public string? ClassType { get; set; }
        public string? Where { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime DateModification { get; set; }
    }
}
