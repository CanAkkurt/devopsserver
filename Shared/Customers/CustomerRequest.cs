using Shared.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Customers
{
    public static class CustomerRequest
    {
        public class Index
        {
            public string? Searchterm { get; set; }
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 25;
        }

        public class Detail
        {
            public int Id { get; set; }
        }

        public class Create
        {
            public CustomerDto.Create Customer { get; set; }
        }

        public class Edit
        {
            public int Id { get; set; }
            public CustomerDto.Update Customer { get; set; }
        }

        public class Delete
        {
            public int Id { get; set; }
        }

        
    }
}
