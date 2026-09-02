using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class vw_EBS_WorkOrdersEntity : DBEntity
    {
        public string? WIP_ENTITY_NAME { get; set; }
        public string? ASSET_NUMBER { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime? DATE_COMPLETED { get; set; }
    }
}
