using Shared.Customers;
using Shared.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Members
{
    public class MemberResult
    {
        public class Index
        {
            public IEnumerable<MemberDto.Index>? Members { get; set; }
            public int TotalAmount { get; set; }
        }

        public class Detail
        {
            public MemberDto.Detail Member { get; set; }
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
