using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class InventoryBOHEntity : DBEntity
    {
        public int? BOHID { get; set; }
        public int? ItemNumberID { get; set; }
        public int? LocationID { get; set; }
        public int? QtyOnHand { get; set; }
        public string? ItemNumber { get; set; }
        public string? LocationNumber { get; set; }
        public decimal? QuantityOnHand { get; set; }
        public string? UOM { get; set; }
        public string? ItemStatus { get; set; }
        public string? ActualSupplier { get; set; }
    }
}
