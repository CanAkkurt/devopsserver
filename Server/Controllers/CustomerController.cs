using Microsoft.AspNetCore.Mvc;
using Shared.Customers;
using Shared.Projects;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        this._customerService = customerService;
    }

    [SwaggerOperation("Returns a list of customers.")]
    [HttpGet]
    public async Task<CustomerResult.Index> GetIndex([FromQuery] CustomerRequest.Index request)
    {
        return await _customerService.GetIndexAsync(request);
    }

    [SwaggerOperation("Returns a specific customer.")]
    [HttpGet("{id}")]
    public async Task<CustomerResult.Detail> GetDetail([FromRoute] CustomerRequest.Detail request)
    {
        return await _customerService.GetDetailAsync(request);
    }
    [SwaggerOperation("Returns a specific customer's VMs.")]
    [HttpGet("{id}/projects")]
    public async Task<ProjectResult.Index> GetProjectsFromCustomer([FromRoute] CustomerRequest.Detail request)
    {
        return await _customerService.GetProjectsFromCustomerAsync(request);
    }

    [SwaggerOperation("Creates a new customer.")]
    [HttpPost]
    public async Task<CustomerResult.Create> Create([FromBody] CustomerRequest.Create request)
    {
        return await _customerService.CreateAsync(request);
    }

    [SwaggerOperation("Edites an existing customer.")]
    [HttpPut("{id}")]
    public async Task<CustomerResult.Edit> Edit([FromBody] CustomerRequest.Edit request)
    {
        return await _customerService.EditAsync(request);
    }

    [SwaggerOperation("Deletes an existing customer.")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] CustomerRequest.Delete request)
    {
        await _customerService.DeleteAsync(request);
        return NoContent();
    }
}
