using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class PartMaintenanceEntity : DBEntity
    {
        public int? PartMaintenanceID { get; set; }
        public string? OrderNum { get; set; }
        public int? ItemNumberID { get; set; }
        public string? ItemNumber { get; set; }
        public string? ItemDescription { get; set; }
        public int? QtyAsigned {  get; set; }     
    }
}
