using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Customers
{
    public abstract class CustomerResult
    {
        public class Index
        {
            public IEnumerable<CustomerDto.Index>? Customers { get; set; }
            public int TotalAmount { get; set; }
        }

        public class Detail
        {
            public CustomerDto.Detail Customer { get; set; }
        }

        public class Create
        {
            public int Id { get; set; }
        }

        public class Edit
        {
            public int Id { get; set; }
        }

        public class Delete
        {

        }
    }
}
