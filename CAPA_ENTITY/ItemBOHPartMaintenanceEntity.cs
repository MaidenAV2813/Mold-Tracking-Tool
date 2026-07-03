using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class ItemBOHPartMaintenanceEntity : DBEntity
    {
        public int? ItemNumberID { get; set; }
        public string? ItemNumber { get; set; }
        public string? ItemDescription { get; set; }
        public int? LocationID { get; set; }
        public string? LocationNumber { get; set; }
        public int? QtyOnHand { get; set; }
    }
}
