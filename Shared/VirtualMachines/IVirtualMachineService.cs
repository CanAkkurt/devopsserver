using Domain;
using Shared.Customers;
using Shared.Tickets;

namespace Shared.VirtualMachines;

public interface IVirtualMachineService
{
    Task<VirtualMachineResult.Usage> GetUsageAsync();
    Task<VirtualMachineResult.Index> GetIndexAsync(VirtualMachineRequest.Index request);
    Task<VirtualMachineResult.Detail> GetDetailAsync(VirtualMachineRequest.Detail request);
    Task<TicketResult.Index> GetTicketsFromVirtualMachineAsync(VirtualMachineRequest.Detail request);
    Task<CustomerResult.Detail> GetCustomerFromVirtualMachineAsync(VirtualMachineRequest.Detail request);
    Task<VirtualMachineResult.Create> CreateAsync(VirtualMachineRequest.Create request);
    Task<VirtualMachineResult.Edit> EditAsync(VirtualMachineRequest.Edit request);
    Task DeleteAsync(VirtualMachineRequest.Delete request);
}
