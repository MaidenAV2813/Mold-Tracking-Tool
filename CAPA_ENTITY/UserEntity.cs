using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class UserEntity : DBEntity
    {
        public int? RolID { get; set; }

        public string? RolDescription { get; set; }

        public int? IdNumber { get; set; }

        public string? EmpName { get; set; }

        public Boolean UserStatus { get; set; }

      }
}
