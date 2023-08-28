using Shared.Projects;

namespace Shared.Customers;

public interface ICustomerService
{
    Task<CustomerResult.Index> GetIndexAsync(CustomerRequest.Index request);
    Task<CustomerResult.Detail> GetDetailAsync(CustomerRequest.Detail request);
    Task<ProjectResult.Index> GetProjectsFromCustomerAsync(CustomerRequest.Detail request);
    Task<CustomerResult.Create> CreateAsync(CustomerRequest.Create request);
    Task DeleteAsync(CustomerRequest.Delete request);
    Task<CustomerResult.Edit> EditAsync(CustomerRequest.Edit request);
}
