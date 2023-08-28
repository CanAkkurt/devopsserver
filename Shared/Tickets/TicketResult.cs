
using Shared.Customers;

namespace Shared.Tickets
{
    public class TicketResult
    {
        public class Index
        {
            public IEnumerable<TicketDto.Index>? Tickets { get; set; }
            public int TotalAmount { get; set; }
        }

        public class Detail
        {
            public TicketDto.Detail Ticket { get; set; }
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
