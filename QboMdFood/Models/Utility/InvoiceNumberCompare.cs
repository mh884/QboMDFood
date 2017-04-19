using System.Collections.Generic;
using QboMdFood.Models.API;

namespace QboMdFood.Models.Utility
{
    public class InvoiceNumberCompare : IEqualityComparer<Record>
    {
        public int GetHashCode(Record co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.Invoice_number.GetHashCode();
        }

        public bool Equals(Record x1, Record x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.Invoice_number == x2.Invoice_number;
        }
    }
}