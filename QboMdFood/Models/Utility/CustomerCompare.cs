using System.Collections.Generic;
using QboMdFood.Models.API;

namespace QboMdFood.Models.Utility
{
    public class CustomerCompare : IEqualityComparer<Record>
    {
        public int GetHashCode(Record co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.Customer_id.GetHashCode();
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
            return x1.Customer_id == x2.Customer_id;
        }
    }
}