using Shared.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Members
{
    public static class MemberRequest
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
            public MemberDto.Create Member { get; set; }
        }

        public class Edit
        {
            public int Id { get; set; }
            public MemberDto.Update Member { get; set; }
        }

        public class Delete
        {
            public int Id { get; set; }
        }
    }
}
