using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class ItemBomEntity : DBEntity
    {
        public int? ItemNumberID { get; set; }
        public int? MoldID { get; set; }
        public string? MoldNumber { get; set; }
        public string? ItemNumber { get; set; }
        public string? ICUItemNumber {  get; set; }
        public string? ItemDescription { get; set; }
        public int? ItemCost { get; set; }
        public int? ItemInvMin { get; set; }
        public int? ItemInvMax { get; set; }
        public string? ItemSupplierNumber { get; set; }
        public string? ActualSupplier { get; set; }
        public string? UOM { get; set; }
        public int? ItemCritically { get; set; }
        public string? ItemStatus { get; set; }
        public int? Plano { get; set; }

    }
}
