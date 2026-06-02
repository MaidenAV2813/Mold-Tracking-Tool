using System;
using System.Collections.Generic;
using System.Text;

namespace CAPA_ENTITY
{
    public class TransactionEntity : DBEntity
    {
        public int? TransactionID { get; set; }
        public string? TransactionType { get; set; }
        public string? TransactionStatus { get; set; }

    }
}
