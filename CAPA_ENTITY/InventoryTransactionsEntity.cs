using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class InventoryTransactionsEntity : DBEntity
    {
        public int? TransactionID { get; set; }
        public int? ItemNumberID { get; set; }
        public string? ItemNumber { get; set; }
        public int? TransactionTypeID { get; set; }
        public string? TransactionType { get; set; }
        public int? LocationID { get; set; }
        public string? LocationNumber { get; set; }
        public int? Qty { get; set; }
        public string? Comments { get; set; }
        public string? ActualSupplier { get; set; }

    }
}
