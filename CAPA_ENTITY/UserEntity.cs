using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    internal class UserEntity
    {
        public int? UserID { get; set; }
        public int? RolID { get; set; }
        public string? User { get; set; }
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Rol { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime DateModification { get; set; }
    }
}
