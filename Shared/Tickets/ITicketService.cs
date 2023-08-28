using Shared.VirtualMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tickets
{
    public interface ITicketService
    {
        Task<TicketResult.Index> GetIndexAsync(TicketRequest.Index request);
        Task<TicketResult.Detail> GetDetailAsync(TicketRequest.Detail request);
        Task<TicketResult.Create> CreateAsync(TicketRequest.Create request);
        Task<TicketResult.Edit> EditAsync(TicketRequest.Edit request);
        Task DeleteAsync(TicketRequest.Delete request);
    }
}
