using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class UserEntity : DBEntity
    {

        public int UserID { get; set; }

        public int? RolID { get; set; }

        public string? RolType { get; set; }

        public string? RolDescription { get; set; }

        public string? Username { get; set; }

        public string? EmpName { get; set; }

        public Boolean UserStatus { get; set; }

        public bool? RolStatus { get; set; }

    }
}
