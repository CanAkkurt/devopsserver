using Microsoft.AspNetCore.Mvc;
using Shared.Customers;
using Shared.Projects;
using Shared.Tickets;
using Shared.VirtualMachines;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VirtualMachineController : ControllerBase
{
    private readonly IVirtualMachineService _vmService;

    public VirtualMachineController(IVirtualMachineService vmService)
    {
        _vmService = vmService;
    }

    [SwaggerOperation("Returns a list of virtual machines.")]
    [HttpGet]
    public async Task<VirtualMachineResult.Index> GetIndex([FromQuery] VirtualMachineRequest.Index request)
    {
        return await _vmService.GetIndexAsync(request);
    }

    [SwaggerOperation("Returns a specific virtual machine.")]
    [HttpGet("{id}")]
    public async Task<VirtualMachineResult.Detail> GetDetail([FromRoute] VirtualMachineRequest.Detail request)
    {
        return await _vmService.GetDetailAsync(request);
    }
    [SwaggerOperation("Returns the tickets of a specific vm.")]
    [HttpGet("{id}/tickets")]
    public async Task<TicketResult.Index> GetTicketsFromVirtualMachine([FromRoute] VirtualMachineRequest.Detail request)
    {
        return await _vmService.GetTicketsFromVirtualMachineAsync(request);
    }
    [SwaggerOperation("Returns the customer of a specific vm.")]
    [HttpGet("{id}/customer")]
    public async Task<CustomerResult.Detail> GetCustomerFromVirtualMachine([FromRoute] VirtualMachineRequest.Detail request)
    {
        return await _vmService.GetCustomerFromVirtualMachineAsync(request);
    }

    [SwaggerOperation("Creates a new virtual machine.")]
    [HttpPost]
    public async Task<VirtualMachineResult.Create> Create([FromBody] VirtualMachineRequest.Create request)
    {
        return await _vmService.CreateAsync(request);
    }


    [SwaggerOperation("Edites an existing virtual machine.")]
    [HttpPut("{id}")]
    public async Task<VirtualMachineResult.Edit> Edit([FromBody] VirtualMachineRequest.Edit request)
    {
        return await _vmService.EditAsync(request);
    }


    [SwaggerOperation("Deletes an existing virtual machine.")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] VirtualMachineRequest.Delete request)
    {
        await _vmService.DeleteAsync(request);
        return NoContent();
    }

    [Route("/api/virtualmachine/analytics")]
    [SwaggerOperation("Gets the analytics for virtual machines.")]
    [HttpGet]
    public async Task<VirtualMachineResult.Usage> Usage()
    {
        return await _vmService.GetUsageAsync();
    }
	[SwaggerOperation("Returns the tickets of a specific vm.")]
	[Route("/api/virtualmachine/all")]
	public List<String> getallinfo()
    {
       var list = new List<String>();
        list.Add("test");
        list.Add("oke");
      

        return list;
    }
}

